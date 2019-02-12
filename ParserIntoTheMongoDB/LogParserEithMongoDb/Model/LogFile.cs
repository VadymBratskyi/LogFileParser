using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace LogParserWithMongoDb.Model
{
    class LogFile
    {
        public ObjectId Id { get; set; }
        public string FileName { get; set; }
        public string FileStatus { get; set; }
        public int CountOpen { get; set; }
    }
}
