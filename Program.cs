using System;
using HypeMan;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

class Program
{

    static void Main(string[] args)
    {

        // twilio creds
        const string accountSid = "AC5eceb5eaaa2847554ab7c6ed2984dce3";
        const string authToken = "850bda67f16d24f2b486d0254bd4b0eb";
        // Initialize Twilio client
        TwilioClient.Init(accountSid, authToken);

        List<string> contactList = ["+16473276227", "+15195519012", "+16479360747"];

        //getting quotes from mongodb
        DB db = new();
        List<Quote> quotes = db.GetAllQuotes();

        // Specify the message details
        Console.WriteLine(quotes.Count);
        if (quotes.Count > 0)
        {

            var text = $"Quote of the day:\n{quotes[0].detail}\n-{quotes[0].author}";
            Console.WriteLine(text);
            foreach (string contact in contactList)
            {
                var message = MessageResource.Create(
                body: text,
                from: new PhoneNumber("+14434291713"), // Your Twilio number
                to: new PhoneNumber(contact) // Recipient's number
                );
            }

        }

        // Print the message SID to confirm success
        Console.WriteLine($"Messages Sent!");
    }
}
