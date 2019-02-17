using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace LogParserWithMongoDb.Model
{
    public class Error
    {
        public ObjectId Id { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public BsonDocument ResponsError { get; set; }
    }
}
