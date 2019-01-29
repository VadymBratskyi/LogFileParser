using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;

namespace LogParserEithMongoDb.Process
{
    class BuildTree : LogAnaliz
    {
        public BuildTree()
        {
            
        }

        public BuildTree(JObject obj)
        {
            jsObject = obj;
        }

        public TreeNode Json2Tree(JObject obj, bool withValue)
        {
                
            TreeNode parent = new TreeNode();

            foreach (var token in obj)
            {
                parent.Text = token.Key.ToString();
                TreeNode child = new TreeNode();
                var oldName = sameProperties.FirstOrDefault(o => o.Key.Contains(token.Key.ToString())).Value;
                child.Text = oldName != null ? oldName : token.Key;

                if (token.Value.Type.ToString() == "Object")
                {
                    JObject o = (JObject)token.Value;
                    child =  Json2Tree(o, withValue);
                    child.Text = oldName != null ? oldName : parent.Text;
                    child.ImageIndex = 1;
                    parent.Nodes.Add(child);
                }
                else if (token.Value.Type.ToString() == "Array")
                {
                    int i = -1;
                    foreach (var val in token.Value)
                    {
                        if (val.Type.ToString() == "Object")
                        {
                            TreeNode objNode = new TreeNode();
                            i++;
                            JObject o = (JObject)val;
                            objNode =  Json2Tree(o, withValue);
                            objNode.Text = "[" + i + "]";
                            child.Nodes.Add(objNode);
                        }
                        else if (val.Type.ToString() == "Array")
                        {
                            i++;
                            TreeNode dtArray = new TreeNode();
                            foreach (var vl in val)
                            {
                                dtArray.Text = token.Key + "[" + i + "]";
                                dtArray.Nodes.Add(vl.ToString());
                            }
                            child.Nodes.Add(dtArray);
                        }
                        else
                        {
                            child.Nodes.Add(val.ToString());
                        }
                    }
                    parent.Nodes.Add(child);
                }
                else
                {
                    if (token.Value.ToString() == "" && withValue)
                    {
                        child.Nodes.Add("null");
                    }
                    else if (token.Value.ToString() != "" && withValue)
                    {
                        child.Nodes.Add(token.Value.ToString());
                    }
                    parent.Nodes.Add(child);
                }
            }
            return parent;
        }
    }
}
