using BLL.DTOModels;
using BLL.ServicesInterfaces;
using BLL_MongoDb.MongoModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_MongoDb.MongoServices
{
    public class MongoProductService : IProductService
    {
        private readonly IMongoCollection<Product> _products;
        private readonly IMongoCollection<ProductGroup> _groups;

        public MongoProductService(IMongoDatabase database)
        {
            _products = database.GetCollection<Product>("products");
            _groups = database.GetCollection<ProductGroup>("product_groups");
        }

        public IEnumerable<ProductResponseDTO> GetProducts(string? nameFilter = null, string? groupNameFilter = null, int? groupIdFilter = null, bool includeInactive = false, string? sortBy = "Name", bool descending = false)
        {
            var filterBuilder = Builders<Product>.Filter;
            var filters = new List<FilterDefinition<Product>>();

            if (!string.IsNullOrEmpty(nameFilter))
                filters.Add(filterBuilder.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(nameFilter, "i")));

            if (!includeInactive)
                filters.Add(filterBuilder.Eq(p => p.IsActive, true));

            if (groupIdFilter.HasValue)
            {
                var groupIds = GetAllGroupIds(groupIdFilter.Value.ToString());
                filters.Add(filterBuilder.In(p => p.GroupID, groupIds));
            }

            var filter = filters.Any() ? filterBuilder.And(filters) : filterBuilder.Empty;

            var products = _products.Find(filter).ToList();
            var groups = _groups.Find(Builders<ProductGroup>.Filter.Empty).ToList();

            var result = products
                .Select(p => new
                {
                    Product = p,
                    FullGroupName = p.GroupID != null ? GetFullGroupName(p.GroupID, groups) : null
                })
                .Where(p => string.IsNullOrEmpty(groupNameFilter) || (p.FullGroupName != null && p.FullGroupName.Contains(groupNameFilter)))
                .Select(p => new ProductResponseDTO
                {
                    ID = p.Product.Id,
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
                GroupID = productDto.GroupID,
                IsActive = true
            };
            _products.InsertOne(product);
        }

        public void DeactivateProduct(int productId)
        {
            var update = Builders<Product>.Update.Set(p => p.IsActive, false);
            _products.UpdateOne(p => p.Id == productId.ToString(), update);
        }

        public void ActivateProduct(int productId)
        {
            var update = Builders<Product>.Update.Set(p => p.IsActive, true);
            _products.UpdateOne(p => p.Id == productId.ToString(), update);
        }

        public void DeleteProduct(int productId)
        {
            _products.DeleteOne(p => p.Id == productId.ToString());
        }

        public List<string> GetAllGroupIds(string groupId)
        {
            var groupIds = new List<string> { groupId };

            var subGroups = _groups.Find(g => g.ParentID == groupId).ToList();

            foreach (var subGroup in subGroups)
            {
                groupIds.AddRange(GetAllGroupIds(subGroup.Id));
            }

            return groupIds;
        }

        public string GetFullGroupName(string? groupId, List<ProductGroup> allGroups)
        {
            if (groupId == null) return string.Empty;

            var names = new List<string>();
            var current = allGroups.FirstOrDefault(g => g.Id == groupId);

            while (current != null)
            {
                names.Add(current.Name);
                current = allGroups.FirstOrDefault(g => g.Id == current.ParentID);
            }

            names.Reverse();
            return string.Join(" / ", names);
        }
    }
}
