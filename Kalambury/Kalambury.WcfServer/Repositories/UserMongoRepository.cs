﻿using System.Collections.Generic;
using System.Linq;
using Kalambury.Database.Mongo;
using Kalambury.Mongo.Interfaces;
using Kalambury.Mongo.Mongo;
using Kalambury.WcfServer.Interfaces;
using Kalambury.WcfServer.Models;
using MongoDB.Driver.Builders;

namespace Kalambury.WcfServer.Repositories
{
    public class UserMongoRepository: MongoRepository<User>, IUserRepository
    {
        //TODO: Container resolve connection settings
        public UserMongoRepository(IDatabaseServer connectionSettings)
            : base((MongoDatabaseServer)connectionSettings, "Users")
        {
            
        }

        public bool IsUsernameUnique(string username)
        {
            return GetUserByUsername(username) == null;
        }

        public User GetUserByUsername(string username)
        {
            return GetItemByQuery(x => x.Username == username);
        }

        public List<User> GetUsersByRanking(int count)
        {
            return Collection.FindAll()
                .SetSortOrder(SortBy.Descending("Ranking"))
                .SetLimit(count)
                .ToList();
        }
    }
}
