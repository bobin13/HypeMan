using MongoDB.Bson;
using MongoDB.Driver;

namespace HypeMan
{
    public class DB
    {
        string dbName = "motivation";

        //returns a collectioin as a list whose name is passed.
        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            var username = Environment.GetEnvironmentVariable("MONGO_USERNAME");
            var password = Environment.GetEnvironmentVariable("MONGO_PASSWORD");

            if (string.IsNullOrWhiteSpace(username))
                throw new InvalidOperationException("Missing required MongoDB environment variable: MONGO_USERNAME.");
            if (string.IsNullOrWhiteSpace(password))
                throw new InvalidOperationException("Missing required MongoDB environment variable: MONGO_PASSWORD.");

            MongoClient client = new MongoClient($"mongodb+srv://{Uri.EscapeDataString(username)}:{Uri.EscapeDataString(password)}@cluster0.wi0uk9y.mongodb.net/?retryWrites=true&w=majority");
            var db = client.GetDatabase(dbName);
            return db.GetCollection<T>(collectionName);

        }
        public Quote? GetQuote()
        {
            var filter = Builders<Quote>.Filter.Eq("is_used", false);
            Quote quote = GetCollection<Quote>("quotes").Find(filter).FirstOrDefault();

            return quote;
        }

        public void SetUsedToTrue(Quote quote)
        {
            var filter = Builders<Quote>.Filter.Eq("_id", quote._id);
            var update = Builders<Quote>.Update.Set("is_used", true);

            GetCollection<Quote>("quotes").UpdateOne(filter, update);

        }

        public void SetAllToFalse()
        {
            var filter = Builders<Quote>.Filter.Eq("is_used", true);
            var update = Builders<Quote>.Update.Set("is_used", false);
            GetCollection<Quote>("quotes").UpdateMany(filter, update);
        }

        public List<Contact> GetActiveContacts()
        {
            var filter = Builders<Contact>.Filter.Eq(contact => contact.active, true);
            return GetCollection<Contact>("contacts").Find(filter).ToList();
        }


        public bool AddQuote(Quote Quote)
        {
            if (Quote == null)
                return false;

            var collection = GetCollection<Quote>("quotes");
            if (collection == null)
                return false;

            collection.InsertOne(Quote);

            return true; //returns true if Quote added.
        }


    }
}
