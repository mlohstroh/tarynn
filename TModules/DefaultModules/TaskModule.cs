using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModules.Core;
using System.Text.RegularExpressions;
using SharpCouch;
using TModules.DefaultModules.Tasks;
using LitJson;

namespace TModules.DefaultModules
{
    public class TaskModule : TModule
    {
        public DB couch = new DB();

        const string SERVER_ADDRESS = "http://127.0.0.1:5984";
        const string DB_NAME = "tarynn-tasks";

        private List<TodoTask> _allTasks = new List<TodoTask>();

        public TaskModule(ModuleManager manager)
            : base("Tasks", manager)
        {
            if (!DBExists(DB_NAME))
            {
                couch.CreateDatabase(SERVER_ADDRESS, DB_NAME);
            }

            DocInfo[] docs = couch.GetAllDocuments(SERVER_ADDRESS, DB_NAME);

            LoadDocs(docs);

            //most of these regexes were taken from here
            //https://github.com/github/hubot-scripts/blob/master/src/scripts/remind.coffee
            _allCallbacks.Add("remind me in ((?:(?:\\d+) (?:weeks?|days?|hours?|hrs?|minutes?|mins?|seconds?|secs?)[ ,]*(?:and)? +)+)to (.*)", Remind);
        }

        private void LoadDocs(DocInfo[] docs)
        {
            foreach (DocInfo doc in docs)
            {
                string json = couch.GetDocument(SERVER_ADDRESS, DB_NAME, doc.ID);
                JsonData data = JsonMapper.ToObject(json);
                _allTasks.Add(TodoTask.BuildTask(data));
            }
        }

        private void Remind(Match message)
        {
            string time = message.Groups[1].Value;
            string action = message.Groups[2].Value;

            OneTimeTodoTask t = new OneTimeTodoTask();
            t.Deadline = new DateTime(DateTime.UtcNow.Ticks + 1000);
            t.Title = action;
            t.Type = "one_time";
            couch.CreateDocument(SERVER_ADDRESS, DB_NAME, JsonMapper.ToJson(t));
        }

        private bool DBExists(string name)
        {
            string[] dbNames = couch.GetDatabases(SERVER_ADDRESS);
            foreach (string s in dbNames)
            {
                if (s == name)
                    return true;
            }
            return false;
        }
    }
}
