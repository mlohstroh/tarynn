using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TModules.Core;
using WitAI;

namespace TModules.DefaultModules
{
    public class QueryManager : TModule
    {
        private PlatformManager _platformManager;

        public QueryManager() 
            : base("Query")
        {
            Intents.Add("query", CheckForResponse);
        }

        public override void Initialize()
        {
            base.Initialize();

            _platformManager = new PlatformManager(Host);

            _platformManager.Init();
        }

        private void CheckForResponse(WitOutcome outcome)
        {
            string platform = null;
            string resource = null;

            foreach (var kvp in outcome.Entities)
            {
                if (kvp.Key == "platform")
                {
                    var ent = kvp.Value.FirstOrDefault();
                    if(ent != null)
                        platform = ent.GetValue("value").ToString();
                }
                if (kvp.Key == "resource")
                {
                    var ent = kvp.Value.FirstOrDefault();
                    if(ent != null)
                        resource = ent.GetValue("value").ToString();
                }
            }

            // check for generic
            if (string.IsNullOrEmpty(platform))
                platform = "generic";

            if (!string.IsNullOrEmpty(resource))
            {
                string response = _platformManager.Respond(platform, resource, outcome);
                if(!string.IsNullOrEmpty(response))
                    Host.SpeakEventually(response);
                else
                    Host.SpeakEventually("None of your installed resource responders were able to handle your query...");
            }
            else
                Fail();
        }
    }
}
