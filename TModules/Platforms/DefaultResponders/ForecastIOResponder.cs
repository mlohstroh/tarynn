using System;
using System.Diagnostics;
using System.Linq;
using ForecastIO;
using Matrix.Xmpp.XHtmlIM;
using WitAI;
using Analytics;

namespace TModules
{
    class ForecastIOResponder : ResourceResponder
    {
        public override string Resource
        {
            get { return "weather"; }
        }

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
            TConsole.InfoFormat("Time for Forecast: {0}", time.ToString("R"));

            ForecastIORequest req = new ForecastIORequest(_apiKey, 32.7767f, -96.8134f, time, Unit.us);
            var res = req.Get();
            var daily = res.daily.data.FirstOrDefault();

            Debug.Assert(daily != null, "daily != null");
            return daily.summary;
        }
    }
}
