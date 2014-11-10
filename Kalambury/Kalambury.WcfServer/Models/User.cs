using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Kalambury.WcfServer.Models
{
    [DataContract]
    public class User
    {
        public ObjectId Id { get; set; }
        [DataMember]
        public Int64 UserId { get; set; }
        [DataMember]
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
