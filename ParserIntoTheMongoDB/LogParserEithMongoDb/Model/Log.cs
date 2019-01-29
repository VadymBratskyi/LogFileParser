using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace LogParserEithMongoDb.Model
{
    class Log
    {
        public ObjectId Id { get; set; }
        public DateTime RequestDate { get; set; }
        public BsonDocument Request { get; set; }
        public BsonDocument Response { get; set; }
        public DateTime ResponseDate { get; set; }
        public ObjectId LogFileId { get; set; }
        public string MessageId { get; set; }
    }
}
