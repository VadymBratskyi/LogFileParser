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
        /// создание Error object
        /// </summary>
        public static void CreateError(string output)
        {
            dynamic jsOutput = JObject.Parse(output);

            var stastus = jsOutput.status;
            if (stastus != null && stastus == "OK")
            {
                var result = JArray.Parse(jsOutput.RESULT.ToString());
                foreach (var res in result)
                {
                    var jsError = res.error;
                    if (jsError != null)
                    {
                        var error = new Error();
                        error.Message = res.error;
                        error.ResponsError = BsonDocument.Parse(jsOutput.ToString());
                        CreateUnKnownError(error);
                        ErrorsList.Add(error);
                    }

                }
            }
            else
            {
                var jsError = jsOutput.error;
                if (jsError!= null)
                {
                    var error = new Error();
                    error.Message = jsOutput.error.message;
                    error.Details = jsOutput.error.data;
                    error.ResponsError = BsonDocument.Parse(jsOutput.ToString());
                    CreateUnKnownError(error);
                    ErrorsList.Add(error);
                }
               
            }

        }

        /// <summary>
        /// создаем не извесные ошибки
        /// </summary>
        private static void CreateUnKnownError(Error error)
        {
            var index = error.Message.IndexOf(".'");
            var sms = index >= 0 ? error.Message.Substring(0, index) : error.Message;

            IEnumerable<string> arrError = sms.Trim().Split(' ');

            UnKnownError findUnKnownError = null;
            KnownError findKnownError = null;

            string message = "";

            foreach (var err in arrError)
            {
                int res;
                var dd = Int32.TryParse(err, out res);
                if (err.IndexOf("'") < 0 && res == 0)
                {
                    message += err+" ";
                }
            }

            findUnKnownError = unKnownErrorsList.Find(o => o.ErrorText == message.TrimEnd());
            findKnownError = knownErrorsList.Find(o => o.Message == message.TrimEnd());

            if (findUnKnownError == null && findKnownError == null)
            {
                var unError = new UnKnownError()
                {
                    ErrorText = message.TrimEnd(),
                    Error = error.ResponsError,
                    CountFounded = 1
                };
                
                unKnownErrorsList.Add(unError);
            } else if (findUnKnownError != null)
            {
                findUnKnownError.CountFounded++;
                if (findUnKnownError.Id != ObjectId.Empty)
                {
                    findUnKnownError.IsModified = true;
                }
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
        /// сохраннение StatusError обьектов
        /// </summary>
        public static async Task SaveStatusErrorsIntoDb(List<StatusError> statusErrors)
        {
            await db.SaveStatusErrors(statusErrors);
        }

        /// <summary>
        /// сохраннение UnKnownError обьектов
        /// </summary>
        public static async Task SaveUnKnownErrorsIntoDb(List<UnKnownError> unKnownError)
        {
            await db.SaveUnKnownErrors(unKnownError);
        }

        /// <summary>
        /// сохраннение UnKnownError обьектов
        /// </summary>
        public static async Task UpdateUnKnownErrorsIntoDb(List<UnKnownError> unKnownError)
        {
            await db.UpdateUnKnownErrors(unKnownError);
        }

        /// <summary>
        /// сохраннение KnownError обьектов
        /// </summary>
        public static async Task SaveKnownErrorsIntoDb(KnownError knownError)
        {
            await db.SaveKnownErrors(knownError);
        }

        /// <summary>
        /// сохраннение Answer обьектов
        /// </summary>
        public static async Task SaveAnswerIntoDb(Answer answer)
        {
            await db.SaveAnswers(answer);
        }
        
        /// <summary>
        /// сохраннение StatusError обьекта
        /// </summary>
        public static async Task SaveStatusErrorIntoDb(StatusError statusError)
        {
            await db.SaveStatusError(statusError);
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

        public static Task DeleteDocument(FilterDefinition<BsonDocument> bsonQuery, string collection)
        {
            return db.DeleteObject(bsonQuery, collection);
        }
    }
}
