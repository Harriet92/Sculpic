using Kalambury.Database.Interfaces;

namespace Kalambury.Mongo.Interfaces
{
    public interface IDatabaseServer
    {
        bool Connect();
        void Disconnect();
        void SetConnectionSettings(IConnectionSettings connection);
        bool IsConnected { get; }
    }
}
