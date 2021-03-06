﻿using System;
using System.Collections;
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
        private TConsole _logger = new TConsole(typeof(ScriptLoader));
        private string mName;

        private StreamReader mReader;
        private List<MethodPackage> loadedPackages = new List<MethodPackage>();
        private Interpreter mInterpreter;
        private List<string> builtInFunctions = new List<string>();
        public ArrayList Lines { get; private set; }
        public Dictionary<string, int> Branches { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the script</param>
        public ScriptLoader(string name, Interpreter i)
        {
            Lines = new ArrayList();
            Branches = new Dictionary<string, int>();
            mInterpreter = i;
            mName = name;
            mReader = new StreamReader(name);
            builtInFunctions.Add("new");
            builtInFunctions.Add("add");
            builtInFunctions.Add("sub");
            builtInFunctions.Add("get_input");
            builtInFunctions.Add("return");
        }

        public bool Validate()
        {
            try
            {
                bool validScript = true;
                Profiler.SharedInstance.StartProfiling("script_validate");
                _logger.Info("Validating Script");
                string line;
                int lineNumber = 0;
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
                                    this.mInterpreter.AddError("Invalid Package: " + args[0]);
                                    //Not library to load
                                    return false;
                                }
                                //dynamically create an instance of the specified method package
                                MethodPackage newLib = (MethodPackage)Activator.CreateInstance(type);
                                newLib.Host = this.mInterpreter;
                                loadedPackages.Add(newLib);
                            }
                            else if (line.StartsWith("branch"))
                            {
                                string[] args = GetArgsForMethod(line);
                                Branches.Add(args[0], lineNumber);
                            }
                            else
                            {
                                //make sure that all whitespace is gone for simplicity
                                line = line.Substring(0, line.IndexOf('('));

                                //checking for end of program
                                if (line.Contains("return"))
                                {
                                    doesEnd = true;
                                    //it should just be a value
                                }

                                bool methodExists = false;

                                foreach (string s in builtInFunctions)
                                {
                                    if (s == line)
                                        methodExists = true;
                                }

                                if (!methodExists)
                                {
                                    foreach (MethodPackage p in loadedPackages)
                                    {
                                        if (p.MethodExists(line))
                                            methodExists = true;
                                    }
                                }

                                if (!methodExists)
                                {
                                    validScript = false;
                                    break;
                                }
                            }
                        }
                    }
                    Lines.Add(line);
                    lineNumber++;
                }
                _logger.Info("Done validating script");
                _logger.Debug("Elapsed time for script validation: " + Profiler.SharedInstance.GetTimeForKey("script_validate"));

                if (!doesEnd)
                {
                    this.mInterpreter.AddError("Script does not end!");
                    return doesEnd;
                }

                return validScript;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
            finally
            {
                mReader.Close();
            }
        }

        public void OpenStream()
        {
            mReader = new StreamReader(mName);
        }

        public string NextLine()
        {
            string line = mReader.ReadLine();

            while ((line != null) && line.Contains("#") || line == "")
            {
                line = mReader.ReadLine();
            }

            return line;
        }

        private string[] GetArgsForMethod(string line)
        {
            char[] para = new char[] { '(', ')'};

            line = line.Remove(0, line.IndexOf('('));
            line = line.Trim(para);

            return line.Split(',');
        }

        public List<MethodPackage> RequiredPackages
        {
            get { return loadedPackages; }
        }

        public void CloseStream()
        {
            mReader.Close();
        }
    }
}
