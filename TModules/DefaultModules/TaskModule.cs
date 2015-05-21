using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using RestSharp;
using TModules.Core;
using System.Text.RegularExpressions;
using TModules.DefaultModules.Tasks;
using LitJson;
using Analytics;
using TRouter;
using WitAI;
using Route = TRouter.Route;

namespace TModules.DefaultModules
{
    public class TaskModule : TModule
    {
        private Dictionary<ObjectId, TodoTask> _allTasks = new Dictionary<ObjectId, TodoTask>();

        private IMongoCollection<TodoTask> _collection;

        public TaskModule(ModuleManager manager)
            : base("Tasks", manager)
        {
            Intents.Add("reminder", WitReminder);

            Get("/test", (request, response) =>
            {
                response.ResponseBody = "I somehow got this working!";
            });

            Post("/test", (request, response) =>
            {
                response.ResponseBody = request.RawBody;
            });
        }

        public override void Initialize()
        {
            base.Initialize();

            BsonClassMap.RegisterClassMap<TodoTask>(map =>
            {
                map.AutoMap();
            });

            using (Profiler.SharedInstance.ProfileBlock ("Task Mongo Load"))
            {
                // load all of them from the database
                _collection = _database.GetCollection<TodoTask> ("tasks");
                IAsyncCursor<TodoTask> t = _collection.FindAsync (x => true).GetAwaiter ().GetResult ();
                t.ForEachAsync (task => _allTasks.Add (task.Id, task)).Wait ();

                _logger.DebugFormat ("{0} documents were loaded...", _allTasks.Count);
            }
            StartChecking();
        }

        private void StartChecking()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    _logger.Info("Checking for tasks");

                    var filtered = _allTasks.Where(x => x.Value.Due < DateTime.Now);
                    List<ObjectId> tmp = new List<ObjectId>();

                    foreach (var task in filtered)
                    {
                        if (task.Value.Title != null)
                        {
                            Host.SpeakEventually("I'm supposed to remind you about something.");
                            Host.SpeakEventually(task.Value.Title);
                        }

                        tmp.Add(task.Value.Id);
                    }

                    foreach (var objectId in tmp)
                    {
                        _collection.DeleteOneAsync(x => x.Id == objectId).Wait();
                        _allTasks.Remove(objectId);
                    }

                    //delay is in miliseconds
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
            if (reminder != null && datetime != null)
            {
                DateTime due = DateTime.Parse(datetime.GetValue("value").ToString());
                string task = reminder.GetValue("value").ToString();

                TodoTask t = new TodoTask(task, due);

                Host.SpeakEventually ("Ok, I've added that to your task list");
                _collection.InsertOneAsync(t).Wait();
                _allTasks.Add(t.Id, t);
            }
            else
            {
                Fail();
            }
        }
    }
}
