using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TModules
{
    public abstract class TModule
    {
        public delegate void Heard(string message);

        private Dictionary<string, List<Heard>> _allCallbacks = new Dictionary<string,List<Heard>>();

        public string ModuleName { get; private set; }

        public TModule(string name)
        {

        }
    }
}
