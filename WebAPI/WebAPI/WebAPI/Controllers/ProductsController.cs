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
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetProducts([FromQuery] string? nameFilter = null, [FromQuery] string? groupNameFilter = null,
                                         [FromQuery] int? groupIdFilter = null, [FromQuery] bool includeInactive = false,
                                         [FromQuery] string? sortBy = "Name", [FromQuery] bool descending = false)
        {
            var products = _productService.GetProducts(nameFilter, groupNameFilter, groupIdFilter, includeInactive, sortBy, descending);
            return Ok(products);
        }

        [HttpPost]
        public IActionResult AddProduct([FromBody] ProductRequestDTO productDto)
        {
            _productService.AddProduct(productDto);
            return CreatedAtAction(nameof(GetProducts), new { nameFilter = productDto.Name }, productDto);
        }

        [HttpPut("{productId}/deactivate")]
        public IActionResult DeactivateProduct(int productId)
        {
            _productService.DeactivateProduct(productId);
            return NoContent();
        }

        [HttpPut("{productId}/activate")]
        public IActionResult ActivateProduct(int productId)
        {
            _productService.ActivateProduct(productId);
            return NoContent();
        }

        [HttpDelete("{productId}")]
        public IActionResult DeleteProduct(int productId)
        {
            _productService.DeleteProduct(productId);
            return NoContent();
        }

        [HttpGet("{productId}/group-name")]
        public IActionResult GetFullGroupName(int productId)
        {
            var fullGroupName = _productService.GetFullGroupName(productId);
            return Ok(fullGroupName);
        }
    }
}
