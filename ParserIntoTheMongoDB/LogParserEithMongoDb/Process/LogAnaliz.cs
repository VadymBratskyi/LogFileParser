using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;

namespace LogParserWithMongoDb.Process
{
    class  LogAnaliz
    {
        #region Properties Data
        protected JObject jsObject = new JObject();
        protected BsonDocument bsObject = new BsonDocument();
        protected Dictionary<string, string> sameProperties = new Dictionary<string, string>();
        #endregion


        public LogAnaliz()
        {
            sameProperties.Clear();
        }

        public JObject GetJsObject
        {
            get { return jsObject; }
        }

        public void CompareJson(JObject obj)
        {
            foreach (var token in obj.Properties())
            {
                if (token.Value.Type == JTokenType.Object)
                {
                    CompareJson((JObject)token.Value);
                }
                else if (token.Value.Type == JTokenType.Array)
                {
                    foreach (var val in token.Value)
                    {
                        if (val.Type == JTokenType.Object)
                        {
                            CompareJson((JObject)val);
                        }
                        else if (val.Type == JTokenType.Array)
                        {
                            foreach (var vl in val)
                            {
                                CompareJson((JObject)vl);
                            }
                        }
                        else
                        {
                            ParseObject(val, obj);
                        }
                    }
                }
                else
                {
                    ParseObject(token, obj);
                }
            }
        }


        private void ParseObject(JToken token, JObject secJObject)
        {
            if (token!=null)
            {
                var propInObj = jsObject.SelectToken(token.Path);

                if (propInObj == null)
                {
                    var arrProperties = token.Path.Split('.');
                    StringBuilder strBuildObject = new StringBuilder();

                    strBuildObject.Append(arrProperties.First());

                    if (arrProperties.Length > 1)
                    {

                        for (int i = 1; i < arrProperties.Length + 1; i++)
                        {
                            var selObj = jsObject.SelectToken(strBuildObject.ToString());

                            if (selObj == null)
                            {
                                JToken parentObj;
                                var tp = GetType(strBuildObject.ToString(), secJObject, out parentObj);

                                if (tp == JTokenType.Object)
                                {
                                    if (jsObject.SelectToken(parentObj.Path) == null)
                                    {
                                        var obj = (JObject)jsObject.SelectToken(parentObj.Parent.Path);
                                        obj.Add(secJObject.Parent);
                                    }
                                    else
                                    {
                                        var objJToken = jsObject.SelectToken(parentObj.Path);
                                        if (objJToken.Type == JTokenType.Object)
                                        {
                                            var mainObj = (JObject)jsObject.SelectToken(parentObj.Path);
                                            mainObj.Add(token);
                                        }
                                        else
                                        {
                                            var property = GetProperty(parentObj.Path);
                                            var newName = "obj_" + property;
                                            if (!sameProperties.ContainsKey(newName))
                                            {
                                                sameProperties.Add(newName, property);
                                                var obj = (JObject)jsObject.SelectToken(objJToken.Parent.Parent.Path);
                                                obj.Add(newName, secJObject);
                                            }
                                            else
                                            {
                                                var newPath = token.Path.Replace(property, newName);
                                                var newToken = jsObject.SelectToken(newPath);
                                                ParseObject(newToken, secJObject);
                                            }
                                        }
                                    }
                                }
                                else if (tp == JTokenType.Array)
                                {
                                    var sameObj = jsObject.SelectToken(parentObj.Path);
                                    if (sameObj != null)
                                    {
                                        var property = GetProperty(parentObj.Path);
                                        var newName = "arr_" + property;
                                        if (!sameProperties.ContainsKey(newName))
                                        {

                                            sameProperties.Add(newName, property);
                                            var obj = (JObject)jsObject.SelectToken(sameObj.Parent.Parent.Path);
                                            obj.Add(newName, secJObject.Parent);
                                        }
                                        else
                                        {
                                            var newPath = token.Path.Replace(property, newName);
                                            var newToken = jsObject.SelectToken(newPath);
                                            ParseObject(newToken, secJObject);
                                        }
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                if (i < arrProperties.Length)
                                {
                                    strBuildObject.Append('.' + arrProperties[i]);
                                }

                            }

                        }

                    }
                    else
                    {
                        var obj = (JProperty)token;
                        jsObject.Add(new JProperty(obj.Name, ""));
                    }

                }
            }
        }


        private string GetProperty(string path)
        {
            var lastPoin = path.Reverse().ToString().IndexOf('.');
            var result = path.Substring(path.Length - lastPoin);

            return result;
        }


        private JTokenType GetType(string path, JObject secJObject, out JToken parentObject)
        {
            if (path.Contains('['))
            {
                parentObject = secJObject.Parent;
                return secJObject.Parent.Type;
            }
            else
            {
                parentObject = secJObject.Parent;
                return secJObject.Type;
            }

        }

    }
}
