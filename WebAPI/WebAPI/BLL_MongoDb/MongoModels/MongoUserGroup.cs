using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_MongoDb.MongoModels
{
    public class UserGroup
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonIgnoreIfNull]
        public ICollection<User>? Users { get; set; }
    }
}
