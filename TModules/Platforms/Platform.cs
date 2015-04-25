using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WitAI;

namespace TModules
{
    public class Platform
    {
        private Dictionary<string, ResourceResponder> _responders = new Dictionary<string, ResourceResponder>();
        public PlatformManager Manager { get; set; }

        public string Name { get; protected set; }

        public Platform(string name, PlatformManager manager)
        {
            Name = name;
            Manager = manager;
        }

        public void AddResponder(string resource, ResourceResponder responder)
        {
            if (_responders.ContainsKey(resource))
            {
                throw new DuplicateResourceResponderException();
            }
            else
            {
                _responders.Add(resource, responder);
                responder.SetPlatform(this);
                responder.Initialize();
            }
        }

        public virtual string Respond(string resource, WitOutcome outcome)
        {
            ResourceResponder responder;
            if (_responders.TryGetValue(resource, out responder))
            {
                return responder.Respond(outcome);
            }
            return null;
        }
    }
}
