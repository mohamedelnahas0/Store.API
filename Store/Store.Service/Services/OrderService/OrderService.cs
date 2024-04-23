using AutoMapper;
using Store.Data.Entities;
using Store.Data.Entities.OrderEntities;
using Store.Repository.BasketRepository;
using Store.Repository.BasketRepository.Models;
using Store.Repository.Interfaces;
using Store.Repository.Specification.Order;
using Store.Service.Services.BasketService.Dots;
using Store.Service.Services.OrderService.Dtos;
using Store.Service.Services.PaymentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitOfWork , IBasketRepository basketRepository ,IMapper mapper , IPaymentService paymentService )
        {
            _unitOfWork = unitOfWork; 
            _basketRepository = basketRepository;
            _mapper = mapper;
            _paymentService = paymentService;
        }
        public async Task<OrderResultDto> CreateOrderAsync(OrderDto input)
        {

            var basket = await _basketRepository.GetBasketAsync(input.BasketId);
            if (basket  is null)
                throw new Exception("Basket Not Found");
            var orderitems = new List<OrderItemDto>();
            var customerBasketDto = _mapper.Map<CustomerBasketDto>(basket);

            foreach (var basketItem in basket.BasketItems)
            {
                var productItem = await _unitOfWork.Repository<Product, int>().GetByIdAsync(basketItem.ProductId);
                if (productItem is null)
                    throw new Exception("Product Not Exist");
                var itemOrdered = new ProductItemOrder
                {
                    ProductItemId = productItem.Id,
                    ProductName = productItem.Name,
                    PictureUrl = productItem.PictureUrl
                };
                var orderItem = new OrderItem
                {
                    price = productItem.Price,
                    Quantity = basketItem.Quantity,
                    ItemOrder = itemOrdered
                };
                var mappedOrderItem = _mapper.Map<OrderItemDto>(orderItem);
                orderitems.Add(mappedOrderItem);
            }
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(input.DeliveryMethodId);
            if (deliveryMethod is null)
                throw new Exception("Delivery method Not provided");
            var subTotal = orderitems.Sum(item => item.Quantity * item.price);

            var specs = new OrderwithpaymentintentSpecification(basket.PaymentIntentId);
            var exsitingorder = await _unitOfWork.Repository<Order, Guid>().GetWithSpecificationByIdAsync(specs);
            if (exsitingorder != null)
            {

                _unitOfWork.Repository<Order, Guid>().Delete(exsitingorder);
               await _paymentService.CreateOrUpdatePaymentIntentForExistingOrder(customerBasketDto);
            }
            else
            {
                await _paymentService.CreateOrUpdatePaymentIntentForNewOrder(basket.Id);
            }
            var mappedshippingAddress = _mapper.Map<ShippingAddress>(input.ShipingAddress);
            var mappedOrderItems = _mapper.Map<List<OrderItem>>(orderitems);
            var order = new Order
            {
                DeliveryMethodId = deliveryMethod.Id,
                ShippingAddress=mappedshippingAddress,
                BuyerEmail=input.BuyerEmail,
                OrderItems = mappedOrderItems,
                SubTotal = subTotal,
                basketId=basket.Id,
            };
            await _unitOfWork.Repository<Order, Guid>().AddAsync(order);
            await _unitOfWork.CompleteAsync();
            var mappedOrder = _mapper.Map<OrderResultDto>(order);

            return mappedOrder;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodAsync()
             => await _unitOfWork.Repository<DeliveryMethod,int >().GetAllAsync();

        public async Task<IReadOnlyList<OrderResultDto>> GetAllOrderForUserAsync(string BuyerEmail)
        {
            var spec = new OrderWithItemSpecification(BuyerEmail);
            var orders = await _unitOfWork.Repository<Order, Guid>().GetAllWithSpecificationAsync(spec);

            if (orders is { Count: <= 0 })
                throw new Exception("Current user is not have any orders yet");



            var mappedOrders = _mapper.Map<List<OrderResultDto>>(orders);
           
            return mappedOrders;
        }

        public async Task<OrderResultDto> GetOrderByIdAsync(Guid id, string BuyerEmail)
        {
            var spec = new OrderWithItemSpecification( id,BuyerEmail);

            var order = await _unitOfWork.Repository<Order, Guid>().GetWithSpecificationByIdAsync(spec);



            var mappedOrders = _mapper.Map<OrderResultDto>(order);
            return mappedOrders;
        }
    }


}
