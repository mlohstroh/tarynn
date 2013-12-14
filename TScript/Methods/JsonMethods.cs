using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitJson;

namespace TScript.Methods
{
    public class JsonMethods : MethodPackage
    {
        public JsonMethods()
            : base("JSON")
        {
            this.methodNames.Add("string_to_json");
            this.methodNames.Add("json_to_string");
            this.methodNames.Add("data_for_key");
            this.supportedTypes.Add("json");
        }

        public override TObject GetNewObjectForType(string type, string name)
        {
            TObject obj = new TObject();
            obj.Name = name;
            obj.InnerType = type;

            switch (type)
            {
                case "json":
                    obj.Value = new JsonData();
                    break;
            }

            return obj;
        }

        public override TObjectChange GetResultForMethod(string method, object[] args)
        {
            TObjectChange change = new TObjectChange();

            switch (method)
            {
                case "string_to_json":
                    string stringValue = (string)Host.GetObjectValue(args[0]);
                    TObject endVariable = (TObject)args[1];
                    change = Host.MakeChange(endVariable, JsonMapper.ToObject(stringValue));
                    break;
                case "json_to_string":
                    TObject jsonData = (TObject)args[0];
                    string jsonString = ((JsonData)jsonData.Value).ToString();
                    change = Host.MakeChange((TObject)args[1], jsonString);
                    break;
                case "data_for_key":
                    string key = Host.GetRawTextFromArgIndex(0);
                    TObject data = (TObject)args[1];
                    string value = ((JsonData)data.Value)[key].ToString();
                    change = Host.MakeChange((TObject)args[2], value);
                    break;
            }
            return change;
        }
    }
}
