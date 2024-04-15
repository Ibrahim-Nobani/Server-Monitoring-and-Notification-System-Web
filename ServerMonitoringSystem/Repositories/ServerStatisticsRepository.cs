using MongoDB.Driver;
using ServerMonitoringAndNotification.Repositories;
using ServerMonitoringAndNotification.Models;

namespace ServerMonitoringAndNotification.Repositories
{
    public class ServerStatisticsRepository : IRepository<ServerStatistics>
    {
        private readonly IMongoCollection<ServerStatistics> _serverStatisticsCollection;

        public ServerStatisticsRepository(IMongoDatabase database)
        {
            _serverStatisticsCollection = database.GetCollection<ServerStatistics>("ServerStatistics");
        }

        public void Add(ServerStatistics serverStatistics)
        {
            _serverStatisticsCollection.InsertOne(serverStatistics);
        }

        public void Delete(ServerStatistics serverStatistics)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ServerStatistics> GetAll()
        {
            return _serverStatisticsCollection.Find(_ => true).ToList();
        }

        public void Update(ServerStatistics serverStatistics)
        {
            throw new NotImplementedException();
        }
    }
}