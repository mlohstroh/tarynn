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
        private List<MethodPackage> loadedPackages = new List<MethodPackage>();
        private Interpreter mInterpreter;
        private List<string> builtInFunctions = new List<string>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the script</param>
        public ScriptLoader(string name, Interpreter i)
        {
            mInterpreter = i;
            mName = name;
            mReader = new StreamReader("Scripts\\" + name);
            builtInFunctions.Add("new");
            builtInFunctions.Add("add");
            builtInFunctions.Add("sub");
        }

        public bool Validate()
        {
            bool validScript = true;
            Profiler.SharedInstance.StartProfiling("script_validate");
            TConsole.Info("Validating Script");
            string line;
            bool doesEnd = false;
            
            while ((line = mReader.ReadLine()) != null)
            {
                //skip the line completely if it's comment
                if (!line.Contains('#'))
                {
                    if (line.Length != 0)
                    {
                        //they are wanting to load a package here
                        if (line.StartsWith("use"))
                        {
                            string[] args = GetArgsForMethod(line);
                            Type type = Type.GetType(args[0], false);
                            if (type == null)
                            {
                                //Not library to load
                                return false;
                            }
                            //dynamically create an instance of the specified method package
                            MethodPackage newLib = (MethodPackage)Activator.CreateInstance(type);
                            loadedPackages.Add(newLib);
                        }
                        else
                        {
                            //make sure that all whitespace is gone for simplicity
                            line = line.Substring(0, line.IndexOf('('));

                            //checking for end of program
                            if (line.Contains("return"))
                            {
                                //get rid of it because this is just validation
                                line = line.Replace("return ", "");
                                //a return statement is required
                                doesEnd = true;
                            }

                            bool methodExists = false;

                            foreach (MethodPackage p in loadedPackages)
                            {
                                if (p.MethodExists(line))
                                    methodExists = true;
                            }

                            if (!methodExists)
                                return false;

                            Console.WriteLine(line);
                        }
                    }
                }
            }
            TConsole.Info("Done validating script");
            TConsole.Debug("Elapsed time for script validation: " + Profiler.SharedInstance.GetTimeForKey("script_validate"));

            if (!doesEnd)
                return doesEnd;

            return validScript;
        }

        public string NextLine()
        {
            return "";
        }

        private string[] GetArgsForMethod(string line)
        {
            char[] para = new char[] { '(', ')'};

            line = line.Remove(0, line.IndexOf('('));
            line = line.Trim(para);

            return line.Split(',');
        }
    }
}
