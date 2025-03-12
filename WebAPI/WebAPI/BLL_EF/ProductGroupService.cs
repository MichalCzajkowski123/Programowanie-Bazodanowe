using BLL.DTOModels;
using BLL.ServicesInterfaces;
using DAL.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Model;

namespace BLL_EF
{
    public class ProductGroupService : IProductGroupService
    {
        private readonly WebStoreContext _context;

        public ProductGroupService(WebStoreContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductGroupResponseDTO> GetProductGroups(int? parentGroupId = null, string? sortBy = "Name", bool descending = false)
        {
            var query = _context.ProductGroups
                .AsQueryable();

            if (parentGroupId.HasValue)
            {
                query = query.Where(pg => pg.ParentID == parentGroupId.Value);
            }
            else
            {
                query = query.Where(pg => pg.ParentID == null); 
            }

            switch (sortBy?.ToLower())
            {
                case "name":
                    query = descending ? query.OrderByDescending(pg => pg.Name) : query.OrderBy(pg => pg.Name);
                    break;
                case "id":
                    query = descending ? query.OrderByDescending(pg => pg.ID) : query.OrderBy(pg => pg.ID);
                    break;
                default:
                    query = query.OrderBy(pg => pg.Name);
                    break;
            }

            var result = query.Select(pg => new ProductGroupResponseDTO
            {
                ID = pg.ID,
                Name = pg.Name,
                HasSubgroups = _context.ProductGroups.Any(subGroup => subGroup.ParentID == pg.ID)
            }).ToList();

            return result;
        }

        public void AddProductGroup(ProductGroupRequestDTO groupDto)
        {
            if (groupDto.ParentID.HasValue)
            {
                var parentGroup = _context.ProductGroups.Find(groupDto.ParentID.Value);
                if (parentGroup == null)
                {
                    throw new ArgumentException("Parent group does not exist.");
                }
            }
            var productGroup = new ProductGroup
            {
                Name = groupDto.Name,
                ParentID = groupDto.ParentID
            };
            _context.ProductGroups.Add(productGroup);
            _context.SaveChanges();
        }
    }
}
