using System;
using HypeMan;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

class Program
{

    static void Main(string[] args)
    {

        //Loading variabbles from .env file
        var root = Directory.GetCurrentDirectory();
        var dotenv = Path.Combine(root, "../../../.env");
        DotEnv.Load(dotenv);
	Console.WriteLine(root);

        DB db = new();
        List<Contact> contacts = db.GetAllContacts();
        Quote quote = db.GetQuote();

        if (quote != null && contacts.Count > 0)
        {
            SendMessage(contacts, quote);
        }

    }

    public static void SendMessage(List<Contact> contacts, Quote quote)
    {
        var accountSid = "AC5eceb5eaaa2847554ab7c6ed2984dce3";
        var authToken = "c151777ef7a128cdd77718e1b9811384";
        // Initialize Twilio client
        TwilioClient.Init(accountSid, authToken);

        foreach (Contact contact in contacts)
        {
            //Creatring the message string
            var messageText = $"\n{contact.name}, your quote of the day:\n{quote.detail}\n-{quote.author}";
	
            var message = MessageResource.Create(
            body: messageText,
            from: new PhoneNumber("+14434291713"), // Your Twilio number
            to: new PhoneNumber(contact.phoneNumber) // Recipient's number
            );
        }
    }
}
