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
    [Route("api/product-groups")]
    [ApiController]
    public class ProductGroupController : ControllerBase
    {
        private readonly IProductGroupService _productGroupService;

        public ProductGroupController(IProductGroupService productGroupService)
        {
            _productGroupService = productGroupService;
        }

        [HttpGet]
        public IActionResult GetProductGroups([FromQuery] int? parentGroupId = null, [FromQuery] string? sortBy = "Name", [FromQuery] bool descending = false)
        {
            var groups = _productGroupService.GetProductGroups(parentGroupId, sortBy, descending);
            return Ok(groups);
        }

        [HttpPost]
        public IActionResult AddProductGroup([FromBody] ProductGroupRequestDTO groupDto)
        {
            _productGroupService.AddProductGroup(groupDto);
            return CreatedAtAction(nameof(GetProductGroups), new { parentGroupId = groupDto.ParentID }, groupDto);
        }
    }
}
