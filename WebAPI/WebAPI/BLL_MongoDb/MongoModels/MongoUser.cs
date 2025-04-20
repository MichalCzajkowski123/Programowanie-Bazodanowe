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
    public class User
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement("Login")]
        public string Login { get; set; }
        [BsonElement("Password")]
        public string Password { get; set; }
        [BsonElement("Type")]
        public string Type { get; set; }
        [BsonElement("IsActive")]
        public bool IsActive { get; set; }
        [BsonElement("GroupID")]
        public string? GroupID { get; set; }
        [BsonIgnoreIfNull]
        public UserGroup? UserGroup { get; set; }
    }
}
