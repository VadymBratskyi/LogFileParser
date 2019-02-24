using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace LogParserWithMongoDb.Model
{
    public class KnownError
    {
        public ObjectId Id { get; set; }
        public string Message { get; set; }
        public BsonDocument Error { get; set; }
        public ObjectId StatusId { get; set; }
        public BsonDocument Answer { get; set; }
    }
}
