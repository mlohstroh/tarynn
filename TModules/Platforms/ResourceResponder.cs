using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WitAI;

namespace TModules
{
    public class ResourceResponder
    {
        public virtual string PlatformName { get { return "generic"; } }

        /// <summary>
        /// This needs to be implemented, otherwise exceptions will be thrown
        /// </summary>
        public virtual string Resource { get { return null; } }

        public Platform Platform { get; private set; }

        public void SetPlatform(Platform p)
        {
            Platform = p;
        }

        public virtual void Initialize()
        { }

        public virtual string Respond(WitOutcome outcome)
        {
            return null;
        }
    }
}
