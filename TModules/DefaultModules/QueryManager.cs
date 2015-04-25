using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModules.Core;

namespace TModules.DefaultModules
{
    public class QueryManager : TModule
    {
        private PlatformManager _platformManager;

        public QueryManager(ModuleManager host) 
            : base("Query", host)
        {
            
        }

        public override void Initialize()
        {
            base.Initialize();

            _platformManager = new PlatformManager();
        }
    }
}
