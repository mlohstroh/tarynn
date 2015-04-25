using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TModules
{
    public class PlatformManager
    {
        private Dictionary<string, Platform> _platforms = new Dictionary<string, Platform>();

        public PlatformManager()
        {
        }

        public void RegisterPlatform(Platform p)
        {
            if (_platforms.ContainsKey(p.Name))
            {
                
            }
            else
            {
                
            }
        }
    }
}
