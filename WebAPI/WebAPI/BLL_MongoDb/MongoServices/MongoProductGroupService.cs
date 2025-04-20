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
    public class MongoProductGroupService : IProductGroupService
    {
        private readonly IMongoCollection<ProductGroup> _groups;

        public MongoProductGroupService(IMongoDatabase database)
        {
            _groups = database.GetCollection<ProductGroup>("product_groups");
        }

        public IEnumerable<ProductGroupResponseDTO> GetProductGroups(int? parentGroupId = null, string? sortBy = "Name", bool descending = false)
        {
            FilterDefinition<ProductGroup> filter;

            if (parentGroupId.HasValue)
                filter = Builders<ProductGroup>.Filter.Eq(pg => pg.ParentID, parentGroupId.Value.ToString());
            else
                filter = Builders<ProductGroup>.Filter.Eq(pg => pg.ParentID, null);

            var productGroups = _groups.Find(filter).ToList();

            switch (sortBy?.ToLower())
            {
                case "name":
                    productGroups = descending ? productGroups.OrderByDescending(pg => pg.Name).ToList() : productGroups.OrderBy(pg => pg.Name).ToList();
                    break;
                case "id":
                    productGroups = descending ? productGroups.OrderByDescending(pg => pg.Id).ToList() : productGroups.OrderBy(pg => pg.Id).ToList();
                    break;
                default:
                    productGroups = productGroups.OrderBy(pg => pg.Name).ToList();
                    break;
            }

            var allGroups = _groups.Find(Builders<ProductGroup>.Filter.Empty).ToList();
            var result = productGroups.Select(pg => new ProductGroupResponseDTO
            {
                ID = pg.Id,
                Name = pg.Name,
                HasSubgroups = allGroups.Any(sub => sub.ParentID == pg.Id)
            }).ToList();

            return result;
        }

        public void AddProductGroup(ProductGroupRequestDTO groupDto)
        {
            if (groupDto.ParentID != null)
            {
                var parentExists = _groups.Find(pg => pg.Id == groupDto.ParentID).Any();
                if (!parentExists)
                {
                    throw new ArgumentException("Parent group does not exist.");
                }
            }

            var newGroup = new ProductGroup
            {
                Name = groupDto.Name,
                ParentID = groupDto.ParentID
            };

            _groups.InsertOne(newGroup);
        }
    }
}
