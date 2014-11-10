using Kalambury.Mongo.Interfaces;
using Kalambury.WcfServer.Models;

namespace Kalambury.WcfServer.Interfaces
{
    public interface IUserRepository: IRepository<User>
    {
        User GetUserByUsername(string username);
    }
}
