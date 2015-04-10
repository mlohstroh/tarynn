using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitAI
{
    public class WitResponse
    {
        public string MsgID { get; private set; }
        public string QueryText { get; private set; }
        public List<WitOutcome> Outcomes { get; private set; }

        public WitResponse()
        {
            
        }
    }
}
