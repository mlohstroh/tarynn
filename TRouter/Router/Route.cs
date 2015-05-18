using System;
using System.Net;

namespace TRouter
{
    public class Route
    {
        public ParameterString ParameterString { get; private set; }
        public Func<HttpWebRequest, HttpWebResponse, HttpWebResponse> DelegateFunction { get; private set; }

        public Route (string url, Func<HttpWebRequest, HttpWebResponse, HttpWebResponse> func)
        {
            ParameterString = new ParameterString(url);
            DelegateFunction = func;
        }
    }
}

