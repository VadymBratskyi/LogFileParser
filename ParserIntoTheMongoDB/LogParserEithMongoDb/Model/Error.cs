using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace LogParserWithMongoDb.Model
{
    class Error
    {
        public ObjectId Id { get; set; }
        public BsonDocument ResponsError { get; set; }
    }
}
