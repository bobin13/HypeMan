using HypeMan;
using MongoDB.Bson;
using Twilio;
using Twilio.Exceptions;
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

        DB db = new();
        if (args.Contains("--check-mongodb"))
        {
            var diagnosticContacts = db.GetActiveContacts();
            var diagnosticQuote = db.GetQuote();
            Console.WriteLine($"MongoDB connected. Active contacts: {diagnosticContacts.Count}; available quote: {diagnosticQuote != null}.");
            return;
        }

        // List<Contact> temp =
        // [
        //     new Contact{
        //         _id = ObjectId.GenerateNewId(),
        //         name = "Bobin",
        //         phoneNumber = "+16479360747"
        //     },
        // ];
        List<Contact> contacts = db.GetActiveContacts();
        Quote? quote = db.GetQuote();

        //check to change property is_used to false for every quote,
        // if no quote is returned with property is_used as false.
        if (quote == null)
        {
            db.SetAllToFalse();
            quote = db.GetQuote();// getting a quote after setting all to false
        }

        if (quote == null)
        {
            Console.Error.WriteLine("No quotes are available in MongoDB.");
            return;
        }

        if (contacts.Count == 0)
        {
            Console.WriteLine("No active contacts found; no messages sent.");
            return;
        }

        Console.WriteLine(quote.detail);
        try
        {
            SendMessage(contacts, quote);
        }
        catch (ApiException ex)
        {
            Console.Error.WriteLine($"Twilio rejected the message (HTTP {ex.Status}, code {ex.Code}): {ex.Message}");
            Console.Error.WriteLine("Check the Twilio account trial status, remaining balance, verified recipient numbers, and messaging restrictions.");
            Environment.ExitCode = 1;
            return;
        }

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
            from: new PhoneNumber("+13435035312"), // Your Twilio number
            to: new PhoneNumber(contact.phoneNumber) // Recipient's number
            );
        }


    }
}
