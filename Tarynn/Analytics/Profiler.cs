using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarynn.Analytics
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

        private Dictionary<string, int> timeValues;

        /// <summary>
        /// Constructor
        /// </summary>
        public Profiler()
        {
            timeValues = new Dictionary<string, int>();
        }

        /// <summary>
        /// Writes the time to a hash
        /// </summary>
        /// <param name="key">The key to retrieve the time from</param>
        public void StartProfiling(string key)
        {

        }

        /// <summary>
        /// Returns the lapsed time for the specified key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetTimeForKey(string key)
        {
            return -1;
        }
    }
}
