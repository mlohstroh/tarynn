using System;
using System.Linq;
using ForecastIO;
using Matrix.Xmpp.XHtmlIM;
using WitAI;

namespace TModules.Platforms.DefaultResponders
{
    class ForecastIOResponder : ResourceResponder
    {
        private string _apiKey;
        
        public override void Initialize()
        {
            _apiKey = Platform.Manager.Host.RetrieveCachedFile("forecast_io_api");
        }

        public override string Respond(WitOutcome outcome)
        {
            DateTime time = DateTime.Now;

            if (outcome.Entities.ContainsKey("datetime"))
            {
                var dateEnt = outcome.Entities["datetime"];
                var firstOrDefault = dateEnt.FirstOrDefault();
                if (firstOrDefault != null)
                    time = DateTime.Parse(firstOrDefault.GetValue("value").ToString());
            }

            // dallas 33.0347,-96.8134

            ForecastIORequest req = new ForecastIORequest(_apiKey, 32.7767f, -96.8134f, time, Unit.us);
            var res = req.Get();
            var daily = res.daily.data.FirstOrDefault();

            return daily.summary;
        }
    }
}
