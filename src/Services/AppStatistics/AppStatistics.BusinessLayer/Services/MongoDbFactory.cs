using AppStatistics.BusinessLayer.Contracts.DataAccess;
using MongoDB.Driver;

namespace AppStatistics.BusinessLayer.Services
{
    public class MongoDbFactory : IMongoDbFactory
    {
        private readonly IMongoClient _client;

        public MongoDbFactory(string connectionString)
        {
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            _client = new MongoClient(settings);
        }

        public IMongoCollection<T> GetCollection<T>(string dbName, string collectionName) where T : class =>
            _client.GetDatabase(dbName).GetCollection<T>(collectionName);
    }
}
