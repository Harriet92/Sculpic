using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Kalambury.Database.Interfaces;

namespace Kalambury.Database.Mongo
{
    public class MongoConnectionSettings : IConnectionSettings
    {
        public string Ip { get; set; }
        public string DatabaseName { get; set; }
        public string Port { get; set; }
        public string DatabaseUsername { get; set; }
        public string DatabaseUserPassword { get; set; }
        public string GetConnectionString()
        {
            StringBuilder connectionString = new StringBuilder();
            connectionString.Append("mongodb://");
            if (!string.IsNullOrEmpty(DatabaseUsername))
                connectionString.AppendFormat("{0}:{1}@", DatabaseUsername, DatabaseUserPassword);
            return connectionString.AppendFormat("{0}:{1}/{2}", Ip, Port, DatabaseName).ToString();
        }
    }

}
