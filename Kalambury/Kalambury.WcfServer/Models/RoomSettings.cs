using System.Runtime.Serialization;

namespace Kalambury.WcfServer.Models
{
    [DataContract]
    public class RoomSettings
    {
        [DataMember]
        public string GameName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public int UsersLimit { get; set; }
    }
}
