using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
    public partial class ShowLog : Form
    {
        private List<QueryModel> queryList = new List<QueryModel>();
        private List<string> _listCollections;
        private List<Answer> _listAnswers;
        HashSet<TreeNode> _listNodes;
        private readonly string _dbName;
        private readonly SynchronizationContext _synchronizationContext;
        public int DefaultCountTake = 50;
        public int Skip = 0;

        private UnKnownError SelectedUnknownError;

        public ShowLog()
        {  
            _listCollections = DataProcessor.GetCollections();
            _dbName = DataProcessor.GetDatabase().Name;
            InitializeComponent();
        }

        private void LoadDataControls()
        {
            label4.Text = _dbName;
            comboBox1.DataSource = _listCollections;
            textBox5.Text = DefaultCountTake.ToString();
            textBox4.Text = Skip.ToString();
            textBox1.Text = "db.getCollection(" + comboBox1.Text + ").find({ })";
            dateTimePicker1.Value = DateTime.Today;
            treeView1.ContextMenuStrip = mnuTextFile;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
            
            GetValueToQuery();
            GetData();
            
            if (queryList.Count == 1)
            {
                treeView1.TopNode.Expand();
            }
        }
        

        private void GetData()
        {
            treeView1.Nodes.Clear();
            
            if (queryList.Any())
            {
                foreach (QueryModel queryModel in queryList)
                {
                    TreeNode queryTree = new TreeNode("Query result: ");
                    BuildTreeForResultQueryAsynk(queryModel, queryTree, true);
                    treeView1.Nodes.Add(queryTree);
                }
            }
        }

        private void GetValueToQuery()
        {
            var start = @"\(.+\)\..+?\(\{(|.+)\}\)";
            var pattern1 = @"\(.+\)\.";
            var pattern2 = @"\{.+:";
            var pattern3 = @"\:.+\}";

            queryList.Clear();
            var matchests = Regex.Matches(textBox1.Text, start);
            foreach (Match match in matchests)
            {
                var db = Regex.Match(match.Value, pattern1);
                dynamic dbName = GetValueOfType(db.Value);
                var tempKey = Regex.Match(match.Value, pattern2);
                dynamic key = GetValueOfType(tempKey.Value);
                var tempVal = Regex.Match(match.Value, pattern3);
                dynamic value = GetValueOfType(tempVal.Value);
                queryList.Add(new QueryModel() { CollectionName = dbName, Key = key, Value = value });
            }
        }

        private object GetValueOfType(string match)
        {
            try
            {
                if (match.Contains("(") || match.Contains(")"))
                {
                    return match.Replace('(', ' ').Replace(")", " ").Replace(".", " ").Replace("\"", " ").Replace("'", " ").Trim();
                }
                else if (match.Contains("'") || match.Contains("\""))
                {
                    var rez = match.Replace('{', ' ').Replace(":", " ").Replace("\"", " ").Replace("\'", "").Replace('}', ' ')
                        .Trim();

                    ObjectId objId;
                    if (ObjectId.TryParse(rez, out objId))
                    {
                        return objId;
                    }

                    var reg = @"\d{2}(/|\.|-)\d{2}(/|\.|-)\d{4}";
                    var dates = Regex.Matches(rez, reg).Cast<Match>().Select(o=>o.Value).ToArray();
                    if (dates.Any())
                    {
                        DateTime stDate;
                        DateTime enDate;
                        if (dates.Length == 1)
                        {
                            if (DateTime.TryParse(dates[0], out stDate))
                            {
                                return stDate;
                            }
                        }
                        else
                        {
                            if (DateTime.TryParse(dates[0], out stDate) && DateTime.TryParse(dates[1], out enDate))
                            {
                                return new QueryDate() { StartDate = stDate, EndDate = enDate };
                            }
                        }

                    }

                    return rez;
                }
                else if (match.Contains("true") || match.Contains("false"))
                {
                    return Convert.ToBoolean(match.Replace(":", " ").Replace("}", " ").Trim());
                }
                else if(!match.Contains("null") && match!="")
                {
                    return Convert.ToInt64(match.Replace(':', ' ').Replace('}', ' ').Trim());
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return null;
        }


        private async void button2_Click(object sender, EventArgs e)
        {
            LoadDataControls();
            treeView2.CheckBoxes = true;
            treeView2.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            var data = await LoadDataToBuildQuery();
            BuildTreeForQueryAsynk(data, treeView2, false);
        }
        

        private async Task<List<BsonDocument>> LoadDataFromDbToRezult(QueryModel queryMod)
        {
            
            FilterDefinition<BsonDocument> filter = null;

            if (queryMod.Key == null && queryMod.Value == null)
            {
                filter = Builders<BsonDocument>.Filter.Empty;
            }
            else
            {
                if (queryMod.Value.GetType() == typeof(DateTime))
                {
                    DateTime dt = (DateTime)queryMod.Value;
                    var filterBuilder = Builders<BsonDocument>.Filter; 
                    filter = filterBuilder.Gt(queryMod.Key, dt) &
                             filterBuilder.Lt(queryMod.Key, dt.AddDays(1));
                }
                else if (queryMod.Value.GetType() == typeof(QueryDate))
                {
                    QueryDate dt = (QueryDate)queryMod.Value;
                    var filterBuilder = Builders<BsonDocument>.Filter;
                    filter = filterBuilder.Gt(queryMod.Key, dt.StartDate) &
                             filterBuilder.Lt(queryMod.Key, dt.EndDate.AddDays(1));
                }
                else
                {
                    filter = Builders<BsonDocument>.Filter.Eq(queryMod.Key, queryMod.Value);
                }

            }
            return await DataProcessor.GetDataFind(filter, queryMod.CollectionName, Skip, DefaultCountTake);
            
        }

        private async Task<List<BsonDocument>> LoadDataToBuildQuery()
        {
            var filter = Builders<BsonDocument>.Filter.Empty;
            return await DataProcessor.GetDataFind(filter, comboBox1.SelectedItem.ToString(), null, Int32.MaxValue); 
        }
       
        public async void BuildTreeForResultQueryAsynk(QueryModel model, TreeNode tree, bool withValue)
        {
            pictureBox2.Visible = true;
            var docs = await LoadDataFromDbToRezult(model);

            var treee = new BuildTree();
            var i = 1;

            foreach (var doc in docs)
            {
                TreeNode parent = await Task.Run(() =>
                {
                   JObject obj = JObject.Parse(doc.ToJson().Replace("ObjectId(", "").Replace("ISODate(", "").Replace(")", ""));
                   var prnt = treee.Json2Tree(obj, withValue);
                   prnt.Text = (i++) + " - ObjectId(" + doc.FirstOrDefault().Value + ")";
                   return prnt;
                });
                tree.Nodes.Add(parent);
            }
            pictureBox2.Visible = false;
        }

        public async void BuildTreeForQueryAsynk(List<BsonDocument> docs, TreeView tree, bool withValue)
        {
            var firstObj = docs.FirstOrDefault();

            if (firstObj != null)
            {

                var jObject = JObject.Parse(firstObj.ToJson().Replace("ObjectId(", "").Replace("ISODate(", "").Replace(")", ""));

                Process.BuildTree treee = new BuildTree(jObject);
                tree.Nodes.Clear();
                pictureBox1.Visible = true;

                await Task.Run(() =>
                {
                    foreach (var doc in docs.Skip(1))
                    {
                        if (!withValue)
                        {
                            treee.CompareJson(JObject.Parse(doc.ToJson().Replace("ObjectId(", "").Replace("ISODate(", "").Replace(")", "")));
                        }
                    }
                });

                TreeNode parent = treee.Json2Tree(treee.GetJsObject, withValue);
                parent.Text = comboBox1.SelectedItem.ToString();
                tree.Nodes.Add(parent);
                treeView2.Nodes[0].Expand();
                var dt = treeView2.Nodes[0];
              //  GetNodesTree(dt, null);
                pictureBox1.Visible = false;
            }
        }

        private static JObject jobj = new JObject();

        public static void GetNodesTree(TreeNode node, JObject prevObj)
        {
            foreach (TreeNode nd in node.Nodes)
            {
                if (nd.Nodes.Count > 0)
                {
                    var parentObj = new JObject(new JProperty(nd.Text));
                    GetNodesTree(nd, parentObj);
                }
                else if (prevObj != null)
                {
                    prevObj.Add(nd.Text, null);                  
                }
                else
                {
                    if (prevObj != null)
                    {
                        jobj.Add(prevObj);
                    }
                    else
                    {
                        jobj.Add(nd.Text, null);
                    }
                    
                }
            }
        }


        private void SelectNode()
        {
            queryList.Clear();
            _listNodes = new HashSet<TreeNode>();
            bld = new StringBuilder();
            GetChackedNode(treeView2.Nodes[0].Nodes);
         
            if (_listNodes.Any())
            {
                foreach (var node in _listNodes)
                {
                    _countNode++;
                    bld.Append("db.getCollection('" + comboBox1.SelectedItem + "').find({'");
                    BuildQuery(node);
                    bld.Append("':  })\r\n");
                }

                textBox1.Text = "";
                textBox1.Text = bld.ToString();
            }
            else
            {
                textBox1.Text = "db.getCollection(" + comboBox1.Text + ").find({ })";
            }
            
        }

        private void treeView2_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    CheckAllChildNodes(e.Node, e.Node.Checked);
                }
                if (e.Node.Parent != null)
                {
                    CheckAllParentNodes(e.Node);
                }
                SelectNode();
            }
        }

        private void CheckAllParentNodes(TreeNode treeNode)
        {
            if (treeNode.Parent != null)
            {
                var isCh = false;
                foreach (TreeNode prNodes in treeNode.Parent.Nodes)
                {
                    if (prNodes.Checked)
                    {
                        isCh = true;
                    }
                }
                treeNode.Parent.Checked = isCh;
                CheckAllParentNodes(treeNode.Parent);
            }
        }
        
        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        private void treeView2_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node.Parent == null)
            {
                int d = (int)(0.2 * e.Bounds.Height);
                Rectangle rect = new Rectangle(d + treeView2.Margin.Left, d + e.Bounds.Top, e.Bounds.Height - d * 2, e.Bounds.Height - d * 2);
                e.Graphics.FillRectangle(new SolidBrush(Color.FromKnownColor(KnownColor.Control)), rect);
                e.Graphics.DrawRectangle(Pens.Silver, rect);
                StringFormat sf = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };
                e.Graphics.DrawString(e.Node.IsExpanded ? "-" : "+", treeView2.Font, new SolidBrush(Color.Blue), rect, sf);
                //Draw the dotted line connecting the expanding/collapsing button and the node Text
                using (Pen dotted = new Pen(Color.Black) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot })
                {
                    e.Graphics.DrawLine(dotted, new Point(rect.Right + 1, rect.Top + rect.Height / 2), new Point(rect.Right + 4, rect.Top + rect.Height / 2));
                }
                //Draw text
                Rectangle textRect = new Rectangle(e.Bounds.Left + rect.Right + 4, e.Bounds.Top, e.Bounds.Width - rect.Right - 4, e.Bounds.Height);
                e.Graphics.DrawString(e.Node.Text, treeView2.Font, new SolidBrush(Color.Black), textRect);
            }
            else e.DrawDefault = true;
        }

        private StringBuilder bld;
        private int _countNode = 0;
        
        private void GetChackedNode(TreeNodeCollection node)
        { 
            foreach (TreeNode nd in node)
            {
                if (nd.Checked)
                {
                    if (nd.Nodes.Count > 0)
                    {
                        GetChackedNode(nd.Nodes);
                    }
                    else
                    {
                        _listNodes.Add(nd);
                    }
                }
            }
        }


        private void BuildQuery(TreeNode  nod)
        {
            var fullPath = nod.FullPath;
            var query = fullPath.Replace("\\", ".").Replace(".[0]","");
            var rez = query.Substring(query.IndexOf(".") + 1);
            bld.Append(rez);
        }


        private void GetCollapsTree(TreeNode nod)
        {
            if (nod.Parent!=null)
            {
                nod.Parent.Expand();
                GetCollapsTree(nod.Parent);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var tr = treeView1.Nodes[0];
            var tgr = treeView1.Nodes[0].Nodes.Cast<TreeNode>().Where(o => o.Text == "_id");
            var t2gr = treeView1.FlattenTree().Where(o => o.Text == textBox6.Text.Trim());

            foreach (var nod in t2gr)
            {
                GetCollapsTree(nod);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (treeView1.Nodes.Cast<TreeNode>().Any())
            {
                if (Skip == 0)
                {
                    return;
                }
                Skip -= Convert.ToInt32(textBox5.Text);
                textBox4.Text = Skip.ToString();
                GetData();
                treeView1.TopNode.Expand();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (treeView1.Nodes.Cast<TreeNode>().Any())
            {
                Skip += Convert.ToInt32(textBox5.Text);
                textBox4.Text = Skip.ToString();
                GetData();
                treeView1.TopNode.Expand();
            }
        }

        private void copyValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = treeView1.SelectedNode;
            Clipboard.SetText(selectedNode.Text);
        }
        
        private void copyPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selNode = treeView1.SelectedNode;
            if (selNode != null)
            {
                var arrPath = selNode.FullPath.Split(new char[] { '\\' });
                var fullPath = new StringBuilder();
                if (selNode.Nodes.Count != 0)
                {
                    for (int i = 0; i < arrPath.Length; i++)
                    {
                        if (i>1)
                        {
                            fullPath.Append(arrPath[i]+".");
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < arrPath.Length; i++)
                    {
                        if (i > 1 && i<arrPath.Length-1)
                        {
                            fullPath.Append(arrPath[i]+".");
                        }
                    }
                }
                fullPath.Replace(".[0]","").Remove(fullPath.Length - 1, 1);
                Clipboard.SetText(fullPath.ToString());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var dt = dateTimePicker1.Value.Date;

            var insertText = dt.ToShortDateString();
            var selectionIndex = textBox1.SelectionStart;
            textBox1.Text = textBox1.Text.Insert(selectionIndex, "'"+insertText+"'");
            textBox1.SelectionStart = selectionIndex + insertText.Length;
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                if (treeView1.Nodes.Cast<TreeNode>().Any())
                {
                    var tk = Convert.ToInt32(textBox5.Text);

                    if (tk == 0)
                    {
                        tk = DefaultCountTake;
                        textBox5.Text = tk.ToString();
                    }

                    Skip = Convert.ToInt32(textBox4.Text);
                    DefaultCountTake = tk;
                    GetData();
                    treeView1.TopNode.Expand();
                }
            }
            
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                if (treeView1.Nodes.Cast<TreeNode>().Any())
                {
                    var tk = Convert.ToInt32(textBox5.Text);

                    if (tk == 0)
                    {
                        tk = DefaultCountTake;
                        textBox5.Text = tk.ToString();
                    }

                    Skip = Convert.ToInt32(textBox4.Text);
                    DefaultCountTake = tk;
                    GetData();
                    treeView1.TopNode.Expand();
                }
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            var selectedError = SelectedUnknownError;

            if (selectedError != null && textBox3.Text != "")
            {
                var txtAnswer = textBox3.Text;
                var answer = new Answer()
                {
                    Id = ObjectId.GenerateNewId(),
                    Text = txtAnswer
                };

                var bsonValue = BsonDocument.Parse(answer.ToJson());

                var knowError = new KnownError()
                {
                    Message = selectedError.ErrorText,
                    Error = selectedError.Error,
                    Answer = bsonValue
                };

                await DataProcessor.SaveKnownErrorsIntoDb(knowError);

                var filter1 = Builders<BsonDocument>.Filter.Eq("Text", answer.Text);
                var answers = await DataProcessor.GetDataFind(filter1, "Answers",0,Int32.MaxValue);

                if (!answers.Any())
                {
                    await DataProcessor.SaveAnswerIntoDb(answer);
                }

                var filter2 = Builders<BsonDocument>.Filter.Eq("_id", selectedError.Id);
                await DataProcessor.DeleteDocument(filter2, "UnKnownError");

                var filter = Builders<BsonDocument>.Filter.Empty;
                var data = DataProcessor.GetDataFind(filter, "Answers", 0, Int32.MaxValue).Result;
                _listAnswers = data.Select(d => BsonSerializer.Deserialize<Answer>(d)).ToList();
                comboBox2.DataSource = _listAnswers;
                LoadData();

                textBox3.Text = "";
            }
           
        }

        private void splitContainer3_Panel1_Paint(object sender, PaintEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            var source = new BindingSource();
            var data = InitDbLogHelper.GetUnKnownErrors();
            source.DataSource = data;
            dataGridView1.DataSource = source;
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                var id = row.Cells["Id"].Value.ToString();
                var message = row.Cells["ErrorText"].Value.ToString();
                var error = row.Cells["Error"].Value.ToString();
                textBox2.Text = message;
                textBox3.Text = String.Empty;
                SelectedUnknownError = new UnKnownError(){Id = new ObjectId(id), ErrorText = message, Error = BsonDocument.Parse(error) };
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPageIndex == 1)
            {
                _listCollections = DataProcessor.GetCollections();
                LoadDataControls();
            }
            else if (e.TabPageIndex == 2)
            {
                var filter = Builders<BsonDocument>.Filter.Empty;
                var data = DataProcessor.GetDataFind(filter, "Answers", 0, Int32.MaxValue).Result;
                _listAnswers = data.Select(d => BsonSerializer.Deserialize<Answer>(d)).ToList();
                comboBox2.DataSource = _listAnswers;
                comboBox2.DisplayMember = "Text";
                comboBox2.ValueMember = "Id";
                textBox3.Text = String.Empty;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var selectedItem = comboBox2.SelectedItem as Answer;
            textBox3.Text = selectedItem == null ? String.Empty : selectedItem.Text;
        }
    }

    public static class TreeExtension
    {
        public static IEnumerable<TreeNode> FlattenTree(this TreeView tv)
        {
            return FlattenTree(tv.Nodes);
        }

        public static IEnumerable<TreeNode> FlattenTree(this TreeNodeCollection coll)
        {
            return coll.Cast<TreeNode>()
                .Concat(coll.Cast<TreeNode>()
                    .SelectMany(x => FlattenTree(x.Nodes)));
        }
        
    }

    

}

public class QueryModel
{
    public string CollectionName { get; set; }
    public string Key { get; set; }
    public object Value { get; set; }
}
class QueryDate
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
/*

      var filter = "db.Collection('Logs').find({'Request.isEmpty':'true'})";
            var dd =  DataProcessor.GetDataFind(filter, comboBox1.SelectedItem.ToString());
            BuildTree(dd,treeView1,true);
    
    
    build json

 
List<Post> posts = GetPosts();

JObject rss =
    new JObject(
        new JProperty("channel",
            new JObject(
                new JProperty("title", "James Newton-King"),
                new JProperty("link", "http://james.newtonking.com"),
new JProperty("description", "James Newton-King's blog."),
                new JProperty("item",
                    new JArray(
                        from p in posts
                        orderby p.Title
                        select new JObject(
                            new JProperty("title", p.Title),
                            new JProperty("description", p.Description),
                            new JProperty("link", p.Link),
                            new JProperty("category",
                                new JArray(
                                    from c in p.Categories
                                    select new JValue(c)))))))));

Console.WriteLine(rss.ToString());
     
     */


/*

    build from object

JObject o = JObject.FromObject(new
{
    channel = new
    {
        title = "James Newton-King",
        link = "http://james.newtonking.com",
        description = "James Newton-King's blog.",
        item =
            from p in posts
            orderby p.Title
            select new
            {
                title = p.Title,
                description = p.Description,
                link = p.Link,
                category = p.Categories
            }
    }
});


     //.Eq("Request.params.RNK", 25184);
        //Eq("Request.params", BsonNull.Value);
        //Eq("Response.RESULT.sessionId", "fpy3zb35rd41nw5uywl4z52k");
        //Eq("Request.message_id", "BARS-MESS-6725551");
        //Eq("MessageId", "BARS-MESS-6725552");
        //Eq("_id", ObjectId.Parse("59f495521274a61de4ff90c3"));
        

        //var filterBuilder = Builders<BsonDocument>.Filter;
        //var filter = filterBuilder.Gt("RequestDate", new DateTime(2016, 05, 06)) &
        //             filterBuilder.Lt("RequestDate", new DateTime(2016, 06, 04));

        // var filter = Builders<BsonDocument>.Filter.Eq("Request.params.RNK", 25184);

 */
