using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace LogParserWithMongoDb.Model
{
   public class StatusError
    {
        public ObjectId Id { get; set; }
        public int StatusCode { get; set; }
        public string StatusTitle { get; set; }
        public ObjectId SubStatusId { get; set; }
    }
}
