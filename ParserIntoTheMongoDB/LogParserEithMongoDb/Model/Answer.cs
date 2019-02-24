using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogParserWithMongoDb.Model
{
    public class Answer
    {
        public ObjectId Id { get; set; }
        public string Text { get; set; }
    }
}
