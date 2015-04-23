using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
#if !__MonoCS__
using System.Speech.Synthesis;
#endif
using WitAI;

namespace TModules.Core
{
    public abstract class TModule
    {
        public Dictionary<string, Action<WitOutcome>> Intents { get; protected set;  } 

        public delegate void Heard(Match message);

        private Dictionary<string, Heard> _allCallbacks = new Dictionary<string, Heard>();

        public string ModuleName { get; private set; }
        public ModuleManager Host;

        public TModule(string name, ModuleManager host)
        {
            this.Host = host;
            this.ModuleName = name;
            Intents = new Dictionary<string, Action<WitOutcome>>();
        }

        protected void AddCallback(string pattern, Heard callback)
        {
            _allCallbacks.Add(pattern, callback);
        }

        /// <summary>
        /// Called by the Manager to see if this module responds to a certain message
        /// </summary>
        /// <param name="message">The message typed in</param>
        public bool RespondTo(WitOutcome outcome)
        {
            foreach (string intent in Intents.Keys)
            {
                if (string.Compare(intent, outcome.Intent, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    // I have a callback that responds to such an intent
                    var callback = Intents[intent];
                    callback(outcome);
                    return true;
                }
            }
            
            return false;
        }
    }
}
