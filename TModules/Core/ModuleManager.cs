using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Diagnostics;
using LitJson;
using RestSharp;
using WitAI;
using TRouter;

namespace TModules.Core
{
    public class ModuleManager
    {
        private TConsole _logger = new TConsole (typeof(ModuleManager));
        private Dictionary<string, TModule> _registeredModules = new Dictionary<string, TModule>();

        private SpeechHandler mSpeechHandler = new SpeechHandler();

        private EmbeddedServer _server = new EmbeddedServer();

        public Router Router = new Router();

        private MongoClient _client = new MongoClient();

        private PlatformManager _platformManager;

        private Wit _wit = null;

        public ModuleManager()
        {
            CheckForMongo();

            _wit = new Wit(RetrieveCachedFile("wit_api"));
            _logger.Info("WitAI Library is initialized");   

             _platformManager = new PlatformManager(this);

            RegisterModule(new ConfigModule(this));
            #if !__MonoCS__
            RegisterModule(new SpotifyModule(this));
            #endif
            RegisterModule(new TaskModule(this));
            RegisterModule(new UtilityModule(this));
            RegisterModule(new EventModule(this));
            RegisterModule(new ProxyModule(this));
            RegisterModule(new QueryManager(this));
            RegisterModule(new AlarmModule(this));

            InitModules();


            _server.Start(Router);
            _server.Run();
        }

        private void CheckForMongo()
        {
            // This will only work in development with zero credentials
            RestClient dummy = new RestClient("http://localhost:27017/");
            IRestRequest req = new RestRequest("/");
            var res = dummy.Get(req);
            if (res.StatusCode != HttpStatusCode.OK)
            {
                _logger.ErrorFormat("Mongo is not running! Tarynn will not function without mongodb");
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                Environment.Exit(1);
            }
            else
            {
                _logger.InfoFormat("Mongodb is up and running");
            }
        }

        public string RespondTo(string message)
        {
            try
            {
                WitResponse response;
                using (Profiler.SharedInstance.ProfileBlock ("Wit Response Time"))
                {
                    response = _wit.Query(message);
                    
                    Console.WriteLine(response.RawContent);
                }

                if (response.Outcomes.Count > 0)
                {
                    WitOutcome outcome = response.Outcomes.First();

                    using(Profiler.SharedInstance.ProfileBlock("Module Response Time"))
                    {
                        foreach (var kvp in _registeredModules)
                        {
                            // don't process anymore if someone wants to respond
                            if (kvp.Value.RespondTo(outcome))
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("Error executing query {0}. Exception {1}", message, ex);   
            }

            return "";
        }

        public bool RegisterModule(TModule module)
        {
            _registeredModules.Add(module.ModuleName, module);
            module.SetDatabase(_client.GetDatabase(module.ModuleName));

            _logger.InfoFormat("Registered Module: {0}", module.ModuleName);
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

        public string RetrieveCachedFile(string filename, string def = "")
        {
            string savePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create);
            string path = Path.Combine (Path.Combine (savePath, "Tarynn"), filename);
            if (File.Exists(path))
                return File.ReadAllText(path);

            if (string.IsNullOrEmpty(def))
            {
                _logger.InfoFormat(
                    "Please Enter the value for {0}. If this value is incorrect, Tarynn might not work correctly.",
                    filename);
                Console.Write("Value for {0}: ", filename);
                string val = Console.ReadLine();
                CacheFile (filename, val);
                return val;
            }
            else
                return def;
        }

        #endregion

        public void SpeakEventually(string message)
        {
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
        /// Rudly interrupts anything talking and blocks whatever thread is speaking
        /// </summary>
        /// <param name="message">The message for the handler to speak</param>
        public void BlockingSpeak(string message)
        {
            mSpeechHandler.SpeakNowAndBlock(message);
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
