using System;
using System.Collections.Generic;

namespace TModules
{
    public class ParameterString
    {
        private const string ParameterRegex = @"\/(:\w+)|\*\/";
        private string _underlyingString;
        public Dictionary<string, string> Parameters = new Dictionary<string, string>();


        public ParameterString (string s)
        {
            _underlyingString = s;


        }

        /// <summary>
        /// Checks if a string matches the parmeterized string
        /// </summary>
        /// <returns><c>true</c>, if match was doesed, <c>false</c> otherwise.</returns>
        /// <param name="url">URL.</param>
        public bool DoesMatch(string url)
        {
            return false;
        }
    }
}

