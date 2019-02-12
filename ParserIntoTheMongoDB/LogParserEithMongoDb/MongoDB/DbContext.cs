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

        public async Task SaveStatusErrors(List<StatusError> statusErrors)
        {
            var collection = GetStatusErrors();
            await collection.InsertManyAsync(statusErrors);
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
