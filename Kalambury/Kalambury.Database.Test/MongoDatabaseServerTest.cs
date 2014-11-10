using System;
using FluentAssertions;
using Kalambury.Database.Interfaces;
using Kalambury.Database.Mongo;
using Kalambury.Mongo.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kalambury.Mongo.Test
{
    [TestClass]
    public class MongoDatabaseServerTest
    {
        private IConnectionSettings connectionSettings;
        private MongoDatabaseServer mongoDatabaseServer;

        [TestInitialize]
        public void Initialize()
        {
            connectionSettings = MongoTestData.TestLocalhostConnectionSettings;
            mongoDatabaseServer = new MongoDatabaseServer(connectionSettings);
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        //Remember to ensure that Mongo server is running at localhost!
        [TestCategory("Mongo MongoDatabaseServer")]
        [TestMethod]
        public void Connect_BasicTest_ShouldSucceed()
        {
            mongoDatabaseServer.Connect().Should().BeTrue();
        }

        [TestCategory("Mongo MongoDatabaseServer")]
        [TestMethod]
        public void Connect_InvalidSettingsProvided_ShouldReturnFalse()
        {
            var wrongMongoServer = new MongoDatabaseServer(MongoTestData.InvalidLocalhostConnectionSettings);
            wrongMongoServer.Connect().Should().BeFalse();
        }

        [TestCategory("Mongo MongoDatabaseServer")]
        [TestMethod]
        public void GetMongoDatabase_BasicTest_ShouldSucceed()
        {
            mongoDatabaseServer.Connect();
            mongoDatabaseServer.GetMongoDatabase().Should().NotBeNull();
        }

        [TestCategory("Mongo MongoDatabaseServer")]
        [TestMethod]
        public void GetMongoCollection_BasicTest_ShouldSucceed()
        {
            mongoDatabaseServer.Connect();
            mongoDatabaseServer.GetMongoCollection<string>("IntItems").Should().NotBeNull();
        }
    }
}
