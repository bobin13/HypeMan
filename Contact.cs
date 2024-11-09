using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace HypeMan
{
    public class Contact
    {
        public ObjectId _id { get; set; }
        public string name { get; set; }
        public string phoneNumber { get; set; }
    }
}