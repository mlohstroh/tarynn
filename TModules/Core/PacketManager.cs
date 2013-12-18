using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitJson;
using SharpCouch;

namespace TModules.Core
{
    public class PacketManager
    {
        public static PacketManager SharedInstance { get; private set; }

        //packets are readonly
        private Dictionary<string, Dictionary<string, List<JsonData>>> _packets = new Dictionary<string, Dictionary<string, List<JsonData>>>();

        private DB _couch = new DB();

        const string SERVER_ADDRESS = "http://127.0.0.1:5984";
        const string DB_NAME = "tarynn-packets";

        public PacketManager()
        {
            InitDatabase();
            SharedInstance = this;
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
            _packets.Clear();

            DocInfo[] docs = _couch.GetAllDocuments(SERVER_ADDRESS, DB_NAME);

            foreach (DocInfo doc in docs)
            {
                string json = _couch.GetDocument(SERVER_ADDRESS, DB_NAME, doc.ID);
                JsonData data = JsonMapper.ToObject(json);

                Dictionary<string, JsonData> category = new Dictionary<string, JsonData>();

                category[data["category"].ToString()] = data["data"];
            }
        }

        private void SaveDocs()
        {
            foreach (var pair in _packets)
            {
                string id = pair.Key;
                JsonData d = new JsonData();
                d["category"] = pair.Value.ElementAt(0).Key;
                d["data"] = JsonMapper.ToObject(pair.Value.ElementAt(0).Value.ToString());
                _couch.DeleteDocument(SERVER_ADDRESS, DB_NAME, id);
                _couch.CreateDocument(SERVER_ADDRESS, DB_NAME, d.ToString());
            }
        }
        
        public void PushPacket(string module, JsonData data)
        {
            foreach(var pair in _packets)
            {

                if(pair.Value[module] == null)
                {
                    pair.Value[module] = new List<JsonData>();
                }

                pair.Value[module].Add(data);
                break;
            }
        }

        public JsonData GetLastPacketByCategory(string category)
        {
            foreach (var pair in _packets)
            {
                if (pair.Value[category] != null)
                {
                    if (pair.Value[category].Count > 0)
                        return null;
                    else
                        return pair.Value[category][pair.Value.Count - 1];
                }
            }

            return null;
        }
    }
}
