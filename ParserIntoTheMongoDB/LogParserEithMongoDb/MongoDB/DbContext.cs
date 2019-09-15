using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LogParserWithMongoDb.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace LogParserWithMongoDb.MongoDB
{
    class DbContext
    {
        private List<Log> _logs; 
        private string connectionString = ConfigurationManager.ConnectionStrings["DocumentsDB"].ConnectionString;

        [ThreadStatic]
        private static MongoClient client;

        [ThreadStatic]
        private static IMongoDatabase db;

        [ThreadStatic]
        private static IMongoCollection<Log> _mongoLogs;
         
        [ThreadStatic]
        private static IMongoCollection<LogFile> _mongoLogFiles;

        [ThreadStatic]
        private static IMongoCollection<Error> _mongoErrors;

        [ThreadStatic]
        private static IMongoCollection<StatusError> _mongoStatusErrors;

        [ThreadStatic]
        private static IMongoCollection<UnKnownError> _mongoUnKnownErrors;

        [ThreadStatic]
        private static IMongoCollection<KnownError> _mongoKnownErrors;

        [ThreadStatic]
        private static IMongoCollection<Answer> _mongoAnswers;

        public async Task SaveLogFile(LogFile logFile)
        {
            var collection = GetLogFiles();
            await  collection.InsertOneAsync(logFile);
        }

        public async Task SaveDocuments(List<Log> logs)
        {
            var collection = GetLogs();
            await collection.InsertManyAsync(logs);
        }

        public async Task SaveErrors(List<Error> errors)
        {
            var collection = GetErrors();
            await collection.InsertManyAsync(errors);
        }

        public async Task UpdateUnKnownErrors(List<UnKnownError> unKnownErrors)
        {
            var collection = GetUnKnownErrors();
            foreach (var error in unKnownErrors)
            {
                var filter = Builders<UnKnownError>.Filter.Eq(o => o.Id, error.Id);
                await collection.ReplaceOneAsync(filter, error);
            }
            
        }

        public async Task SaveStatusErrors(List<StatusError> statusErrors)
        {
            var collection = GetStatusErrors();
            await collection.InsertManyAsync(statusErrors);
        }

        public async Task SaveUnKnownErrors(List<UnKnownError> unKnownError)
        {
            var collection = GetUnKnownErrors();
            await collection.InsertManyAsync(unKnownError);
        }

        public async Task SaveKnownErrors(KnownError knownError)
        {
            var collection = GetKnownErrors();
            await collection.InsertOneAsync(knownError);
        }

        public async Task SaveAnswers(Answer answers)
        {
            var collection = GetAnswers();
            await collection.InsertOneAsync(answers);
        }

        public async Task SaveStatusError(StatusError statusError)
        {
            var collection = GetStatusErrors();
            await collection.InsertOneAsync(statusError);
        }

        private MongoClient GetClient()
        {
            if (client == null)
            {
                client = new MongoClient(connectionString);
            }
            return client;
        }

        public IMongoDatabase GetDB()
        {
            if (db == null)
            {
                db = GetClient().GetDatabase("LogsParser");
            }
            return db;
        }


        private IMongoCollection<Log> GetLogs()
        {
            if (_mongoLogs == null)
            {
                _mongoLogs = GetDB().GetCollection<Log>("Logs");
            }
            return _mongoLogs;
        }

        private IMongoCollection<LogFile> GetLogFiles()
        {
            if (_mongoLogFiles == null)
            {
                _mongoLogFiles = GetDB().GetCollection<LogFile>("LogsFiles");
            }
            return _mongoLogFiles;
        }

        private IMongoCollection<Error> GetErrors()
        {
            if (_mongoErrors == null)
            {
                _mongoErrors = GetDB().GetCollection<Error>("Errors");
            }
            return _mongoErrors;
        }

        private IMongoCollection<StatusError> GetStatusErrors()
        {
            if (_mongoStatusErrors == null)
            {
                _mongoStatusErrors = GetDB().GetCollection<StatusError>("StatusError");
            }
            return _mongoStatusErrors;
        }

        private IMongoCollection<UnKnownError> GetUnKnownErrors()
        {
            if (_mongoUnKnownErrors == null)
            {
                _mongoUnKnownErrors = GetDB().GetCollection<UnKnownError>("UnKnownError");
            }
            return _mongoUnKnownErrors;
        }

        private IMongoCollection<KnownError> GetKnownErrors()
        {
            if (_mongoKnownErrors == null)
            {
                _mongoKnownErrors = GetDB().GetCollection<KnownError>("KnownError");
            }
            return _mongoKnownErrors;
        }

        private IMongoCollection<Answer> GetAnswers()
        {
            if (_mongoAnswers == null)
            {
                _mongoAnswers = GetDB().GetCollection<Answer>("Answers");
            }
            return _mongoAnswers;
        }

        public List<string> GetCollections()
        {
            var server = GetClient().GetServer();
            var database = server.GetDatabase("LogsParser");
            return database.GetCollectionNames().ToList();
        }

        public MongoDatabase GetDatabase()
        {
            var server = GetClient().GetServer();
            var database = server.GetDatabase("LogsParser");
            return database;
        }


        public Task DeleteObject(FilterDefinition<BsonDocument> bsnQuery, string collection)
        {
            var logs = GetDB().GetCollection<BsonDocument>(collection);
            return logs.DeleteOneAsync(bsnQuery);
        }

        public Task<List<BsonDocument>> GetDataFind(FilterDefinition<BsonDocument> bsnQuery, string collection, int? skip, int limit)
        {
            var logs = GetDB().GetCollection<BsonDocument>(collection);
            return logs.Find(bsnQuery).Skip(skip).Limit(limit).ToListAsync();
        }

        public async Task GridFS(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            var db = GetDB();

            IGridFSBucket gridFS = new GridFSBucket(db);
            using (Stream fs = new FileStream(filePath, FileMode.Open))
            {
                ObjectId id = await gridFS.UploadFromStreamAsync(fileName, fs);
            }
        }
    }
}
