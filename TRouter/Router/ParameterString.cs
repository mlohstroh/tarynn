using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TRouter
{
    public class ParameterString
    {
        private const string ParameterRegex = @"(:\w+)";
        private const string ParameterSegmentRegex = @"(.*?)";
        private string _underlyingString;
        public Dictionary<string, string> Parameters = new Dictionary<string, string>();
        public List<string> ParameterKeys = new List<string>(10);

        public string URLRegex { get; private set; }

        public ParameterString (string s)
        {
            _underlyingString = s;
            DiceAndCompileRegex();
        }

        private void DiceAndCompileRegex()
        {
            string[] split = _underlyingString.Split('/');

            if(split.Length <= 1)
                throw new Exception("Invalid URL");

            StringBuilder builder = new StringBuilder();

            // start regex
            builder.Append(@"\A");

            for (int i = 1; i < split.Length; i++)
            {
                builder.Append(@"/");
                string segment = split[i];

                Match m = Regex.Match(segment, ParameterRegex, RegexOptions.Singleline | RegexOptions.IgnoreCase);

                string regexAdd = segment;

                if (m.Success)
                {
                    string key = m.Groups[0].Value;
                    // strip the colon out
                    key = key.Remove(0, 1);
                    ParameterKeys.Add(key);
                    regexAdd = ParameterSegmentRegex;
                }

                builder.Append(regexAdd);
            }

            // end regex with optional / at the end
            builder.Append(@"/?$");

            URLRegex = builder.ToString();
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

