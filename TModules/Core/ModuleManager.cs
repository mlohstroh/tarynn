using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModules.Core;
using TModules.DefaultModules;
using System.IO;

namespace TModules
{
    public class ModuleManager
    {
        private List<TModule> registeredModules = new List<TModule>();

        private SpeechHandler mSpeechHandler = new SpeechHandler();

        public ModuleManager()
        {
            RegisterModule(new SpotifyModule(this));
        }

        public string RespondTo(string message)
        {
            foreach (TModule module in registeredModules)
            {
                module.RespondTo(message);
            }
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
    }
}
