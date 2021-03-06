﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TScript.Methods
{
    class StructureMethods : MethodPackage
    {
        public StructureMethods()
            : base("Structure Methods")
        {
            this.methodNames.Add("csv_to_dict");
            this.methodNames.Add("dict_to_csv");
            this.methodNames.Add("get_item");
            this.methodNames.Add("set_item");
            this.supportedTypes.Add("dict");
        }

        public override TObject GetNewObjectForType(string type, string name)
        {
            TObject obj = new TObject();
            obj.Name = name;
            obj.InnerType = type;

            switch (type)
            {
                case "dict":
                    obj.Value = new Dictionary<string, string>();
                    break;
            }

            return obj;
        }

        public override TObjectChange GetResultForMethod(string method, object[] args)
        {
            TObjectChange change = new TObjectChange();
            switch (method)
            {
                case "get_item":
                    change = HandleGetItem(args);
                    break;
                case "csv_to_dict":
                    change = HandleCsvToDict(args);
                    break;
                case "dict_to_csv":
                    change = HandleDictToCsv(args);
                    break;
                case "set_item":
                    change = HandleSetItem(args);
                    break;
            }
            return change;
        }

        private TObjectChange HandleGetItem(object[] args)
        {
            Dictionary<string, string> dict = (Dictionary<string, string>)Host.GetObjectValue(args[1]);
            string key = Host.GetObjectValue(args[0]).ToString();

            string value;
            dict.TryGetValue(key, out value);

            return Host.MakeChange((TObject)args[1], value);
        }

        private TObjectChange HandleCsvToDict(object[] args)
        {
            Dictionary<string, string> dict = (Dictionary<string, string>)Host.GetObjectValue(args[1]);

            string fullBlob = Host.GetObjectValue(args[0]).ToString();

            string[] lines = fullBlob.Split(';');
            for(int i = 0; i < lines.Length - 1; i++)
            {
                string[] splitLine = lines[i].Split(',');
                dict.Add(splitLine[0], splitLine[1]);
            }

            return Host.MakeChange((TObject)args[1], dict);
        }

        private TObjectChange HandleDictToCsv(object[] args)
        {
            Dictionary<string, string> dict = (Dictionary<string, string>)Host.GetObjectValue(args[0]);

            StringBuilder builder = new StringBuilder();

            foreach (string key in dict.Keys)
            {
                string value;
                dict.TryGetValue(key, out value);

                builder.Append(key + "," + value + ";");
            }

            return Host.MakeChange((TObject)args[1], builder.ToString());
        }

        private TObjectChange HandleSetItem(object[] args)
        {
            Dictionary<string, string> dict = (Dictionary<string, string>)Host.GetObjectValue(args[2]);

            string key = Host.GetObjectValue(args[0]).ToString();
            string value = Host.GetObjectValue(args[1]).ToString();
            if (dict.ContainsKey(key))
            {
                dict.Remove(key);
            }
            dict.Add(key, value);

            return Host.MakeChange((TObject)args[2], dict);
        }
    }
}
