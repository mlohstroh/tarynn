using System;
using System.Collections.Generic;
using LitJson;

namespace WitAI
{
    public class WitOutcome
    {
        public string QueryText { get; private set; }
        public string Intent { get; private set; }
        public Dictionary<string, List<Dictionary<string, object>>> Entities { get; private set; }
        public int Confidence { get; private set; }

        public WitOutcome()
        {

        }
    }
}
