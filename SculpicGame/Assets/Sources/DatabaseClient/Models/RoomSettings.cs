using System;

namespace Assets.Sources.DatabaseClient.Models
{
    [Serializable]
    public class RoomSettings
    {
        public string GameName { get; set; }
        public string Password { get; set; }
        public int UsersLimit { get; set; }

        public RoomSettings()
        {
            
        }
    }
}
