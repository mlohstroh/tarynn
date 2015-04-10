using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Analytics;
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
        private List<TModule> registeredModules = new List<TModule>();

        private SpeechHandler mSpeechHandler = new SpeechHandler();

        private List<string> _speechString = new List<string>();
        EmbeddedServer _server = new EmbeddedServer();

        private Wit _wit = null;

        public ModuleManager()
        {
            _wit = new Wit(RetrieveCachedFile("wit_api"));
            TConsole.Info("WitAI Library is initialized");

            PacketManager p = new PacketManager();

            RegisterModule(new ConfigModule(this));
            RegisterModule(new StorageModule(this));
            //RegisterModule(new SpotifyModule(this));
            //RegisterModule(new TaskModule(this));
            RegisterModule(new UtilityModule(this));
            RegisterModule(new EventModule(this));
            RegisterModule(new UserManagement(this));
            RegisterModule(new JsonModule(this));
            RegisterModule(new ProxyModule(this));

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

            /*
             * Hack to disable below code for now. All of the default modules need to be converted 
             * to deal with wit.ai.
             */

            return "";

            Profiler.SharedInstance.StartProfiling("responding");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            foreach (TModule module in registeredModules)
            {
                if (module.RespondTo(message))
                {
                    break;
                }
            }

            span = Profiler.SharedInstance.GetTimeForKey("responding");

            TConsole.Info("Module Reponse Time: " + Profiler.SharedInstance.FormattedTime(span));

            return "";
        }

        public bool RegisterModule(TModule module)
        {
            registeredModules.Add(module);
            TConsole.InfoFormat("Registered Module: {0}", module.ModuleName);
            return true;
        }

        #region Caching

        //EPIC GIANT HACK!!!! THIS IS AWFUL BUT I WANT IT!

        public void CacheFile(string filename, string contents)
        {
            string savePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create);
            if (!Directory.Exists(savePath + "\\Tarynn"))
                Directory.CreateDirectory(savePath + "\\Tarynn");
            File.WriteAllText(savePath + "\\Tarynn\\" + filename, contents);
        }

        public string RetrieveCachedFile(string filename)
        {
            string savePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create);
            return File.ReadAllText(savePath + "\\Tarynn\\" + filename);
        }

        #endregion

        public void PushPacket(string moduleName, string jsonPacket)
        {
            JsonData speech = new JsonData(_speechString);

            JsonData packet = JsonMapper.ToObject(jsonPacket);
            packet["speech"] = speech;

            PacketManager.SharedInstance.PushPacket(moduleName, packet);
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
            foreach (TModule m in registeredModules)
            {
                if (m.ModuleName.ToLower() == name.ToLower())
                    return m;
            }

            return null;
        }

        /// <summary>
        /// Returns a module by the given type
        /// </summary>
        /// <typeparam name="T">The type of module that will be returned.</typeparam>
        /// <returns>The module if it exists. Null if it doesn't</returns>
        public T GetModule<T>() where T : TModule
        {
            foreach (var m in registeredModules)
            {
                if (m is T)
                    return (T)m;
            }
            return null;
        }
    }
}
