using AutoMapper;
using Store.Repository.BasketRepository;
using Store.Repository.BasketRepository.Models;
using Store.Service.Services.BasketService.Dots;

namespace Store.Service.Services.BasketService
{
    public class BasketService : IBasketService
    {
        private IBasketRepository _basketRepositry;
        private IMapper _mapper;

        public BasketService(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepositry = basketRepository;
            _mapper = mapper;
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        => await _basketRepositry.DeleteBasketAsync(basketId);
        

        public async Task<CustomerBasketDto> GetBasketAsync(string basketId)
        {
            var basket = await _basketRepositry.GetBasketAsync(basketId);
            if (basket is null)
                return new CustomerBasketDto();
            var mappedBasket = _mapper.Map<CustomerBasketDto>(basket);
            return mappedBasket;
        }

        public async Task<CustomerBasketDto> UpdateBasketAsync(CustomerBasketDto basket)
        {
            var customerBasket = _mapper.Map<CustomerBasket>(basket);
            var updatedBasket = await _basketRepositry.UpdateBasketAsync(customerBasket);
            var mappedCustomerBasket = _mapper.Map<CustomerBasketDto>(updatedBasket);
            return mappedCustomerBasket;
        }
    }
}
