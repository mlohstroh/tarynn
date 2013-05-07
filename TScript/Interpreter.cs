using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TScript
{
    public class Interpreter
    {
        ScriptLoader loader;

        public Interpreter(string name)
        {
            loader = new ScriptLoader(name);
        }

        /// <summary>
        /// The parent should invoke this to make sure the script will mostly run
        /// </summary>
        /// <returns>Whether or not the script it healthy</returns>
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
    }
}
