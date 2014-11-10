using System.Runtime.InteropServices;
using Kalambury.Database.Mongo;
using Kalambury.Mongo.Interfaces;
using Kalambury.WcfServer.Interfaces;
using Kalambury.WcfServer.Models;
using Kalambury.WcfServer.Repositories;

namespace Kalambury.WcfServer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        //For testing purposes
        public UserService()
        {
            IDatabaseServer serverConnection = new MongoDatabaseServer(
                new MongoConnectionSettings()
                {
                    DatabaseName = "TestUserDB",
                    Ip = "127.0.0.1",
                    Port = "27017"
                });
            userRepository = new UserMongoRepository(serverConnection);
        }

        public UserService(IDatabaseServer serverConnection)
        {
            userRepository = new UserMongoRepository(serverConnection);
        }

        public User LoginUser(string username, string password)
        {
            User user = userRepository.GetUserByUsername(username);
            if (user == null || user.Password != password) 
                return null;
            return user;
        }

        public User AddNewUser(string username, string password)
        {
            User user = LoginUser(username, password);
            if (user != null)
                return user;
            if (userRepository.GetUserByUsername(username) != null)
                return null;
            return userRepository.Insert(new User()
            {
                UserId = userRepository.CountAll(),
                Username = username,
                Password = password
            });
        }

        public string PingService()
        {
            return "OK!";
        }
    }
}
