using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Model;

namespace BLL_MongoDb.MongoModels
{
    public class BasketPosition
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement("ProductID")]
        public string ProductID { get; set; }
        [BsonElement("UserID")]
        public string UserID { get; set; }
        [BsonElement("amount")]
        public int Amount { get; set; }
        [BsonIgnoreIfNull]
        public Product? Product { get; set; }
        [BsonIgnoreIfNull]
        public User? User { get; set; }
    }
}
