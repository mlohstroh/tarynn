using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModules.Core;
using System.Text.RegularExpressions;
using SharpCouch;
using LitJson;
using TModules.DefaultModules.Events;

namespace TModules.DefaultModules.Tasks
{
    public class EventModule : TModule
    {
        private DB _couch = new DB();

        const string SERVER_ADDRESS = "http://127.0.0.1:5984";
        const string DB_NAME = "tarynn-events";

        private Dictionary<string, Dictionary<string, Event>> _allEvents = new Dictionary<string, Dictionary<string, Event>>();

        public EventModule(ModuleManager manager)
            : base("Events", manager)
        {
            InitDatabase();

            AddCallback("I just (.*)", EventHappened);
            AddCallback("Count of events with the phrase (.*)", CountEvents);
        }

        private void InitDatabase()
        {
            if (!DBExists(DB_NAME))
            {
                _couch.CreateDatabase(SERVER_ADDRESS, DB_NAME);
            }

            LoadDocs();    
        }

        private void LoadDocs()
        {
            DocInfo[] docs = _couch.GetAllDocuments(SERVER_ADDRESS, DB_NAME);

            foreach (DocInfo doc in docs)
            {
                string json = _couch.GetDocument(SERVER_ADDRESS, DB_NAME, doc.ID);
                JsonData data = JsonMapper.ToObject(json);

                Event e = new Event(data["Name"].ToString(), int.Parse(data["Count"].ToString()));
                Dictionary<string, Event> eventHash = new Dictionary<string, Event>();
                eventHash.Add(doc.ID, e);

                _allEvents.Add(data["Name"].ToString(), eventHash);
            }
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
                _couch.DeleteDocument(SERVER_ADDRESS, DB_NAME, pair.Key);
            }
            else
            {
                e = new Event(name, 1);
            }
            _couch.CreateDocument(SERVER_ADDRESS, DB_NAME, JsonMapper.ToJson(e));

            LoadDocs();
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
                    Host.SpeakEventually(e.Name + " has happened " + e.Count + " times");
                }
            }
            else
            {
                Host.SpeakEventually("No matching events were found");
            }
        }

        private bool DBExists(string name)
        {
            string[] dbNames = _couch.GetDatabases(SERVER_ADDRESS);
            foreach (string s in dbNames)
            {
                if (s == name)
                    return true;
            }
            return false;
        }
    }
}
