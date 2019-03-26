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
        public StatusDialog()
        {
            InitializeComponent();
            //treeView1.CheckBoxes = true;
            //treeView1.DrawMode = TreeViewDrawMode.OwnerDrawAll;
        }

        private async void StatusDialog_Load(object sender, EventArgs e)
        {
            var filter1 = Builders<BsonDocument>.Filter.Empty;
            var statusError = await DataProcessor.GetDataFind(filter1, "StatusError", 0, Int32.MaxValue);
            BuildTreeForQueryAsynk(statusError, treeView1, true);
        }

        public async void BuildTreeForQueryAsynk(List<BsonDocument> documents, TreeView tree, bool withValue)
        {
            var docs = documents.Select(d => BsonSerializer.Deserialize<StatusError>(d)).ToList();

            var firstObj = docs.FirstOrDefault();

            if (firstObj != null)
            {

                var jObject = JObject.Parse(firstObj.ToJson().Replace("ObjectId(", "").Replace("ISODate(", "").Replace(")", ""));

                BuildTree treee = new BuildTree(jObject);
                tree.Nodes.Clear();

                foreach (var doc in docs)
                {
                    TreeNode parent = await Task.Run(() =>
                    {
                        JObject obj = JObject.Parse(doc.ToJson().Replace("ObjectId(", "").Replace("ISODate(", "").Replace(")", ""));
                        var prnt = treee.Json2Tree(obj, withValue);
                        prnt.Text = doc.StatusTitle;
                        return prnt;
                    });
                    tree.Nodes.Add(parent);

                }
            }
        }
    }
}
