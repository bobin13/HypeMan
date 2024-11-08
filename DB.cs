using MongoDB.Driver;

namespace HypeMan
{
    public class DB
    {
        MongoClient client = new MongoClient("mongodb+srv://bobin13:4K8J276bWqmd5iBr@cluster0.wi0uk9y.mongodb.net/?retryWrites=true&w=majority");
        string dbName = "motivation";

        //returns a collectioin as a list whose name is passed.
        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {

            var db = client.GetDatabase(dbName);
            return db.GetCollection<T>(collectionName);

        }
        public List<Quote> GetAllQuotes()
        {
            var quotes = GetCollection<Quote>("quotes").AsQueryable().ToList();
            return quotes;
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
