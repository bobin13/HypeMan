using System;
using HypeMan;
using MongoDB.Driver.Linq;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

class Program
{

    static void Main(string[] args)
    {

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
        const string accountSid = "AC5eceb5eaaa2847554ab7c6ed2984dce3";
        const string authToken = "850bda67f16d24f2b486d0254bd4b0eb";
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
