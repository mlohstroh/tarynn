using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Tarynn.JSON
{
    public class Document
    {
        public Dictionary<string, Object> Contents { get; set; }

        private string _fileName = null;

        public Collection Collection { get; private set; }

        public Document(Collection collection)
        {
            Collection = collection;
        }

        public Document(Collection collection, Dictionary<string, Object> contents)
        {
            Collection = collection;
            this.Contents = contents;
        }

        public string DictToJson()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter  sw = new StringWriter(sb);

            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.WriteStartObject();
                foreach (string key in Contents.Keys)
                {
                    object outObject;
                    Contents.TryGetValue(key, out outObject);

                    if (outObject.GetType() == typeof(Array))
                    {

                    }
                    jw.WritePropertyName(key);
                    jw.WriteValue(outObject.ToString());
                }
                jw.WriteEnd();
                jw.WriteEndObject();
                return jw.ToString();
            }
        }

        public void Save()
        {
            if (IsNew())
            {
                File.Create(Collection.GetFileName());
            }
        }

        public bool IsNew()
        {
            return _fileName == null;
        }
    }
}
