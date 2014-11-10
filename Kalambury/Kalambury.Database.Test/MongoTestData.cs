using System;
using System.Collections.Generic;
using System.Linq;
using Kalambury.Database.Interfaces;
using Kalambury.Database.Mongo;
using Kalambury.Mongo.Mongo;
using MongoDB.Bson;

namespace Kalambury.Mongo.Test
{
    public class TestModel
    {
        ObjectId Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
    }

    public class TestRepository : MongoRepository<TestModel>
    {
        public TestRepository(MongoDatabaseServer mongoDatabaseServer)
            : base(mongoDatabaseServer)
        {

        }
    }
    public class MongoTestData
    {
        public static IConnectionSettings TestLocalhostConnectionSettings = new MongoConnectionSettings()
        {
            DatabaseName = "Test",
            Ip = "127.0.0.1",
            Port = "27017"
        };

        public static IConnectionSettings InvalidLocalhostConnectionSettings = new MongoConnectionSettings()
        {
            DatabaseName = "Test",
            Ip = "127.0.0.1",
            Port = "8080"
        };
    }
}
