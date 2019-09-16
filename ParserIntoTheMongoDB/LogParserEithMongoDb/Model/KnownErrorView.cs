using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogParserWithMongoDb.Model
{
    public class KnownErrorView
    {        
        public int Count { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string StatusTitle { get; set; }
        public string Answer { get; set; }        
    }
}
