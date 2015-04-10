using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Analytics
{
    public class Profiler
    {
        #region Singleton

        private static Profiler instance;

        /// <summary>
        /// Singleton for a global instance of profiler
        /// </summary>
        public static Profiler SharedInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Profiler();
                }
                return instance;
            }
        }

        #endregion

        private Dictionary<string, Stopwatch> timeValues;

        /// <summary>
        /// Constructor
        /// </summary>
        public Profiler()
        {
            timeValues = new Dictionary<string, Stopwatch>();
        }

        /// <summary>
        /// Writes the time to a hash
        /// </summary>
        /// <param name="key">The key to retrieve the time from</param>
        public void StartProfiling(string key)
        {
            if (timeValues.ContainsKey(key))
                timeValues.Remove(key);

            Stopwatch watch = new Stopwatch();
            watch.Start();
            timeValues.Add(key, watch);
        }

        /// <summary>
        /// Returns the lapsed time for the specified key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TimeSpan GetTimeForKey(string key)
        {
            Stopwatch watch;
            if (!timeValues.TryGetValue(key, out watch))
            {
                return default(TimeSpan);
            }

            watch.Stop();
            return watch.Elapsed;
        }

        public string FormattedTime(TimeSpan ts)
        {
            return ts.ToString("c");
        }
    }
}
