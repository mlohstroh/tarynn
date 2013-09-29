using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TModules
{
    public class ModuleManager
    {
        private List<TModule> registeredModules = new List<TModule>();

        public ModuleManager()
        {

        }

        public bool RegisterModule(TModule module)
        {
            registeredModules.Add(module);
            return true;
        }
    }
}
