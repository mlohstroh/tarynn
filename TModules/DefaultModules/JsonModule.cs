using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModules.Core;
using System.Text.RegularExpressions;

namespace TModules.DefaultModules
{
    class JsonModule : TModule
    {
        private StorageModule storage;

        public JsonModule(ModuleManager host)
            : base("JSON", host)
        {
            storage = (StorageModule)host.GetModuleByName("Storage");

            AddCallback("list the keys for last data", ListKeys);
        }

        private void ListKeys(Match message)
        {

        }
    }
}
