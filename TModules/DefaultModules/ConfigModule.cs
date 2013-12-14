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

        private Dictionary<string, string> configValues = new Dictionary<string, string>();

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

                configValues[data["key"].ToString()] = data["value"].ToString();
            }
        }

        public string DataForKey(string key)
        {
            return configValues[key];
        }

        public void PutData(string key, string value)
        {
            configValues[key] = value;

            JsonData data = new JsonData();
            data["key"] = key;
            data["value"] = value;

            _couch.CreateDocument(SERVER_ADDRESS, DB_NAME, data.ToJson());

            LoadDocs();
        }
    }
}
