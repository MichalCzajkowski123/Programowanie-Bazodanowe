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
    public class OrderPosition
    {
        [BsonElement("OrderID")]
        public string OrderID { get; set; }
        [BsonElement("ProductID")]
        public string ProductID { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        [BsonIgnoreIfNull]
        public Order? Order { get; set; }
        [BsonIgnoreIfNull]
        public Product? Product { get; set; }
    }
}
