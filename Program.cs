using HypeMan;
using MongoDB.Bson;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

class Program
{

    static void Main(string[] args)
    {

        //Loading variabbles from .env file
        var root = Directory.GetCurrentDirectory();
        var dotenv = Path.Combine(root, ".env");
        DotEnv.Load(dotenv);

        List<Contact> temp =
        [
            new Contact{
                _id = ObjectId.GenerateNewId(),
                name = "Bobin",
                phoneNumber = "+16479360747"
            },
        ];
        DB db = new();
        List<Contact> contacts = db.GetAllContacts();
        Quote quote = db.GetQuote();
        Console.WriteLine(quote.detail);

        //check to change property is_used to false for every quote,
        // if no quote is returned with property is_used as false.
        if (quote == null)
        {
            db.SetAllToFalse();
            quote = db.GetQuote();// getting a quote after setting all to false
        }

        if (quote != null && contacts.Count > 0)
        {
            SendMessage(temp, quote);

        }
        //setting quote is_used to true
        db.SetUsedToTrue(quote);
    }

    public static void SendMessage(List<Contact> contacts, Quote quote)
    {
        var accountSid = Environment.GetEnvironmentVariable("ACCOUNT_SID");
        var authToken = Environment.GetEnvironmentVariable("AUTH_TOKEN");
        // Initialize Twilio client
        TwilioClient.Init(accountSid, authToken);

        foreach (Contact contact in contacts)
        {
            //Creatring the message string
            var messageText = $"\n{contact.name}, your quote today:\n{quote.detail}\n-{quote.author}";

            var message = MessageResource.Create(
            body: messageText,
            from: new PhoneNumber("+14434291713"), // Your Twilio number
            to: new PhoneNumber(contact.phoneNumber) // Recipient's number
            );
        }


    }
}
