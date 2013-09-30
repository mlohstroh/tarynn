using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Speech.Synthesis;

namespace TModules.Core
{
    public abstract class TModule
    {
        public delegate void Heard(Match message);

        protected Dictionary<string, Heard> _allCallbacks = new Dictionary<string, Heard>();

        public string ModuleName { get; private set; }
        public ModuleManager Host;

        private SpeechHandler mSpeechHandler = new SpeechHandler();

        public TModule(string name, ModuleManager host)
        {
            this.Host = host;
            this.ModuleName = name;
        }

        protected void AddCallback(string pattern, Heard callback)
        {
            _allCallbacks.Add(pattern, callback);
        }

        /// <summary>
        /// Called by the Manager to see if this module responds to a certain message
        /// </summary>
        /// <param name="message">The message typed in</param>
        public void RespondTo(string message)
        {
            foreach (string pattern in _allCallbacks.Keys)
            {
                Match match = Regex.Match(message, pattern, RegexOptions.IgnoreCase);
                if(match.Success)
                {
                    Heard callback;
                    _allCallbacks.TryGetValue(pattern, out callback);
                    callback(match);
                    return;
                }
            }
        }

        public void Speak(string message)
        {
            
        }
    }
}
