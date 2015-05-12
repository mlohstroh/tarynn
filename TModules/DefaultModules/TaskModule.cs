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
        private Dictionary<ObjectId, Reminder> _allReminders = new Dictionary<ObjectId, Reminder>();
        private Dictionary<ObjectId, TaskList> _allLists = new Dictionary<ObjectId, TaskList>();

        private IMongoCollection<Reminder> _reminderCollection;
        private IMongoCollection<TaskList> _tasksCollection;

        public TaskModule(ModuleManager manager)
            : base("Tasks", manager)
        {
            Intents.Add("reminder", WitReminder);
            Intents.Add("add_to_list", AddItemToList);
            Intents.Add ("display_list", DisplayList);
            Intents.Add ("remove_from_list", RemoveFromList);
        }

        public override void Initialize()
        {
            base.Initialize();

            BsonClassMap.RegisterClassMap<Reminder>(map =>
            {
                map.AutoMap();
            });

            BsonClassMap.RegisterClassMap<TaskList> (map =>
            {
                map.AutoMap();
            });

            using (Profiler.SharedInstance.ProfileBlock ("Reminder Mongo Load"))
            {
                // load all of them from the database
                _reminderCollection = _database.GetCollection<Reminder> ("reminders");
                IAsyncCursor<Reminder> t = _reminderCollection.FindAsync (x => true).GetAwaiter ().GetResult ();
                t.ForEachAsync (task => _allReminders.Add (task.Id, task)).Wait ();

                _tasksCollection = _database.GetCollection<TaskList> ("tasks");
                IAsyncCursor<TaskList> list = _tasksCollection.FindAsync (x => true).GetAwaiter ().GetResult ();
                list.ForEachAsync (task => _allLists.Add (task.Id, task)).Wait ();

                _logger.DebugFormat ("{0} documents were loaded...", _allReminders.Count + _allLists.Count);
            }
            StartChecking();
        }

        private void StartChecking()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    _logger.Info("Checking for reminders");

                    var filtered = _allReminders.Where(x => x.Value.Due < DateTime.Now);
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
                        _reminderCollection.DeleteOneAsync(x => x.Id == objectId).Wait();
                        _allReminders.Remove(objectId);
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

                Reminder t = new Reminder(task, due);

                Host.SpeakEventually ("Ok, I've added that to your task list");
                _reminderCollection.InsertOneAsync(t).Wait();
                _allReminders.Add(t.Id, t);
            }
            else
            {
                Fail();
            }
        }

        private TaskList GetListForTitle(string title)
        {
            return _allLists.Where (x => string.Compare (x.Value.ListTitle, title, true) == 0).FirstOrDefault().Value;
        }

        private void AddItemToList(WitOutcome outcome)
        {
            WitEntity list_item = outcome.GetFirstEntityOfType("list_item");
            WitEntity todolist = outcome.GetFirstEntityOfType("todolist");

            if (list_item != null && todolist != null)
            {
                string listName = todolist.GetValue ("value").ToString ();
                string itemName = list_item.GetValue ("value").ToString ();

                TaskList foundList = GetListForTitle (listName);
                if (foundList != null)
                {
                    foundList.Items.Add (itemName);
                    _tasksCollection.ReplaceOneAsync (x => x.Id == foundList.Id, foundList);
                } 
                else
                {
                    foundList = new TaskList (listName, new List<string> () { itemName });
                    _tasksCollection.InsertOneAsync (foundList);
                }

                Host.SpeakEventually (string.Format("Ok, I've added {0} to your list", itemName));
            } 
            else
            {
                Fail ();
            }

        }

        private void DisplayList(WitOutcome outcome)
        {
            WitEntity witList = outcome.GetFirstEntityOfType ("todolist");
            string listName = "things to do";

            if (witList != null)
            {
                listName = witList.GetValue ("value").ToString();
            }

            TaskList list = GetListForTitle (listName);
            if (list != null)
            {
                Host.SpeakEventually ("Here are the things on your list");
                foreach (string item in list.Items)
                {
                    Host.SpeakEventually (item);
                }
            }
            else
            {
                Host.SpeakEventually ("No such list exists. You might want to make one.");
            }
        }

        private void RemoveFromList(WitOutcome outcome)
        {
            WitEntity witList = outcome.GetFirstEntityOfType ("todolist");
            string listName = "things to do";

            if (witList != null)
            {
                listName = witList.GetValue ("value").ToString();
            }

            TaskList list = GetListForTitle (listName);
            WitEntity witItem = outcome.GetFirstEntityOfType ("list_item");
            if (witItem != null && list != null)
            {
                string fuzzyValue = witItem.GetValue ("value").ToString();
                int count = list.Items.RemoveAll (x => x.Contains (fuzzyValue));
                if (count > 0)
                {
                    Host.SpeakEventually(string.Format("I removed {0} items from your list", count));
                    _tasksCollection.ReplaceOneAsync (x => x.Id == list.Id, list);
                }
                else
                {
                    Host.SpeakEventually ("Nothing matched your search query...");
                }
            } 
            else
            {
                Host.SpeakEventually ("You need to specify something to remove");    
            }
        }
    }
}
