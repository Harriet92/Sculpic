using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
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
            //TODO: Container resolve
        }

        public User GetUser(int userId)
        {
            return userRepository.GetItemByQuery(x => x.UserId == userId);
        }

        public User AddNewUser(string username)
        {
            return userRepository.Insert(new User()
            {
                UserId = userRepository.CountAll(),
                Username = username
            });
        }


        public string PingService()
        {
            return "OK!";
        }
    }
}
