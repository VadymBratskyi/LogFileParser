using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogParserWithMongoDb.Model;
using LogParserWithMongoDb.Process;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace LogParserWithMongoDb.MongoDB
{
    class DataProcessor : Parser
    {
        private static DbContext db = new DbContext();

        public DataProcessor(List<string> listPath, LogParser logParser) :
            base(listPath, logParser)
        {
        }

        /// <summary>
        /// создание LogFiles object
        /// </summary>
        public static LogFile CreateLogFile(string path)
        {
            var lf = new LogFile()
            {
                FileName = path
            };
            return lf;
        }

        /// <summary>
        /// создание Error object
        /// </summary>
        public static void CreateError(string output)
        {
            
            //dynamic jsOutput = JObject.Parse(output);

            //var stastus = jsOutput.status;
            //if (stastus != null && stastus == "OK")
            //{
            //    var error = new Error();
            //    error.ResponsError = BsonDocument.Parse(jsOutput.ToString());
            //    ErrorsList.Add(error);
            //}
            //else
            //{
            //    var error = new Error();
            //    error. ResponsError = BsonDocument.Parse(jsOutput.ToString());
            //    ErrorsList.Add(error);
            //}

            var error = new Error();
            error.ResponsError = BsonDocument.Parse(output);
            ErrorsList.Add(error);

            //var resultArr = jsOutput.RESULT;
            //if (resultArr != null)
            //{
            //    var result = JArray.Parse(jsOutput.RESULT.ToString());
            //    foreach (var res in result)
            //    {
            //        var jsError = res.error;
            //        if (jsError != null)
            //        {
            //            var error = new Error();
            //            error.Message = res.error;
            //            ErrorsList.Add(error);
            //        }

            //    } 
            //}


        }

        /// <summary>
        /// проверяем в темповой колекции наявность этого обьекта,
        /// если нету, то создаем новый и помещаем в темповую колекцию,
        /// если есть, то проверяем на навяность заполненых свойств и дописуем которых нет
        /// и в конце передаем на проверку полного обьекта
        /// </summary>
        public static void CreateLog(string messageId, DateTime requestDate, string request, DateTime responseDate,
            string response, LogFile logFile)
        {
            var isEmpty = @"{""isEmpty"":true}";
            var log = TempLogsList.FirstOrDefault(o => o.MessageId == messageId);
            if (log == null)
            {
                var newlog = new Log()
                {
                    MessageId = messageId,
                    RequestDate = requestDate,
                    Request = request != "" ? BsonDocument.Parse(request) : BsonDocument.Parse(isEmpty),
                    Response = response != "" ? BsonDocument.Parse(response) : BsonDocument.Parse(isEmpty),
                    ResponseDate = responseDate,
                    LogFileId = logFile.Id
                };
                TempLogsList.Add(newlog);
            }
            else if (request != "")
            {
                log.RequestDate = requestDate;
                log.Request = request != "" ? BsonDocument.Parse(request) : BsonDocument.Parse(isEmpty);
            }
            else if (response != "")
            {
                log.ResponseDate = responseDate;
                log.Response = response != "" ? BsonDocument.Parse(response) : BsonDocument.Parse(isEmpty);
            }

            if (log != null)
            {
                RemovefromTempList(log);
            }

        }

        /// <summary>
        /// проверка на полноту обьекта, если полный, 
        /// то переносим в главнуюю колекцию, которая будет идти в БД,
        /// а с темповой удаляем
        /// </summary>
        private static void RemovefromTempList(Log log)
        {
            if (log.MessageId != "" && log.Request != "" &&
                log.Response != "")
            {
                LogsList.Add(log);
                TempLogsList.Remove(log);
            }
        }

        /// <summary>
        /// сохраннение Log обьектов
        /// </summary>
        public static async Task SaveDocumentsIntoDb(List<Log> logs)
        {
            await db.SaveDocuments(logs);
        }

        /// <summary>
        /// сохраннение LogFiles обьектов
        /// </summary>
        public static async Task SaveLogFileIntoDb(LogFile logF)
        {
            await db.SaveLogFile(logF);
        }

        /// <summary>
        /// сохраннение Errors обьектов
        /// </summary>
        public static async Task SaveErrorsIntoDb(List<Error> errors)
        {
            await db.SaveErrors(errors);
        }

        /// <summary>
        /// сохраннение Errors обьектов
        /// </summary>
        public static async Task SaveStatusErrorsIntoDb(List<StatusError> statusErrors)
        {
            await db.SaveStatusErrors(statusErrors);
        }

        public static List<string> GetCollections()
        {
            return db.GetCollections();
        }

        public static MongoDatabase GetDatabase()
        {
            return db.GetDatabase();
        }

        public static Task<List<BsonDocument>> GetDataFind(FilterDefinition<BsonDocument> bsonQuery, string collection, int? scip, int limit)
        {
            return db.GetDataFind(bsonQuery, collection, scip, limit);
        }
    }
}
