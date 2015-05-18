using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Text.RegularExpressions;

namespace TRouter
{
    public class ParameterString
    {
        private const string ParameterRegex = @"(:\w+)";
        private const string ParameterSegmentRegex = @"(.+?)";
        private string _underlyingString;
        public Dictionary<string, string> Parameters { get; private set; }
        public List<string> ParameterKeys { get; private set; }

        public string URLRegex { get; private set; }

        public ParameterString (string s)
        {
            ParameterKeys = new List<string>(10);
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
            // Make a new expandoobject that we can tack things onto
            Parameters = new Dictionary<string, string>();

            Match m = Regex.Match(url, URLRegex,
                RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

            if (m.Success)
            {
                // parse the parameters

                for (int i = 0; i < m.Groups.Count - 1; i++)
                {
                    string key = ParameterKeys[i];
                    string val = m.Groups[i + 1].Value;
                    Parameters.Add(key, val);
                }

                return true;
            }

            return false;
        }
    }
}

