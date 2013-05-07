using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TScript.Methods;
using Analytics;

namespace TScript
{
    /// <summary>
    /// A class that loads and validates the script.
    /// It does not check for valid output 
    /// </summary>
    public class ScriptLoader
    {
        private string mName;

        private StreamReader mReader;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the script</param>
        public ScriptLoader(string name)
        {
            mName = name;
            mReader = new StreamReader("Scripts\\" + name);
        }

        public bool Validate()
        {
            Profiler.SharedInstance.StartProfiling("script_validate");
            TConsole.Info("Validating Script");
            string line;
            while ((line = mReader.ReadLine()) != null)
            {
                //skip the line completely if it's comment
                if (!line.Contains('#'))
                {
                    if (line.Length != 0)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            TConsole.Info("Done validating script");
            TConsole.Debug("Elapsed time for script validation: " + Profiler.SharedInstance.GetTimeForKey("script_validate"));
            return false;
        }

        public string NextLine()
        {
            return "";
        }
    }
}
