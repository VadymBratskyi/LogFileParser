using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogParserEithMongoDb.Model;
using LogParserEithMongoDb.MongoDB;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using MongoDB.Bson.Serialization;
using Timer = System.Windows.Forms.Timer;


namespace LogParserEithMongoDb.Process
{
    class Parser
    {
        private const string RegBinData = @"""binary_data"":""[^""]+""";     
        private const string RegInput = @"(^(Input)( = .*))";
        private const string RegOutput = @"(^(Output)( = .*))";
        private const string RegStart= @"(=>.*)";
        private const string RegEnd= @"(<=.*)";
        private const string RegError= @"error";
        private const string RegDate = @"([0-3]?[0-9]/[0-3]?[0-9]/(?:[0-9]{2})?[0-9]{2})\s(1[0-2]|[0-9]):(00|0[1-9]{1}|[1-5]{1}[0-9]):(00|0[1-9]{1}|[1-5]{1}[0-9])\s(PM|AM)";
        protected static List<Log> LogsList = new List<Log>();
        protected static List<Log> TempLogsList = new List<Log>();
        protected static List<Error> ErrorsList = new List<Error>();
        private readonly LogParser logParser;


        private readonly SynchronizationContext _synchronizationContext;
        
        public Parser(List<string> listPath, LogParser logParser)
        {
            this.logParser = logParser;
            OpenFiles(listPath);
            _synchronizationContext = SynchronizationContext.Current;
        }
        
        private async void OpenFiles(List<string> listpath)
        {
            if (MessageBox.Show("There are " + listpath.Count() + " elements. \r\nDo you want to parse these files?",
                    "Information!", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                logParser.progressBarToFinish.Value = 0;
                logParser.pictureBox1.Visible = true;
                logParser.labelTime.Visible = false;
                logParser.label4WithoutPair.Text = @"0";
                logParser.label5CountInserted.Text = @"0";
                var countRows = 0;
                var countFiles = listpath.Count;
                var file = 0;
              
                var dt = DateTime.Now.TimeOfDay;

                foreach (var path in listpath)
                {
                    LogsList.Clear();
                    logParser.labelFileName.Text = path;
                    logParser.labelCountFiles.Text = countFiles.ToString();
                    var logFile = DataProcessor.CreateLogFile(path);

                    await DataProcessor.SaveLogFileIntoDb(logFile);
                    var resultParsDoc = await ParsDocument(path, logFile);
                    await DataProcessor.SaveDocumentsIntoDb(LogsList);

                    logParser.textBox1.AppendText(resultParsDoc);
                    countRows += LogsList.Count;
                    countFiles--;
                    file++;

                    var dt2 = DateTime.Now.TimeOfDay;
                    if (countFiles > 0)
                    {
                        logParser.labelTime.Visible = true;
                        logParser.pictureBox1.Visible = false;
                        UpdateTimer(GetTime(dt, dt2, file, countFiles));
                    }
                    UpdateProgBarToFinish((int)((file * 100.0) / listpath.Count));
                }               

                logParser.labelCountFiles.Text = countFiles.ToString();
                logParser.labelFileName.Text = "";
                logParser.progressBarFile.Value = 0;
               
                if (TempLogsList.Any())
                {
                    await DataProcessor.SaveDocumentsIntoDb(TempLogsList);
                    logParser.label4WithoutPair.Text = TempLogsList.Count.ToString();
                }

                logParser.pictureBox1.Visible = false;
                logParser.textBox1.AppendText("\r\n\r\nReading finished successfully! ");
                logParser.label5CountInserted.Text = (countRows + TempLogsList.Count).ToString();
                TempLogsList.Clear();
            }
           
        }
        
        private async Task<string> ParsDocument(string filePath, LogFile logFile)
        {
            try
            {
                await Task.Run(() =>
                {
                    DateTime dateStart = DateTime.MinValue;
                    DateTime dateEnd = DateTime.MinValue;
                    var countFileLine = File.ReadAllLines(filePath).Length;
                    using (var read = new StreamReader(filePath, Encoding.UTF8))
                    {
                        string value;
                        int index = 0;
                        while (!string.IsNullOrEmpty(value = read.ReadLine()))
                        {
                            /*-------------------getDate from log file-----------------*/
                            var startDate = Regex.Match(value, RegStart, RegexOptions.IgnoreCase);
                            if (startDate.Success)
                            {
                                var time = Regex.Match(value, RegDate, RegexOptions.IgnoreCase);
                                if (time.Success)
                                {
                                    dateStart = ConverterToDateTime(time.Value);
                                }
                            }

                            var endDate = Regex.Match(value, RegEnd, RegexOptions.IgnoreCase);
                            if (endDate.Success)
                            {
                                var time = Regex.Match(value, RegDate, RegexOptions.IgnoreCase);
                                if (time.Success)
                                {
                                    dateEnd = ConverterToDateTime(time.Value);
                                }
                            }
                            /*--------------------------------------------------------------*/


                            /*-----------------getRequestAndResponse------------------------*/
                            var input = Regex.Match(value, RegInput, RegexOptions.IgnoreCase);
                            if (input.Success)
                            {
                                var binarydata = Regex.Match(input.Value, RegBinData, RegexOptions.IgnoreCase);
                                var rezult = binarydata.Success ? input.Value.Replace(binarydata.Value, "\"binary_data\":\"true\"") : input.Value;
                                var rez = rezult.Remove(0, 8);
                                var id = rez != "" ? GetmessageId(rez) : "notFoundInput_" + index;
                                var rezId = id != "" ? id : "idIsNull_" + index;
                                DataProcessor.CreateLog(rezId, dateStart, rez, DateTime.MinValue, "", logFile);
                            }
                            var output = Regex.Match(value, RegOutput, RegexOptions.IgnoreCase);
                            if (output.Success)
                            {
                                var rez = output.Value.Remove(0, 8);
                                var error = GetError(rez);
                                var id = rez != "" ? GetmessageId(rez) : "notFoundOutput_" + index;
                                var rezId = id != "" ? id : "idIsNull_" + index;
                                DataProcessor.CreateLog(rezId, DateTime.MinValue, "", dateEnd, rez, logFile);
                            }
                            /*--------------------------------------------------------------*/
                            index++;
                            UpdateProgBarFile((int)((index * 100.0) / countFileLine));
                         
                        }
                        
                    }
                });
                
                return "\r\nDocument " + filePath + ". completed successful! \r\nCount objetcs: " + LogsList.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return "Error!!! "+ filePath;
        }


        private void UpdateProgBarFile(int value)
        {
            _synchronizationContext.Post((o =>
            {
                logParser.progressBarFile.Value = value;
            }), value);
        }

        private void UpdateProgBarToFinish(int value)
        {
            _synchronizationContext.Post((o =>
            {
                logParser.progressBarToFinish.Value = value;
            }), value);
            if (value==100)
            {
                UpdateTimer(TimeSpan.Zero);
            }
        }

        public TimeSpan GetTime(TimeSpan first, TimeSpan second, int files, int counFile)
        {
             return TimeSpan.FromSeconds(((second.TotalSeconds - first.TotalSeconds) / files) * counFile);
        }


        private void UpdateTimer(TimeSpan timefrom)
        {
            _synchronizationContext.Post((o =>
            {
                logParser.Expiry = DateTime.Now.Add(timefrom);
                Timer tm = new Timer();
                tm.Interval = 100;
                tm.Tick += logParser.timer1_Tick;
                logParser.timer1.Enabled = true;
                logParser.timer1.Start();
            }), null);
        }

        /// <summary>
        /// получаем messageId с помощью JObject.Parse()
        /// </summary>
        private static dynamic GetmessageId(string json)
        {
                dynamic msId = JObject.Parse(json);
                return msId.message_id.ToString();
        }

        /// <summary>
        /// получаем error с помощью JObject.Parse()
        /// </summary>
        private static dynamic GetError(string json)
        {
            var parsError = Regex.Match(json, RegError, RegexOptions.IgnoreCase);
            if (parsError.Success)
            {
                dynamic error = JObject.Parse(json);
            }

            return null;
        }

        /// <summary>
        /// конвертирование даты с string в DateTime
        /// </summary>
        private static DateTime ConverterToDateTime(string date)
        {
            if (string.IsNullOrWhiteSpace(date))
            {
                return default(DateTime);
            }

            DateTime outputDateTime;
            if (!DateTime.TryParse(date, CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.None, out outputDateTime))
            {
                outputDateTime = DateTime.MinValue;
            }
            return outputDateTime;
        }
        
    }
}
