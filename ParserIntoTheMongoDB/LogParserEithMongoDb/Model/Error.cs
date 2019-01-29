using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace LogParserEithMongoDb.Model
{
    class Error
    {
        public ObjectId Id { get; set; }
        public BsonDocument ErrorObject { get; set; }
        public ObjectId LogId { get; set; }
    }
}
