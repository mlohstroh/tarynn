using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Tarynn.JSON
{
    public class Collection
    {
        public string Name { get; private set; }

        private List<Document> mDocuments = new List<Document>();

        private int FileCounter = 0;
        private DocumentHandler _handler;

        public Collection(string name, DocumentHandler handler ,bool loadNow = false)
        {
            this.Name = name;
            this._handler = handler;
            if (loadNow)
                LoadDocuments();
        }

        public void LoadDocuments()
        {
            if(!Directory.Exists(CollectionPath()))
            {
                Directory.CreateDirectory(CollectionPath());
                //no reason to load since no documents
                return;
            }
            string[] allFiles = Directory.GetFiles(CollectionPath());
            IEnumerable<string> jsonFiles = allFiles.Where(element => element.IndexOf(".json") != -1);
            foreach (string s in jsonFiles)
            {
                string jsonBlob = File.ReadAllText(CollectionPath() + "\\" + s);
                JsonTextReader reader = new JsonTextReader(new StringReader(jsonBlob));
                JsonSerializer ser = new JsonSerializer();
                Dictionary<string, Object> dict = (Dictionary<string, Object>)ser.Deserialize(reader);
                Document doc = new Document(this, dict);
                mDocuments.Add(doc);
                FileCounter++; //TODO: Fix this because this is awful
            }
        }

        private string CollectionPath()
        {
            return _handler.RootPath + "\\" + Name;
        }

        public string GetFileName(Document doc)
        {
            return ++FileCounter + ".json";
        }
    }
}
