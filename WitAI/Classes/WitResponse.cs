using System;
using System.Collections.Generic;
using LitJson;

namespace WitAI
{
    public class WitResponse
    {
        public string MsgID { get; private set; }
        public string QueryText { get; private set; }
        public List<WitOutcome> Outcomes { get; private set; }
        public string RawContent { get; private set; }

        public WitResponse(JsonData data, string raw)
        {
            RawContent = raw;
            MsgID = (string)data["msg_id"];
            QueryText = (string) data["_text"];
            Outcomes = new List<WitOutcome>();

            // make sure it is an array before trying to parse anything
            if (data["outcomes"].IsArray)
            {
                int count = data["outcomes"].Count;
                for (int i = 0; i < count; i++)
                {
                    WitOutcome outcome = new WitOutcome(data["outcomes"][i]);
                    Outcomes.Add(outcome);
                }
            }
        }
    }
}
