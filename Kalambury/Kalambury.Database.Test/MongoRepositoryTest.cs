using System.IO;
using FluentAssertions;
using Kalambury.Database.Mongo;
using Kalambury.Mongo.Mongo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Kalambury.Mongo.Test
{
    [TestClass]
    public class MongoRepositoryTest
    {
        private TestRepository testRepository;

        [TestInitialize]
        public void Initialize()
        {
            testRepository = new TestRepository(new MongoDatabaseServer(MongoTestData.TestLocalhostConnectionSettings));
        }

        [TestCleanup]
        public void Cleanup()
        {
            testRepository.Collection.Drop();
        }

        [TestCategory("Mongo MongoRepository")]
        [TestMethod]
        public void InsertItem_BasicTest_ShouldSucceed()
        {
            testRepository.Insert(new TestModel()
            {
                UserId = 0,
                Username = "Emilka"
            }).Should().NotBeNull();
            testRepository.Collection.Count().Should().Be(1);
            Cleanup();
        }
    }
}
