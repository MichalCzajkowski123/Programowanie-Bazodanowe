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

            if (groupIdFilter.HasValue)
            {
                var groupIds = GetAllGroupIds(groupIdFilter.Value);
                query = query.Where(p => p.GroupID.HasValue && groupIds.Contains(p.GroupID.Value));
            }

            if (!includeInactive)
            {
                query = query.Where(p => p.IsActive);
            }

            var products = query.ToList();

            var result = products
                .Select(p => new
                {
                    Product = p,
                    FullGroupName = p.ProductGroup != null ? GetFullGroupName(p.ProductGroup.ID) : null
                })
                .Where(p => string.IsNullOrEmpty(groupNameFilter) || (p.FullGroupName != null && p.FullGroupName.Contains(groupNameFilter)))
                .Select(p => new ProductResponseDTO
                {
                    ID = p.Product.ID,
                    Name = p.Product.Name,
                    Price = p.Product.Price,
                    GroupName = p.FullGroupName
                })
                .ToList();

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
        public List<int> GetAllGroupIds(int groupId)
        {
            var groupIds = new List<int> { groupId };

            var subGroups = _context.ProductGroups
                .Where(g => g.ParentID == groupId)
                .Select(g => g.ID)
                .ToList();

            foreach (var subGroupId in subGroups)
            {
                groupIds.AddRange(GetAllGroupIds(subGroupId));
            }

            return groupIds;
        }

        public string GetFullGroupName(int groupId)
        {
            var groupNames = new List<string>();
            var currentGroup = _context.ProductGroups
                .Where(g => g.ID == groupId)
                .Select(g => new ProductGroupRequestDTO
                {
                    ID = g.ID,
                    Name = g.Name,
                    ParentID = g.ParentID
                })
                .FirstOrDefault();

            while (currentGroup != null)
            {
                groupNames.Add(currentGroup.Name);
                currentGroup = _context.ProductGroups
                    .Where(g => g.ID == currentGroup.ParentID)
                    .Select(g => new ProductGroupRequestDTO
                    {
                        ID = g.ID,
                        Name = g.Name,
                        ParentID = g.ParentID
                    })
                    .FirstOrDefault();
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
