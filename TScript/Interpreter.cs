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
            return "<stubbed>";
        }

        /// <summary>
        /// Adds error to the interpreter. This normally occurs after validation
        /// </summary>
        /// <param name="error">The error to add</param>
        public void AddError(string error)
        {
            mErrors.Add(error);
        }
    }
}
