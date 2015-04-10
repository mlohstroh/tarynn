using System;
using System.Collections.Generic;
using LitJson;

namespace WitAI
{
    public class WitOutcome
    {
        public string QueryText { get; private set; }
        public string Intent { get; private set; }
        public Dictionary<string, List<WitEntity>> Entities { get; private set; }
        public double Confidence { get; private set; }

        public WitOutcome(JsonData data)
        {
            QueryText = (string) data["_text"];
            Intent = (string) data["intent"];
            Confidence = (double) data["confidence"];
            Entities = new Dictionary<string, List<WitEntity>>();

            if (data["entities"] != null)
            {
                foreach (KeyValuePair<string, JsonData> node in data["entities"])
                {
                    List<WitEntity> entities = new List<WitEntity>();
                    if (node.Value.IsArray)
                    {
                        int count = node.Value.Count;
                        for (int i = 0; i < count; i++)
                        {
                            WitEntity ent = new WitEntity(node.Value[i]);
                            entities.Add(ent);
                        }
                    }
                    Entities.Add(node.Key, entities);
                }
            }
        }
    }
}
