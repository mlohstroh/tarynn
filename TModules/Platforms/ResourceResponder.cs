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
        public string Resource { get; private set; }
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
