using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Model;
using WebAPI.Model;
using BLL.ServicesInterfaces;

namespace WebAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("{userId}")]
        public IActionResult GenerateOrder(int userId)
        {
            _orderService.GenerateOrder(userId);
            return Ok();
        }

        [HttpPut("{orderId}/pay")]
        public IActionResult PayOrder(int orderId, [FromBody] double amount)
        {
            _orderService.PayOrder(orderId, amount);
            return NoContent();
        }

        [HttpGet]
        public IActionResult GetOrders([FromQuery] bool? paidStatus = null, [FromQuery] int? orderId = null,
                                       [FromQuery] string? sortBy = "Date", [FromQuery] bool descending = false)
        {
            var orders = _orderService.GetOrders(paidStatus, orderId, sortBy, descending);
            return Ok(orders);
        }

        [HttpGet("{orderId}/details")]
        public IActionResult GetOrderDetails(int orderId)
        {
            var details = _orderService.GetOrderDetails(orderId);
            return Ok(details);
        }
    }
}
