using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
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

        private Dictionary<ObjectId, TodoTask> _allTasks = new Dictionary<ObjectId, TodoTask>();

        private IMongoCollection<TodoTask> _collection;

        public TaskModule(ModuleManager manager)
            : base("Tasks", manager)
        {
            Intents.Add("reminder", WitReminder);
        }

        public override void Initialize()
        {
            base.Initialize();

            BsonClassMap.RegisterClassMap<TodoTask>(map =>
            {
                map.AutoMap();
            });

            Profiler.SharedInstance.StartProfiling("task_mongo");

            // load all of them from the database
            _collection = _database.GetCollection<TodoTask>("tasks");
            IAsyncCursor<TodoTask> t = _collection.FindAsync(x => true).GetAwaiter().GetResult();
            t.ForEachAsync(task => _allTasks.Add(task.Id, task)).Wait();

            TConsole.DebugFormat("{0} documents were loaded...", _allTasks.Count);

            var ts = Profiler.SharedInstance.GetTimeForKey("task_mongo");
            TConsole.Info("Mongo Task Load Time: " + Profiler.SharedInstance.FormattedTime(ts));

            StartChecking();
        }

        private void StartChecking()
        {
            Task t = Task.Run(() =>
            {
                while (true)
                {
                    TConsole.Info("Checking for tasks");

                    var filtered = _allTasks.Where(x => x.Value.Due < DateTime.Now);
                    List<ObjectId> tmp = new List<ObjectId>();

                    foreach (var task in filtered)
                    {
                        if (task.Value.Title != null)
                            Host.SpeakEventually("I'm supposed to remind you to " + task.Value.Title);

                        tmp.Add(task.Value.Id);
                    }

                    foreach (var objectId in tmp)
                    {
                        _collection.DeleteOneAsync(x => x.Id == objectId).Wait();
                        _allTasks.Remove(objectId);
                    }

                    //* 60
                    Task.Delay(1000 * 60).Wait();
                }
            });
        }

        private void WitReminder(WitOutcome outcome)
        {
            WitEntity reminder = null;
            WitEntity datetime = null;

            foreach (var entity in outcome.Entities)
            {
                // we are looking for the reminder and datetime entities
                if (entity.Key == "reminder")
                {
                    reminder = entity.Value.FirstOrDefault();
                }

                if (entity.Key == "datetime")
                {
                    // we don't care about other dates. Just pick the first one
                    datetime = entity.Value.FirstOrDefault();
                }
            }

            DateTime due = DateTime.Parse(datetime.GetValue("value").ToString());
            string task = reminder.GetValue("value").ToString();

            TodoTask t = new TodoTask(task, due);

            _collection.InsertOneAsync(t).Wait();
            _allTasks.Add(t.Id, t);
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
                    //if (pair.Value.Type == "one_time" && !pair.Value.Done)
                    //{
                    //    Host.SpeakEventually("Marking " + pair.Value.Title + " complete");
                    //    pair.Value.Done = true;
                    //}

                }
            }
        }

        private void MakeReoccurringTask(Match message)
        {
            string time = message.Groups[1].Value;
            string action = message.Groups[2].Value;

            Host.SpeakEventually("I will remind you to " + action + " in " + time);
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
