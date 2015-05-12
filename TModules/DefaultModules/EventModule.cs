using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModules.Core;
using System.Text.RegularExpressions;
using LitJson;
using TModules.DefaultModules.Events;

namespace TModules.DefaultModules
{
    public class EventModule : TModule
    {
        private Dictionary<string, Dictionary<string, Event>> _allEvents = new Dictionary<string, Dictionary<string, Event>>();

        public EventModule()
            : base("Events")
        {
            AddCallback("I just (.*)", EventHappened);
            AddCallback("Count of events with the phrase (.*)", CountEvents);
            AddCallback("How many times did (.*) happen?", CountEvents);
        }

        private void EventHappened(Match message)
        {
            string name = message.Groups[1].Value;

            Dictionary<string, Event> result;
            Event e;
            if (_allEvents.TryGetValue(name, out result))
            {
                var pair = result.First();
                e = (Event)pair.Value;
                e.Count += 1;
            }
            else
            {
                e = new Event(name, 1);
            }
            e.Instances.Add(new EventInstance(DateTime.Now));
        }

        private void CountEvents(Match message)
        {
            string searchTerm = message.Groups[1].Value;
            List<Event> matchedEvents = new List<Event>();

            var keys = _allEvents.Keys;
            foreach (string s in keys)
            {
                Match m = Regex.Match(s, searchTerm);
                if (m.Success)
                {
                    Dictionary<string, Event> result;
                    _allEvents.TryGetValue(s, out result);
                    var pair = result.First();
                    matchedEvents.Add(pair.Value);
                }
            }

            if (matchedEvents.Count > 0)
            {
                foreach (Event e in matchedEvents)
                {
                    string endingPhrase = "";
                    if (e.Count > 1)
                    {
                        endingPhrase = " times.";
                    }
                    else
                    {
                        endingPhrase = " time.";
                    }
                    Host.SpeakEventually(e.Name + " has happened " + e.Count + endingPhrase);
                }
            }
            else
            {
                Host.SpeakEventually("No matching events were found.");
            }
        }
    }
}
