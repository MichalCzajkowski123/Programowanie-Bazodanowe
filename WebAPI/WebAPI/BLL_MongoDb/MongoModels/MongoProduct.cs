using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Model;

namespace BLL_MongoDb.MongoModels
{
    public class Product
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Price")]
        public double Price { get; set; }
        [BsonElement("Image")]
        public string Image { get; set; }
        [BsonElement("IsActive")]
        public bool IsActive { get; set; }
        [BsonElement("GroupID")]
        public string? GroupID { get; set; }
        [BsonIgnoreIfNull]
        public ICollection<OrderPosition>? OrderPositions { get; set; }
        [BsonIgnoreIfNull]
        public ICollection<BasketPosition>? BasketPositions { get; set; }
        [BsonIgnoreIfNull]
        public ProductGroup? ProductGroup { get; set; }
    }
}
