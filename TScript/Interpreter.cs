using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TScript.Methods;

namespace TScript
{
    public class Interpreter
    {
        ScriptLoader loader;
        private List<string> mErrors = new List<string>();
        private Dictionary<string, TObject> scriptObjects = new Dictionary<string, TObject>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The file name of the script</param>
        public Interpreter(string name)
        {
            loader = new ScriptLoader(name, this);
        }

        /// <summary>
        /// The parent should invoke this to make sure the script will mostly run
        /// </summary>
        /// <returns>Whether or not the script is healthy</returns>
        public bool Validate()
        {
            return loader.Validate();
        }

        /// <summary>
        /// This is where the magic happens
        /// </summary>
        /// <returns></returns>
        public string GetFinalText()
        {
            string line;
            while ((line = loader.NextLine()) != null)
            {
                string method = StripMethodFromLine(line);
                string[] argNames = GetArgsFromLine(line);

                switch (method)
                {
                    case "new":
                        break;
                    case "add": 
                        break;
                    case "sub":
                        break;
                    default:
                        //search for package to pass info on to
                        break;
                }
            }
            return "<stubbed>";
        }

        /// <summary>
        /// Gets a nice friendly string full of errors
        /// </summary>
        /// <returns></returns>
        public string GetErrors()
        {
            StringBuilder b = new StringBuilder();

            foreach(string s in mErrors)
            {
                b.Append(s + "\n");
            }

            return b.ToString();
        }

        /// <summary>
        /// Adds error to the interpreter. This normally occurs after validation
        /// </summary>
        /// <param name="error">The error to add</param>
        public void AddError(string error)
        {
            mErrors.Add(error);
        }

        private string StripMethodFromLine(string line)
        {
            return line.Substring(0, line.IndexOf('('));
        }

        private string[] GetArgsFromLine(string line)
        {
            char[] para = new char[] { '(', ')' };

            line = line.Remove(0, line.IndexOf('('));
            line = line.Trim(para);
            string[] trimmedArgs = line.Split(',');
            for(int i = 0; i< trimmedArgs.Length; i++)
            {
                string s = trimmedArgs[i].Trim();
                trimmedArgs[i] = s;
            }

            return trimmedArgs;
        }
    }
}
