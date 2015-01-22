using System.Collections.Generic;
using Kalambury.Mongo.Interfaces;
using Kalambury.WcfServer.Models;

namespace Kalambury.WcfServer.Interfaces
{
    public interface IUserRepository: IRepository<User>
    {
        User GetUserByUsername(string username);
        bool IsUsernameUnique(string username);
        List<User> GetUsersByRanking(int count);
    }
}
