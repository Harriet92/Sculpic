using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalambury.Database.Interfaces
{
    public interface IConnectionSettings
    {
        string Ip { get; set; }
        string DatabaseName { get; set; }
        string Port { get; set; }
        string DatabaseUsername { get; set; }
        string DatabaseUserPassword { get; set; }
        string GetConnectionString();
    }
}
