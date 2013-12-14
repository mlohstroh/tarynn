using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModules.Core;
using SharpCouch;
using LitJson;

namespace TModules.DefaultModules
{
    public class ConfigModule : TModule
    {
        private DB _couch = new DB();

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
            if (!_couch.DBExists(SERVER_ADDRESS, DB_NAME))
            {
                _couch.CreateDatabase(SERVER_ADDRESS, DB_NAME);
            }

            LoadDocs();
        }

        private void LoadDocs()
        {
            configValues.Clear();

            DocInfo[] docs = _couch.GetAllDocuments(SERVER_ADDRESS, DB_NAME);

            foreach (DocInfo doc in docs)
            {
                string json = _couch.GetDocument(SERVER_ADDRESS, DB_NAME, doc.ID);
                JsonData data = JsonMapper.ToObject(json);

                Dictionary<string, string> dict = new Dictionary<string,string>();
                dict["key"] = data["value"].ToString();

                configValues[doc.ID] = dict;
            }
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

            if (id != null)
            {
                _couch.DeleteDocument(SERVER_ADDRESS, DB_NAME, id);
            }

            JsonData data = new JsonData();
            data["key"] = key;
            data["value"] = value;

            _couch.CreateDocument(SERVER_ADDRESS, DB_NAME, data.ToJson());

            LoadDocs();
        }
    }
}
