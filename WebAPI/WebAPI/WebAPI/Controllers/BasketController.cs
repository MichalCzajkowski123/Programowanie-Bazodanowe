using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Model;
using WebAPI.Model;
using BLL.DTOModels;
using BLL.ServicesInterfaces;

namespace WebAPI.Controllers
{
    [Route("api/basket")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }
        [HttpGet("{userId}")]
        public IActionResult GetUserBasket(int userId)
        {
            var basketItems = _basketService.GetUserBasket(userId);
            return Ok(basketItems);
        }
        [HttpPost]
        public IActionResult AddToBasket([FromBody] BasketRequestDTO basketDto)
        {
            _basketService.AddToBasket(basketDto);
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateBasketAmount([FromQuery] int productId, [FromQuery] int userId, [FromQuery] int newAmount)
        {
            _basketService.UpdateBasketAmount(productId, userId, newAmount);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult RemoveFromBasket([FromQuery] int productId, [FromQuery] int userId)
        {
            _basketService.RemoveFromBasket(productId, userId);
            return NoContent();
        }
    }
}
