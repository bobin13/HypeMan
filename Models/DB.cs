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
            MongoClient client = new MongoClient($"mongodb+srv://{username}:{password}@cluster0.wi0uk9y.mongodb.net/?retryWrites=true&w=majority");

            var db = client.GetDatabase(dbName);
            return db.GetCollection<T>(collectionName);

        }
        public Quote GetQuote()
        {
            Quote quote = GetCollection<Quote>("quotes").Aggregate().Sample(1).FirstOrDefault();
            return quote;
        }

        public List<Contact> GetAllContacts()
        {
            var contacts = GetCollection<Contact>("contacts").AsQueryable().ToList();
            return contacts;
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
