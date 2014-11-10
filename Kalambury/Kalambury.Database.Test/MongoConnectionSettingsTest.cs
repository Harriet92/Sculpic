using Kalambury.Database.Mongo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace Kalambury.Database.Test
{
    [TestClass]
    public class MongoConnectionSettingsTest
    {
        [TestCategory("Mongo MongoConnectionSettings")]
        [TestMethod]
        public void GetConnectionString_BasicTest_ShouldReturnCorrectString()
        {
            MongoConnectionSettings settings = new MongoConnectionSettings()
            {
                DatabaseUsername = "Emilka",
                DatabaseName = "TestDatabase",
                DatabaseUserPassword = "Lol1234",
                Ip = "192.168.1.2",
                Port = "8083"
            };
            settings.GetConnectionString().Should().Be("mongodb://Emilka:Lol1234@192.168.1.2:8083/TestDatabase");
        }

        [TestCategory("Mongo MongoConnectionSettings")]
        [TestMethod]
        public void GetConnectionString_EmptyUsername_ShouldReturnCorrectString()
        {
            MongoConnectionSettings settings = new MongoConnectionSettings()
            {
                DatabaseName = "TestDatabase",
                DatabaseUserPassword = "Lol1234",
                Ip = "192.168.1.2",
                Port = "8083"
            };
            settings.GetConnectionString().Should().Be("mongodb://192.168.1.2:8083/TestDatabase");
        }
    }
}
