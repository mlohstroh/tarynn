using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarynn.Core
{
    public class Query
    {
        private Tarynn instance;

        public Query(string text, Tarynn t)
        {
            OriginalText = text;
            State = QueryState.New;
            instance = t;
        }

        public QueryState State { get; set; }

        public string OriginalText { get; set; }

        public Statement AttachedStatement { get; set; }

        public string Respond()
        {
            Statement current = AttachedStatement;

            while (current.RelatedId != 0)
            {
                current = Statement.Find(current.RelatedId);
            }

            if (current.ResponseText != null)
            {
                return current.ResponseText;
            }
            if (current.ScriptName != null)
            {
                return instance.RunScript(current.ScriptName);
            }
            return "I don't have a response for that";
        }
    }
}
