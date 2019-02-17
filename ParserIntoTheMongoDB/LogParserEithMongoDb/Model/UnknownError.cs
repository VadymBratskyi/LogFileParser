﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace LogParserWithMongoDb.Model
{
    public class UnKnownError
    {
        public ObjectId Id { get; set; }
        public string ErrorText { get; set; }
        public BsonDocument Error { get; set; }
    }
}