using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kalambury.Mongo.Interfaces;
using Kalambury.WcfServer.Models;

namespace Kalambury.WcfServer.Interfaces
{
    public interface IUserRepository: IRepository<User>
    {

    }
}
