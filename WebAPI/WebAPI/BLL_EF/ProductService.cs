using BLL.DTOModels;
using BLL.ServicesInterfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Model;

namespace BLL_EF
{
    public class ProductService : IProductService
    {
        private readonly WebStoreContext _context;

        public ProductService(WebStoreContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductResponseDTO> GetProducts(
            string? nameFilter = null, string? groupNameFilter = null, int? groupIdFilter = null,
            bool includeInactive = false, string? sortBy = "Name", bool descending = false)
        {
            var query = _context.Products
                .Include(p => p.ProductGroup)
                .AsQueryable();

            if (!string.IsNullOrEmpty(nameFilter))
            {
                query = query.Where(p => p.Name.Contains(nameFilter));
            }

            if (!string.IsNullOrEmpty(groupNameFilter))
            {
                query = query.Where(p => p.ProductGroup != null && p.ProductGroup.Name.Contains(groupNameFilter));
            }

            if (groupIdFilter.HasValue)
            {
                query = query.Where(p => p.GroupID == groupIdFilter.Value);
            }

            if (!includeInactive)
            {
                query = query.Where(p => p.IsActive);
            }

            switch (sortBy?.ToLower())
            {
                case "name":
                    query = descending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                    break;
                case "price":
                    query = descending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price);
                    break;
                default:
                    query = query.OrderBy(p => p.Name);
                    break;
            }

            var result = query.Select(p => new ProductResponseDTO
            {
                ID = p.ID,
                Name = p.Name,
                Price = p.Price,
                GroupName = p.ProductGroup != null ? p.ProductGroup.Name : null
            }).ToList();

            return result;
        }

        public void AddProduct(ProductRequestDTO productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                GroupID = productDto.GroupID
            };

            _context.Products.Add(product);
            _context.SaveChanges();
        }
        public string GetFullGroupName(int productId)
        {
            var product = _context.Products
                .Include(p => p.ProductGroup)
                .ThenInclude(pg => pg.ParentGroup)
                .FirstOrDefault(p => p.ID == productId);

            if (product?.ProductGroup == null)
            {
                throw new ArgumentException("No group or product");
            }

            var groupNames = new List<string>();
            var currentGroup = product.ProductGroup;

            while (currentGroup != null)
            {
                groupNames.Add(currentGroup.Name);
                currentGroup = _context.ProductGroups
                    .Include(pg => pg.ParentGroup)
                    .FirstOrDefault(pg => pg.ID == currentGroup.ParentID);
            }

            groupNames.Reverse();

            return string.Join(" / ", groupNames);
        }
        public void DeactivateProduct(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                product.IsActive = false;
                _context.SaveChanges();
            }
        }

        public void ActivateProduct(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                product.IsActive = true;
                _context.SaveChanges();
            }
        }

        public void DeleteProduct(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
