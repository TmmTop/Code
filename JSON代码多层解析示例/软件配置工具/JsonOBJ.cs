
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace 软件配置工具
{
    public enum JsonItemType
    {
        String,
        Array,
        Integer,
        Node
    }

    public sealed class Json
    {
        #region -- Private Fields --  
        private IList<JsonItem> _col;
        private int _deep = 0;
        private Json _parent;
        #endregion

        #region -- Methods --  
        public Json()
        {
            this._col = new List<JsonItem>();
        }
        public void Add(string key, string value)
        {
            if (key == null || key == "" || value == null) throw new ArgumentException("键值不能为空,参数不能为null");
            JsonItem si = new JsonItem();
            si.ItemType = JsonItemType.String;
            si.Key = key;
            si.Value = value;
            this._col.Add(si);
        }
        public void Add(string key, int value)
        {
            if (key == null || key == "") throw new ArgumentException("键值不能为空,参数不能为null");
            JsonItem si = new JsonItem();
            si.ItemType = JsonItemType.Integer;
            si.Key = key;
            si.Value = value;
            this._col.Add(si);
        }
        public void Add(string key, IList<object> value)
        {
            if (key == null || key == "" || value == null) throw new ArgumentException("键值不能为空,参数不能为null");
            JsonItem si = new JsonItem();
            si.ItemType = JsonItemType.Array;
            si.Key = key;
            si.Value = value;
            this._col.Add(si);
        }
        public void Add(string key, Json value)
        {
            if (key == null || key == "" || value == null) throw new ArgumentException("键值不能为空,参数不能为null");
            JsonItem si = new JsonItem();
            si.ItemType = JsonItemType.Node;
            si.Key = key;
            si.Value = value;
            this._col.Add(si);
        }
        public override string ToString()
        {
            return new JsonConvert.Writer(this).ToString();
        }
        public static Json Parse(string jsonString)
        {
            return new JsonConvert.Reader(jsonString).ToJson();

        }
        #endregion

        #region -- Properties --  
        public int Length
        {
            get { return this._col.Count; }
        }
        public Jszn this[string key]
        {
            get
            {
                foreach (JsonItem si in this._col)
                {
                    if (si.Key == key)
                    {
                        return new Jszn(si);
                    }
                }
                throw new ArgumentException("没有找到Key为 " + key + " 的数据序列");
            }
        }
        internal JsonItem this[int index]
        {
            get
            {
                return (JsonItem)this._col[index];
            }
        }
        public int Deep
        {
            get { return this._deep; }
            set { this._deep = value; }
        }
        public Json Parent
        {
            get { return this._parent; }
            set { this._parent = value; }
        }
        #endregion
    }
    public sealed class Jszn
    {
        #region -- Private Fields --  
        JsonItem _item;
        #endregion

        #region -- Method --  
        public Jszn(JsonItem item)
        {
            this._item = item;
        }
        public override string ToString()
        {
            return this._item.Value.ToString();
        }
        public int ToInt()
        {
            if (this._item.ItemType == JsonItemType.Integer)
            {
                return Convert.ToInt32(this._item.Value);
            }
            else
            {
                throw new ArgumentOutOfRangeException("该返回数据类型不应为数字整型 Integer");
            }
        }
        public string[] ToArray()
        {
            if (this._item.ItemType == JsonItemType.Array)
            {
                return (string[])this._item.Value;
            }
            else
            {
                throw new ArgumentOutOfRangeException("该返回数据类型不应为数组型 Array");
            }
        }
        #endregion

        #region -- Properties --  
        public Jszn this[string key]
        {
            get
            {
                if (this._item.ItemType == JsonItemType.Node)
                {
                    Json json = (Json)this._item.Value;
                    return json[key];
                }
                else
                {
                    throw new ArgumentOutOfRangeException("该返回数据类型不应为数据节点类型 Node");
                }
            }
        }
        public Jszn this[int index]
        {
            get
            {
                if (this._item.ItemType == JsonItemType.Array)
                {
                    IList<object> result = (IList<object>)this._item.Value;
                    if (result[index].GetType() == typeof(System.String))
                    {
                        JsonItem ji = new JsonItem();
                        ji.ItemType = JsonItemType.String;
                        ji.Key = "";
                        ji.Value = result[index];
                        return new Jszn(ji);
                    }
                    else if (result[index].GetType() == typeof(System.Collections.Generic.List<object>))
                    {
                        JsonItem ji = new JsonItem();
                        ji.ItemType = JsonItemType.Array;
                        ji.Key = "";
                        ji.Value = result[index];
                        return new Jszn(ji);
                    }
                    else if (result[index].GetType() == typeof(System.Int32))
                    {
                        JsonItem ji = new JsonItem();
                        ji.ItemType = JsonItemType.Integer;
                        ji.Key = "";
                        ji.Value = result[index];
                        return new Jszn(ji);
                    }
                    else if (result[index].GetType() == typeof(Json))
                    {
                        JsonItem ji = new JsonItem();
                        ji.ItemType = JsonItemType.Node;
                        ji.Key = "";
                        ji.Value = result[index];
                        return new Jszn(ji);
                    }
                    else
                    {
                        throw new Exception("未知的返回类型!");
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException("该返回数据类型不应为数组类型 Array");
                }
            }
        }
        #endregion
    }

    public class JsonItem
    {
        #region -- Privtae Fields --  
        private string _key;
        private JsonItemType _itemType;
        private object _value;
        #endregion

        #region -- Properties --  
        public string Key
        {
            get { return this._key; }
            set { this._key = value; }
        }
        public JsonItemType ItemType
        {
            get { return this._itemType; }
            set { this._itemType = value; }
        }
        public object Value
        {
            get { return this._value; }
            set { this._value = value; }
        }
        #endregion
    }

    public sealed class JsonConvert
    {
        public sealed class Reader
        {
            #region -- Privtae Fields --  
            private string _xml;
            private int current;
            #endregion

            #region -- Method --  
            public Reader(string Xml)
            {
                this._xml = Xml;
                this.current = 0;
            }
            public Json ToJson()
            {
                return EnumKey();
            }
            private Json EnumKey()
            {
                bool dump = false, enumKey = true;
                string sKey = "", sValue = "";
                IList<object> aValue;
                int iValue = 0;
                int KeyEnd = 0, KeyStart = 0, KeyCount = 0;
                Json json = new Json();
                if (this._xml[current] == '{')
                {
                    this.Plus();
                    this.Skip();
                    while (enumKey)
                    {
                        #region  -- 取得Key --  
                        if (this._xml[current] != '"') break;
                        this.Plus();
                        KeyStart = current;
                        dump = false;
                        while (!dump)
                        {
                            while (this._xml[current] != '"')
                            {
                                current++;
                            }
                            if (this._xml[current - 1] != '\\')
                            {
                                dump = true;
                            }
                        }
                        KeyEnd = current;
                        KeyCount = KeyEnd - KeyStart;
                        sKey = this.Read(KeyStart, KeyCount);
                        this.Plus();
                        #endregion
                        #region -- Key Value 分割 --  
                        if (this._xml[current] != ':')
                        {
                            if (this._xml[current] != ' ')
                            {
                                this.Skip();
                                if (this._xml[current] != ':')
                                {
                                    break;
                                }
                                else
                                {
                                    this.Plus();
                                    this.Skip();
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            this.Plus();
                            this.Skip();
                        }
                        #endregion
                        #region -- 字符型Value --  
                        if (this._xml[current] == '"')
                        {
                            this.Plus();
                            KeyStart = current;
                            dump = false;
                            while (!dump)
                            {
                                while (this._xml[current] != '"')
                                {
                                    this.Plus();
                                }
                                if (this._xml[current - 1] != '\\')
                                {
                                    dump = true;
                                }
                            }
                            KeyEnd = current;
                            KeyCount = KeyEnd - KeyStart;
                            sValue = this.Read(KeyStart, KeyCount);
                            json.Add(sKey, sValue);
                            this.Plus();
                        }
                        #endregion
                        #region -- 数组型Value --  
                        else if (this._xml[current] == '[')
                        {
                            aValue = EnumList();
                            json.Add(sKey, aValue);
                        }
                        #endregion
                        #region -- 节点型Value --  
                        else if (this._xml[current] == '{')
                        {
                            Json sJson = EnumKey();
                            json.Add(sKey, sJson);
                        }
                        #endregion
                        #region -- 整数型Value --  
                        else
                        {
                            KeyStart = current;
                            while (this._xml[current] != ',' && this._xml[current] != '}')
                            {
                                this.Plus();
                            }
                            KeyEnd = current;
                            KeyCount = KeyEnd - KeyStart;
                            try
                            {
                                iValue = Convert.ToInt32(this.Read(KeyStart, KeyCount));
                            }
                            catch
                            {
                                break;
                            }
                            json.Add(sKey, iValue);
                        }
                        #endregion
                        #region -- 判断结束符 --  
                        this.Skip();
                        if (this._xml[current] == ',')
                        {
                            this.Plus();
                            this.Skip();
                        }
                        else if (this._xml[current] == '}')
                        {
                            enumKey = false;
                            this.Plus();
                            this.Skip();
                        }
                        else
                        {
                            break;
                        }
                        #endregion
                    }
                }
                return json;
            }
            private IList<object> EnumList()
            {
                bool dump = false, enumKey = true;
                string sValue = "";
                IList<object> subValue = new List<object>(), aValue;
                int iValue = 0;
                int KeyEnd = 0, KeyStart = 0, KeyCount = 0;
                if (this._xml[current] == '[')
                {
                    this.Plus();
                    this.Skip();
                    while (enumKey)
                    {
                        #region -- 字符型Value --  
                        if (this._xml[current] == '"')
                        {
                            this.Plus();
                            KeyStart = current;
                            dump = false;
                            while (!dump)
                            {
                                while (this._xml[current] != '"')
                                {
                                    this.Plus();
                                }
                                if (this._xml[current - 1] != '\\')
                                {
                                    dump = true;
                                }
                            }
                            KeyEnd = current;
                            KeyCount = KeyEnd - KeyStart;
                            sValue = this.Read(KeyStart, KeyCount);
                            subValue.Add(sValue);
                            this.Plus();
                        }
                        #endregion
                        #region -- 数组型Value --  
                        else if (this._xml[current] == '[')
                        {
                            aValue = EnumList();
                            subValue.Add(aValue);
                        }
                        #endregion
                        #region -- 节点型Value --  
                        else if (this._xml[current] == '{')
                        {
                            Json sJson = EnumKey();
                            subValue.Add(sJson);
                        }
                        #endregion
                        #region -- 整数型Value --  
                        else
                        {
                            KeyStart = current;
                            while (this._xml[current] != ',' && this._xml[current] != ']')
                            {
                                this.Plus();
                            }
                            KeyEnd = current;
                            KeyCount = KeyEnd - KeyStart;
                            try
                            {
                                iValue = Convert.ToInt32(this.Read(KeyStart, KeyCount));
                            }
                            catch
                            {
                                break;
                            }
                            subValue.Add(iValue);
                        }
                        #endregion
                        #region -- 判断结束符 --  
                        this.Skip();
                        if (this._xml[current] == ',')
                        {
                            this.Plus();
                            this.Skip();
                        }
                        else if (this._xml[current] == ']')
                        {
                            enumKey = false;
                            this.Plus();
                            this.Skip();
                        }
                        else
                        {
                            break;
                        }
                        #endregion
                    }
                }
                return subValue;
            }
            private void Plus()
            {
                this.current++;
            }
            private void Plus(int count)
            {
                this.current += count;
            }
            private void Skip()
            {
                while (current < this._xml.Length && (this._xml[current].ToString() == " " || this._xml[current].ToString() == "\r" || this._xml[current].ToString() == "\n"))
                {
                    current++;
                }
            }
            private string Read(int start, int count)
            {
                return this._xml.Substring(start, count);
            }
            #endregion
        }
        public sealed class Writer
        {
            #region -- Private Fields --  
            private Json _json;
            #endregion

            #region -- Method --  
            public Writer(Json json)
            {
                this._json = json;
            }
            public override string ToString()
            {
                return EnumKey(this._json);
            }
            private string EnumKey(Json j)
            {
                string result = "{";
                for (int i = 0; i < j.Length; i++)
                {
                    JsonItem ji = j[i];
                    result += "\"" + ji.Key + "\":";
                    if (ji.ItemType == JsonItemType.Array)
                    {
                        result += EnumList((IList<object>)ji.Value);
                    }
                    else if (ji.ItemType == JsonItemType.Integer)
                    {
                        result += ji.Value;
                    }
                    else if (ji.ItemType == JsonItemType.String)
                    {
                        result += "\"" + ji.Value + "\"";
                    }
                    else if (ji.ItemType == JsonItemType.Node)
                    {
                        result += EnumKey((Json)ji.Value);
                    }
                    if (i < j.Length - 1) result += ",";
                }
                result += "}";
                return result;
            }
            private string EnumList(IList<object> l)
            {
                string result = "[";
                for (int i = 0; i < l.Count; i++)
                {
                    if (l[i].GetType() == typeof(System.Collections.Generic.List<object>))
                    {
                        result += EnumList((IList<object>)l[i]);
                    }
                    else if (l[i].GetType() == typeof(System.Int32))
                    {
                        result += l[i].ToString();
                    }
                    else if (l[i].GetType() == typeof(System.String))
                    {
                        result += "\"" + l[i].ToString() + "\"";
                    }
                    else if (l[i].GetType() == typeof(Json))
                    {
                        result += EnumKey((Json)l[i]);
                    }
                    if (i < l.Count - 1) result += ",";
                }
                result += "]";
                return result;
            }
            #endregion
        }
    }
    public class ReadeJSON
    {
        public Dictionary<int, string> getJsonNumOnes(string startstr)//获取json子项只有一个数量单位的信息
        {
            string jsonStr = GetConfig()[startstr][0].ToString();
            int nums = SubstringCount(jsonStr, "id");//获取json中,出现的次数
            Dictionary<int, string> kvDictonary = new Dictionary<int, string>();//将json里的value填充字典
            kvDictonary.Add(0, GetConfig()[startstr][0].ToString());
            return kvDictonary;
        }
        public Dictionary<int, string> getJsonNum(string startstr)//获取json子项只有多个数量单位的信息
        {
            string jsonStr = GetConfig()[startstr][0].ToString();
            int nums = SubstringCount(jsonStr, "id");
            Dictionary<int, string> kvDictonary = new Dictionary<int, string>();
            for (int i = 0; i < nums; i++)
            {
                kvDictonary.Add(i, GetConfig()[startstr][i].ToString());
            }
            return kvDictonary;
        }
        public int getJsonNums(string startstr)
        {
            string jsonStr = GetConfig()[startstr].ToString();
            int nums = SubstringCount(jsonStr, "[");
            return nums;
        }
        public int SubstringCount(string str, string substring)
        {
            if (str.Contains(substring))
            {
                string strReplaced = str.Replace(substring, "");
                return (str.Length - strReplaced.Length) / substring.Length;
            }

            return 0;
        }
        public Json GetConfig()
        {
            FileStream fs = new FileStream("./config.json", FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fs, Encoding.Default);
            StringBuilder jsonArrayText_tmp = new StringBuilder();
            string input = null;
            while ((input = sr.ReadLine()) != null)
            {
                jsonArrayText_tmp.Append(input);
            }
            sr.Close();
            string str = jsonArrayText_tmp.ToString();
            Json json = new JsonConvert.Reader(str).ToJson();
            return json;
        }
    }
}

