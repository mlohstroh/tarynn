using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModules.Core;
using WitAI;

namespace TModules
{
    public class PlatformManager
    {
        private Dictionary<string, Platform> _platforms = new Dictionary<string, Platform>();
        public ModuleManager Host { get; private set; }

        public PlatformManager(ModuleManager host)
        {
            Host = host;
        }

        public void RegisterPlatform(Platform p)
        {
            if (_platforms.ContainsKey(p.Name))
            {
                throw new DuplicatePlatformException();
            }
            else
            {
                _platforms.Add(p.Name, p);
            }
        }

        public string Respond(string platform, string resource, WitOutcome outcome)
        {
            Platform p;
            if (_platforms.TryGetValue(platform, out p))
            {
                return p.Respond(resource, outcome);
            }
            return null;
        }
    }
}
