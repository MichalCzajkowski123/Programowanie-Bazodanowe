using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_MongoDb.MongoModels
{
    public class ProductGroup
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("ParentID")]
        public string? ParentID { get; set; }
        [BsonIgnoreIfNull]
        public ProductGroup? ParentGroup { get; set; }
        [BsonIgnoreIfNull]
        public ICollection<Product>? Products { get; set; }
    }
}
