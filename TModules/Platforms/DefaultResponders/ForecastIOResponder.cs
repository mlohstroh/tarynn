using System;
using System.Linq;
using ForecastIO;
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
            WitDateTimeGrain grain = WitDateTimeGrain.Day;

            if (outcome.Entities.ContainsKey("datetime"))
            {
                var dateEnt = outcome.Entities["datetime"];
                var firstOrDefault = dateEnt.FirstOrDefault();
                if (firstOrDefault != null)
                {
                    time = DateTime.Parse(firstOrDefault.GetValue("value").ToString());
                    string grainVal = firstOrDefault.GetValue("grain").ToString();
                    Enum.TryParse(grainVal, true, out grain);
                }
            }

            // dallas 33.0347,-96.8134
            TConsole.InfoFormat("Time for Forecast: {0}", time.ToString("R"));

            ForecastIORequest req = new ForecastIORequest(_apiKey, 32.7767f, -96.8134f, time, Unit.us);
            var res = req.Get();

            switch (grain)
            {
                case WitDateTimeGrain.Second:
                case WitDateTimeGrain.Minute:
                    return res.currently.summary;
                case WitDateTimeGrain.Hour:
                {
                    HourForecast closestForecast = res.hourly.data.FirstOrDefault();
                    long sinceEpoch = time.SinceEpoch();
                    foreach (var forecast in res.hourly.data)
                    {
                        long timeDiff = forecast.time - sinceEpoch;
                        long closestDiff = closestForecast.time - sinceEpoch;
                        if (timeDiff < closestDiff)
                            closestForecast = forecast;
                    }
                    
                    return HourlyForecast(closestForecast);
                }
                case WitDateTimeGrain.Day:
                    return DailyForecast(res.daily.data.FirstOrDefault());
            }
            return null;
        }

        private string DailyForecast(DailyForecast forecast)
        {
            TConsole.InfoFormat("Daily Forecast {0}", forecast.summary);
            return string.Format("{0}, with a high of {1} and a low of {2}", forecast.summary, forecast.temperatureMax, forecast.temperatureMin);
        }

        private string HourlyForecast(HourForecast forecast)
        {
            TConsole.InfoFormat("Hourly Forecast {0}", forecast.summary);
            return string.Format("{0}, with a temperature of {1}", forecast.summary, forecast.temperature);
        }
    }
}
