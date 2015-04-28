using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analytics;
using Matrix.Xmpp.Google.Push;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using TModules.Core;
using TModules.DefaultModules;
using TModules.DefaultModules.Tasks;
using WitAI;

namespace TModules
{
    public class AlarmModule : TModule
    {
        private IMongoCollection<Alarm> _collection;
        private Dictionary<ObjectId, Alarm> _alarms = new Dictionary<ObjectId, Alarm>();

        public AlarmModule(ModuleManager host) :
            base("Alarm", host)
        {
            Intents.Add("alarm_set", AddAlarm);
        }

        public override void Initialize()
        {
            BsonClassMap.RegisterClassMap<Alarm>(map => map.AutoMap());

            using (Profiler.SharedInstance.ProfileBlock ("Alarm Mongo Load"))
            {
                // load all of them from the database
                _collection = _database.GetCollection<Alarm> ("alarm");
                IAsyncCursor<Alarm> t = _collection.FindAsync (x => true).GetAwaiter ().GetResult ();
                t.ForEachAsync (task => _alarms.Add (task.Id, task)).Wait ();

                _logger.DebugFormat ("{0} alarms were loaded...", _alarms.Count);
            }

            StartChecking();
        }

        private void StartChecking()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    _logger.Info("Checking for alarms");

                    var filtered = _alarms.Where(x => x.Value.Time < DateTime.Now);
                    List<ObjectId> tmp = new List<ObjectId>();

                    foreach (var task in filtered)
                    {
                        // wake them up by playing spotify
                        SpotifyModule mod = Host.GetModule<SpotifyModule>();
                        Host.BlockingSpeak("It's time to get up! You had me set an alarm for right now.");

                        if (mod != null)
                        {
                            mod.PlayRandomTrack();
                        }
                        tmp.Add(task.Value.Id);
                    }

                    foreach (var objectId in tmp)
                    {
                        _collection.DeleteOneAsync(x => x.Id == objectId).Wait();
                        _alarms.Remove(objectId);
                    }

                    //delay is in miliseconds
                    Task.Delay(1000 * 60).Wait();
                }
            });
        }

        private void AddAlarm(WitOutcome outcome)
        {
            WitEntity date = null;

            foreach (var entity in outcome.Entities)
            {
                if (entity.Key == "datetime")
                {
                    date = entity.Value.FirstOrDefault();
                    break;
                }
            }

            if (date != null)
            {
                DateTime alarmTime = DateTime.Parse(date.GetValue("value").ToString());

                Alarm a = new Alarm(alarmTime);
                _collection.InsertOneAsync(a).Wait();
                _alarms.Add(a.Id, a);

                Host.SpeakEventually("Ok, Alarm set!");
                _logger.InfoFormat("Alarm set for {0}", alarmTime.ToString("g"));
            }
        }
    }
}
