using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogParserEithMongoDb.MongoDB;
using LogParserEithMongoDb.Process;
using MongoDB.Bson;
using MongoDB.Driver;
using Timer = System.Windows.Forms.Timer;


namespace LogParserEithMongoDb
{
    public partial class LogParser : Form
    {
        public LogParser()
        {
            InitializeComponent();
        }

        private void openFolderWithJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fold = new FolderBrowserDialog();
            if (fold.ShowDialog() == DialogResult.OK)
            {
                var files = Directory.GetFiles(fold.SelectedPath).Where(o => o.Contains("Server")).ToList();
                var pars = new Parser(files, this);
            }
        }

        private void openLOGSFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var listDoc = new List<string>();
            var files = new OpenFileDialog();
            files.Multiselect = true;
            files.Filter = @"LOGS Files (.log)|*.log";
            if (files.ShowDialog() == DialogResult.OK)
            {
                textBox2PathFolder.Text = Path.GetDirectoryName(files.FileName);
                foreach (String file in files.FileNames.Where(d => d.Contains("Server")))
                {
                    var filter = Builders<BsonDocument>.Filter.Eq("FileName", file);
                    var dt = DataProcessor.GetDataFind(filter, "LogsFiles", 0, Int32.MaxValue).Result;
                    if (!dt.Any())
                    {
                        listDoc.Add(file);
                    }
                }
                
                var pars = new Parser(listDoc, this);
            }
        }

        private void showLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void sgowLogsFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        public DateTime Expiry;
        public static TimeSpan Delay { get; }

        public void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan remaining = Expiry - DateTime.Now;
            if (remaining.Hours == 0 && remaining.Minutes == 0 && remaining.Seconds == 0)
            {
                timer1.Stop();
            }
            labelTime.Text = String.Format("{0:00}:{1:00}:{2:00}",
                    remaining.Hours, remaining.Minutes, remaining.Seconds);
        }

        private async void gridFsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var listDoc = new List<string>();
            var files = new OpenFileDialog();
            files.Multiselect = true;
            files.Filter = @"LOGS Files (.log)|*.log";
            if (files.ShowDialog() == DialogResult.OK)
            {
                textBox2PathFolder.Text = Path.GetDirectoryName(files.FileName);
                foreach (String file in files.FileNames.Where(d => d.Contains("Server")))
                {
                    listDoc.Add(file);
                }

                DbContext db = new DbContext();
                Stopwatch st = new Stopwatch();
                st.Start();

                foreach (var list in listDoc)
                {
                    await db.GridFS(list);
                }

                st.Stop();
                TimeSpan ts = st.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);

                MessageBox.Show(elapsedTime);

            }
        }

        private void showDatasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowLog logForm = new ShowLog();
            logForm.Show();
        }
    
    }
}
