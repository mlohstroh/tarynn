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
    class StorageModule : TModule
    {
        private DB _couch = new DB();

        const string SERVER_ADDRESS = "http://127.0.0.1:5984";
        const string DB_NAME = "tarynn-storage";

        private Dictionary<string, string> storageValues = new Dictionary<string, string>();

        public StorageModule(ModuleManager host)
            : base("Storage", host)
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
            storageValues.Clear();

            DocInfo[] docs = _couch.GetAllDocuments(SERVER_ADDRESS, DB_NAME);

            foreach (DocInfo doc in docs)
            {
                string json = _couch.GetDocument(SERVER_ADDRESS, DB_NAME, doc.ID);
                JsonData data = JsonMapper.ToObject(json);

                storageValues[data["key"].ToString()] = data["value"].ToString();
            }
        }

        public string JSONForKey(string key)
        {
            return storageValues[key];
        }
    }
}
