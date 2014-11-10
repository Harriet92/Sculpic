using Kalambury.Database.Mongo;
using Kalambury.Mongo.Interfaces;
using Kalambury.Mongo.Mongo;
using Kalambury.WcfServer.Interfaces;
using Kalambury.WcfServer.Models;

namespace Kalambury.WcfServer.Repositories
{
    public class UserMongoRepository: MongoRepository<User>, IUserRepository
    {
        //TODO: Container resolve connection settings
        public UserMongoRepository(IDatabaseServer connectionSettings)
            : base((MongoDatabaseServer)connectionSettings, "Users")
        {
            
        }

        public User GetUserByUsername(string username)
        {
            return GetItemByQuery(x => x.Username == username);
        }
    }
}
