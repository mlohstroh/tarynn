using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Analytics;
using MongoDB.Bson;
using MongoDB.Driver;
using SpotiFire;
using TModules.Core;
using TModules.DefaultModules;
using System.IO;
using TModules.Users;
using System.Diagnostics;
using LitJson;
using RestSharp;
using WitAI;

namespace TModules.Core
{
    public class ModuleManager
    {
        private Dictionary<string, TModule> _registeredModules = new Dictionary<string, TModule>();

        private SpeechHandler mSpeechHandler = new SpeechHandler();

        private List<string> _speechString = new List<string>();

        private EmbeddedServer _server = new EmbeddedServer();

        private MongoClient _client = new MongoClient();

        private Wit _wit = null;

        public ModuleManager()
        {
            _wit = new Wit(RetrieveCachedFile("wit_api"));
            TConsole.Info("WitAI Library is initialized");

            RegisterModule(new ConfigModule(this));
            //RegisterModule(new SpotifyModule(this));
            RegisterModule(new TaskModule(this));
            RegisterModule(new UtilityModule(this));
            RegisterModule(new EventModule(this));
            RegisterModule(new ProxyModule(this));

            InitModules();

            _server.Start();
            _server.Run();
        }

        public string RespondTo(string message)
        {
            Profiler.SharedInstance.StartProfiling("wit");

            WitResponse response = _wit.Query(message);

            Console.WriteLine(response.RawContent);

            TimeSpan span = Profiler.SharedInstance.GetTimeForKey("wit");

            TConsole.Info("Wit Web Reponse Time: " + Profiler.SharedInstance.FormattedTime(span));

            if (response.Outcomes.Count > 0)
            {
                WitOutcome outcome = response.Outcomes.First();

                Profiler.SharedInstance.StartProfiling("responding");

                foreach (var kvp in _registeredModules)
                {
                    // don't process anymore if someone wants to respond
                    if (kvp.Value.RespondTo(outcome))
                        break;
                }

                span = Profiler.SharedInstance.GetTimeForKey("responding");

                TConsole.Info("Module Reponse Time: " + Profiler.SharedInstance.FormattedTime(span));
            }

            return "";
        }

        public bool RegisterModule(TModule module)
        {
            _registeredModules.Add(module.ModuleName, module);
            module.SetDatabase(_client.GetDatabase(module.ModuleName));

            TConsole.InfoFormat("Registered Module: {0}", module.ModuleName);
            return true;
        }

        public void InitModules()
        {
            foreach (var kvp in _registeredModules)
            {
                kvp.Value.Initialize();
            }
        }

        #region Caching

        //EPIC GIANT HACK!!!! THIS IS AWFUL BUT I WANT IT!

        public void CacheFile(string filename, string contents)
        {
            string savePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create);
            string dirPath = Path.Combine (savePath, "Tarynn");
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            File.WriteAllText(Path.Combine(dirPath, filename), contents);
        }

        public string RetrieveCachedFile(string filename)
        {
            string savePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create);
            string path = Path.Combine (Path.Combine (savePath, "Tarynn"), filename);
            if (File.Exists (path))
                return File.ReadAllText (path);
            else
                return "";
        }

        #endregion

        public void PushPacket(string moduleName, string jsonPacket)
        {
            JsonData speech = new JsonData(_speechString);

            JsonData packet = JsonMapper.ToObject(jsonPacket);
            packet["speech"] = speech;

            _speechString.Clear();
        }

        public void SpeakEventually(string message)
        {
            _speechString.Add(message);
            mSpeechHandler.AddMessageToQueue(message);
        }

        /// <summary>
        /// Interrupt any speech. Eventually have a permission layer on here
        /// </summary>
        public void InterruptSpeech()
        {
            mSpeechHandler.StopSpeaking();
        }

        /// <summary>
        /// Returns a module by the specified name
        /// </summary>
        /// <param name="name">The name of the module. The compare will be case insensitive</param>
        /// <returns>The module if it exists. Null if it doesn't</returns>
        public TModule GetModuleByName(string name)
        {
            TModule retModule;

            _registeredModules.TryGetValue(name, out retModule);

            return retModule;
        }

        /// <summary>
        /// Returns a module by the given type
        /// </summary>
        /// <typeparam name="T">The type of module that will be returned.</typeparam>
        /// <returns>The module if it exists. Null if it doesn't</returns>
        public T GetModule<T>() where T : TModule
        {
            foreach (var m in _registeredModules)
            {
                if (m.Value is T)
                    return (T)m.Value;
            }
            return null;
        }
    }
}
