using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Data.Entities;
using Store.Service.HandelResponses;
using Store.Service.Services.OrderService;
using Store.Service.Services.OrderService.Dtos;
using Stripe;
using Stripe.Climate;
using System.Reflection.Metadata;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Routing.Matching;
using System.Drawing;

namespace Store.API.Controllers
{

    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost]
        public async Task<ActionResult<OrderResultDto>> CreateOrderAsync(OrderDto input)
        {
            var order = await _orderService.CreateOrderAsync(input);
            if (order == null)
            {
                return BadRequest(new Response(400, "error while create your order"));
            }
            return Ok(order);
        }




        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderResultDto>>> GetAllOrdersForuserAsync()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetAllOrderForUserAsync(email);
            return Ok(orders);

        }
        [HttpGet]
        public async Task<ActionResult<OrderResultDto>> GetOrderByIdAsync(Guid id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrderByIdAsync(id, email);
            return Ok(orders);

        }

        [HttpGet]

        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetAllDeliveryMethodAsync()
             => Ok(await _orderService.GetAllDeliveryMethodAsync());




         }
    }



         
         
         
         