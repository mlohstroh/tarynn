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

        const string TIME_REGEX = "((?:(?:\\d+) (?:weeks?|days?|hours?|minutes?|seconds?)))";
        const string BROKEN_TIME_REGEX = "(?:(?:(\\d+)) ((?:weeks?|days?|hours?|hrs?|minutes?|mins?|seconds?|secs?)))";

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
            AddCallback("remind me in ((?:(?:\\d+) (?:weeks?|days?|hours?|hrs?|minutes?|mins?|seconds?|secs?)[ ,]*(?:and)? +)+)to (.*)", Remind);
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
            t.Deadline = BuildTime(time);
            t.Title = action;
            t.Type = "one_time";

            Host.SpeakEventually("What would you rate this? None, one, two, or three?");
            AddFollowup("(none|one|two|three|\\d)", (Match followUpMessage) =>
            {
                switch (followUpMessage.Groups[1].Value)
                {
                    case "none":
                        t.Size = 0;
                        break;
                    case "one":
                        t.Size = 1;
                        break;
                    case "two":
                        t.Size = 2;
                        break;
                    case "three":
                        t.Size = 3;
                        break;
                    default:
                        t.Size = int.Parse(followUpMessage.Groups[1].Value);
                        break;
                }

                couch.CreateDocument(SERVER_ADDRESS, DB_NAME, JsonMapper.ToJson(t));
                Host.SpeakEventually("I will remind you to " + action + " in " + time);
            });
        }

        private DateTime BuildTime(string time)
        {
            DateTime datetime = DateTime.Now;
            Match regex = Regex.Match(time, TIME_REGEX);

            for(int i = 1; i < regex.Groups.Count; i++)   
            {
                string currentTime = regex.Groups[i].Value;

                Match smallRegex = Regex.Match(currentTime, BROKEN_TIME_REGEX);
                int duration = int.Parse(smallRegex.Groups[1].Value);
                switch (smallRegex.Groups[2].Value)
                {
                    case "weeks":
                        datetime = datetime.AddDays(7 * duration);
                        break;
                    case "days":
                        datetime = datetime.AddDays(duration);
                        break;
                    case "hours":
                        datetime = datetime.AddHours(duration);
                        break;
                    case "minutes":
                        datetime = datetime.AddMinutes(duration);
                        break;
                    case "seconds":
                        datetime = datetime.AddSeconds(duration);
                        break;
                }
            }

            return datetime;
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
