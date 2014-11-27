using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kalambury.WcfServer.Models
{
    [DataContract]
    public class User
    {
        [BsonIgnore]
        public const int MAX_USERNAME_LEN = 15;
        [BsonIgnore]
        public const int MIN_USERNAME_LEN = 2;

        public ObjectId Id { get; set; }
        [DataMember]
        public Int64 UserId { get; set; }
        [DataMember]
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
