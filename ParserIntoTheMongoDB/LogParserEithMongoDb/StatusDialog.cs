using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogParserWithMongoDb.Model;
using LogParserWithMongoDb.MongoDB;
using LogParserWithMongoDb.Process;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace LogParserWithMongoDb
{
    public partial class StatusDialog : Form
    {
        private ShowLog showLog;

        private StatusError selectedStatusError;

        public StatusDialog(ShowLog log)
        {
            InitializeComponent();
            showLog = log;
        }

        private void StatusDialog_Load(object sender, EventArgs e)
        {            
            LoadDataToList();
        }

        public async void LoadDataToList()
        {
            var filter1 = Builders<BsonDocument>.Filter.Empty;
            var statusError = await DataProcessor.GetDataFind(filter1, "StatusError", 0, Int32.MaxValue);
            var docs = statusError.Select(d => BsonSerializer.Deserialize<StatusError>(d)).ToList();

            listBox1.DataSource = docs;
            listBox1.ValueMember = "StatusTitle";
        }        

        private void Button1_Click(object sender, EventArgs e)
        {
            showLog.SetStatusError(selectedStatusError);
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void Button3_Click(object sender, EventArgs e)
        {
            var newStatusTitle = textBox1.Text;
            var newStatusCode = textBox2.Text;
            var newStatusParentId = textBox3.Text;

            if (!string.IsNullOrEmpty(newStatusTitle) && !string.IsNullOrEmpty(newStatusCode)) {

                var newStatusError = new StatusError()
                {
                    StatusCode = Convert.ToInt32(newStatusCode),
                    StatusTitle = newStatusTitle,
                };

                if (!string.IsNullOrEmpty(newStatusParentId))
                {
                    newStatusError.SubStatusId = ObjectId.Parse(newStatusParentId);
                }

                await DataProcessor.SaveStatusErrorIntoDb(newStatusError);
                LoadDataToList();
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
            }

         
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null) {
                var selectedItem = (StatusError)listBox1.SelectedItem;
                textBox3.Text = selectedItem.Id.ToString();
                label2.Text = selectedItem.StatusTitle;
                label6.Text = selectedItem.StatusCode.ToString();
                selectedStatusError = selectedItem;
            }            
        }
    }
}
