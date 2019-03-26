using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LogParserWithMongoDb.Model
{
    public class UnKnownError
    {
        public ObjectId Id { get; set; }
        public int CountFounded { get; set; }
        public string ErrorText { get; set; }
        public BsonDocument Error { get; set; }
        [BsonIgnore]
        public bool IsModified { get; set; }
    }
}
