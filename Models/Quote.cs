using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HypeMan
{
    public class Quote
    {
        public ObjectId _id { get; set; }

        public string detail { get; set; }
        public string author { get; set; }
        public bool is_used { get; set; }


    }
}