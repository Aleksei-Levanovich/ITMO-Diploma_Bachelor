using MongoDB.Bson;
using MongoDB.Driver;

namespace Diploma
{
    public class MongoConnect
    {
        public static IMongoCollection<BsonDocument> ConnectMongoDB(string collection) {
            MongoClient client = new MongoClient("mongodb://127.0.0.1:27017");
            IMongoDatabase mongodb = client.GetDatabase("Staff");
            IMongoCollection<BsonDocument> mongoCollection = mongodb.GetCollection<BsonDocument>(collection);
            return mongoCollection;
        }
    }
}
