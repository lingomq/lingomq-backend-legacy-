using MongoDB.Driver;

namespace AppStatistics.BusinessLayer.Contracts.DataAccess
{
    public interface IMongoDbFactory
    {
        IMongoCollection<T> GetCollection<T>(string dbName, string collectionName) where T : class;
    }
}
