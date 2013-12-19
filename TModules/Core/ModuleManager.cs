using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModules.Core;
using TModules.DefaultModules;
using System.IO;
using TModules.Users;
using System.Diagnostics;
using LitJson;

namespace TModules.Core
{
    public class ModuleManager
    {
        private List<TModule> registeredModules = new List<TModule>();

        private SpeechHandler mSpeechHandler = new SpeechHandler();

        private List<string> _speechString = new List<string>();

        public ModuleManager()
        {
            PacketManager p = new PacketManager();

            RegisterModule(new ConfigModule(this));
            RegisterModule(new StorageModule(this));
            //RegisterModule(new SpotifyModule(this));
            RegisterModule(new TaskModule(this));
            RegisterModule(new UtilityModule(this));
            RegisterModule(new EventModule(this));
            RegisterModule(new UserManagement(this));
        }

        public string RespondTo(string message)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            foreach (TModule module in registeredModules)
            {
                if (module.RespondTo(message))
                {
                    break;
                }
            }

            watch.Stop();
            TimeSpan ts = watch.Elapsed;

            // Format and display the TimeSpan value. 
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            Console.WriteLine("Module Reponse Time: " + elapsedTime);

            //SpeakEventually("I'm sorry, I don't know what you mean");
            return "";
        }

        public bool RegisterModule(TModule module)
        {
            registeredModules.Add(module);
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

        public TModule GetModuleByName(string name)
        {
            foreach (TModule m in registeredModules)
            {
                if (m.ModuleName == name)
                    return m;
            }

            return null;
        }
    }
}
