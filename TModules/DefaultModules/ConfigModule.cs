using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModules.Core;
using LitJson;

namespace TModules.DefaultModules
{
    public class ConfigModule : TModule
    {

        const string SERVER_ADDRESS = "http://127.0.0.1:5984";
        const string DB_NAME = "tarynn-config";

        private Dictionary<string, Dictionary<string, string>> configValues = new Dictionary<string, Dictionary<string, string>>();

        public string LastUpdatedValue { get; private set; }

        public ConfigModule(ModuleManager host)
            : base("Config", host)
        {
            InitDatabase();
        }

        private void InitDatabase()
        {

            LoadDocs();
        }

        private void LoadDocs()
        {
            configValues.Clear();

            //DocInfo[] docs = _couch.GetAllDocuments(SERVER_ADDRESS, DB_NAME);

            //foreach (DocInfo doc in docs)
            //{
            //    string json = _couch.GetDocument(SERVER_ADDRESS, DB_NAME, doc.ID);
            //    JsonData data = JsonMapper.ToObject(json);

            //    Dictionary<string, string> dict = new Dictionary<string,string>();
            //    dict["key"] = data["value"].ToString();

            //    configValues[doc.ID] = dict;
            //}
        }

        public string ConfigValueFor(string key)
        {
            foreach (var pair in configValues)
            {
                if(pair.Value.ContainsKey(key))
                {
                    return pair.Value[key];
                }
            }
            return null;
        }
        public void PutData(string key, string value)
        {
            string id = null;

            foreach (var pair in configValues)
            {
                if (pair.Value.ContainsKey(key))
                {
                    id = pair.Key;
                }
            }


            LoadDocs();
        }
    }
}
