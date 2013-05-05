using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarynn.Core
{
    public class Query
    {
        public Query(string text)
        {
            OriginalText = text;
            State = QueryState.New;
        }

        public QueryState State { get; set; }

        public string OriginalText { get; set; }

        public string ResponseText { get; set; }
    }
}
