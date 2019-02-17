using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogParserWithMongoDb.Model;
using LogParserWithMongoDb.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace LogParserWithMongoDb.Process
{
    public class InitDbLogHelper
    {

        public static async void InitDb()
        {
            var filter = Builders<BsonDocument>.Filter.Empty;
            var dt = DataProcessor.GetDataFind(filter, "StatusError", 0, 5).Result;
            if (!dt.Any())
            {
                var statuses = BuildStatusErrors();
                await DataProcessor.SaveStatusErrorsIntoDb(statuses);
            }
        }

        private static List<StatusError> BuildStatusErrors()
        {
            return new List<StatusError>()
            {
                new StatusError(){ StatusCode = 400, StatusTitle = "Bad Request"},
                new StatusError(){ StatusCode = 500, StatusTitle = "Server Error"}
            };
        }


        public static IEnumerable<UnKnownError> GetUnKnownErrors()
        {
            var filter = Builders<BsonDocument>.Filter.Empty;
            var dt = DataProcessor.GetDataFind(filter, "UnKnownError", 0, Int32.MaxValue).Result;
            return dt.Select(d => BsonSerializer.Deserialize<UnKnownError>(d));
        }


    }
}
