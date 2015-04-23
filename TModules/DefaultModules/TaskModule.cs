using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModules.Core;
using System.Text.RegularExpressions;
using TModules.DefaultModules.Tasks;
using LitJson;
using Analytics;
using WitAI;

namespace TModules.DefaultModules
{
    public class TaskModule : TModule
    {
        const string DB_NAME = "tarynn-tasks";

        private Dictionary<string, TodoTask> _allTasks = new Dictionary<string, TodoTask>();
        private Dictionary<string, TodoTask> _todaysTasks;

        public TaskModule(ModuleManager manager)
            : base("Tasks", manager)
        {
            Intents.Add("reminder", WitReminder);

            Task t = Task.Run( () =>
            {
                while (true)
                {
                    TConsole.Info("Checking for tasks");

                    var filtered = _allTasks.Where(x => x.Value.Done == false && x.Value.Type != "reoccurring").Where(x => x.Value.TimeToNotify < DateTime.Now);

                    foreach (var task in filtered)
                    {
                        if(task.Value.Title != null)
                            Host.SpeakEventually("I'm supposed to remind you to " +  task.Value.Title);
                    }

                    //* 60
                    Task.Delay(1000 * 60).Wait();
                }
            });

        }

        private void WitReminder(WitOutcome outcome)
        {
            foreach (var entity in outcome.Entities)
            {
                TConsole.InfoFormat("Key: {0}", entity.Key);

                foreach (var witEntity in entity.Value)
                {
                    TConsole.InfoFormat("Entities: {0}", witEntity._data.ToJson());
                }
            }
        }

        private void BuildReoccurringTime(string message, ref ReoccurringTask t)
        {
            
        }

        #region Callbacks

        private void MarkComplete(Match message)
        {
            string searchTerm = message.Groups[1].Value;

            foreach (var pair in _allTasks)
            {
                Match m = Regex.Match(pair.Value.Title, searchTerm);
                if (m.Success)
                {
                    if (pair.Value.Type == "one_time" && !pair.Value.Done)
                    {
                        Host.SpeakEventually("Marking " + pair.Value.Title + " complete");
                        pair.Value.Done = true;
                    }

                }
            }
        }

        private void MakeReoccurringTask(Match message)
        {
            string time = message.Groups[1].Value;
            string action = message.Groups[2].Value;

            ReoccurringTask t = new ReoccurringTask();
            BuildReoccurringTime(time, ref t);
            t.Title = action;
            t.Type = "reoccurring";

            Host.SpeakEventually("I will remind you to " + action + " in " + time);
        }

        private void DueToday(Match message)
        {
            Host.SpeakEventually(LoadToday());
        }

        public string LoadToday()
        {
            StringBuilder builder = new StringBuilder();
            int counter = 1;

            _todaysTasks = new Dictionary<string, TodoTask>();

            foreach (string s in _allTasks.Keys)
            {
                TodoTask t;
                _allTasks.TryGetValue(s, out t);
                switch (t.Type)
                {
                    case "one_time":
                        OneTimeTodoTask one = (OneTimeTodoTask)t;
                        if (DateTime.Now.DayOfYear == one.Deadline.DayOfYear && !t.Done)
                        {
                            builder.Append(counter++ + " " + one.Title + ",");
                            _todaysTasks.Add(s, one);
                        }

                        break;
                    case "reoccurring":
                        ReoccurringTask re = (ReoccurringTask)t;
                        if (re.DaysToRepeat == null)    //bad fix
                            re.DaysToRepeat = new List<int>();
                        if (re.DaysToRepeat.IndexOf((int)DateTime.Now.DayOfWeek) != -1 && !t.Done)
                        {
                            builder.Append(counter++ + " " + re.Title + ",");
                            _todaysTasks.Add(s, re);
                        }
                        break;
                }
            }
            if (builder.Length > 0)
                return builder.ToString().TrimEnd(builder.ToString()[builder.Length - 1]);
            else
                return "You have nothing for today";
        }

        private void Remind(Match message)
        {
            string time = message.Groups[1].Value;
            string action = message.Groups[2].Value;

            OneTimeTodoTask t = new OneTimeTodoTask();
            t.Title = action;
            t.Type = "one_time";
            t.Done = false;

            Host.SpeakEventually("I will remind you to " + action + " in " + time);
            //});
        }

        #endregion

        private bool TaskExists(string action)
        {
            foreach (var pair in this._allTasks)
            {
                if (pair.Value.Title == action)
                    return true;
            }
            return false;
        }
    }
}
