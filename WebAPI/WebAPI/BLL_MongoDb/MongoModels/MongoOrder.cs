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
    public class Order
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement("UserID")]
        public string UserID { get; set; }
        public bool IsPaid { get; set; }
        public DateTime Date { get; set; }
        [BsonIgnoreIfNull]
        public User? User { get; set; }
        [BsonIgnoreIfNull]
        public ICollection<OrderPosition>? OrderPositions { get; set; }
    }
}
