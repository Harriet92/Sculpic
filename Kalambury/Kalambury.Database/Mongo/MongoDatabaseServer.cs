using Kalambury.Database.Interfaces;
using Kalambury.Mongo.Interfaces;
using MongoDB.Driver;

namespace Kalambury.Database.Mongo
{
    public class MongoDatabaseServer : IDatabaseServer
    {
        private MongoServer server;
        private string databaseName;

        public bool IsConnected
        {
            get { return server != null && server.State == MongoServerState.Connected; }
        }
        public MongoDatabaseServer(IConnectionSettings connectionSetting)
        {
            SetConnectionSettings(connectionSetting);
        }

        public bool Connect()
        {
            try
            {
                server.Connect();
            }
            catch (MongoConnectionException e)
            {
                //TODO: Logging a connection error
            }

            return IsConnected;
        }

        public void Disconnect()
        {
            server.Disconnect();
        }

        public MongoDatabase GetMongoDatabase()
        {
            return server.GetDatabase(databaseName);
        }

        public MongoCollection<T1> GetMongoCollection<T1>(string collectionName)
        {
            return GetMongoDatabase().GetCollection<T1>(collectionName);
        }

        public void SetConnectionSettings(IConnectionSettings connectionSetting)
        {
            databaseName = connectionSetting.DatabaseName;
            var client = new MongoClient(connectionSetting.GetConnectionString());
            server = client.GetServer();
        }
    }
}
