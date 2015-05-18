using System;
using System.Net;

namespace TRouter
{
    public class Route
    {
        public ParameterString ParameterString { get; private set; }
        public Func<HttpListenerRequest, TResponse, TResponse> DelegateFunction { get; private set; }

        public Route(string url, Func<HttpListenerRequest, TResponse, TResponse> func)
        {
            ParameterString = new ParameterString(url);
            DelegateFunction = func;
        }

        public bool DoesMatch(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            return ParameterString.DoesMatch(url);
        }

        public HttpListenerResponse TryExecute(HttpListenerRequest req, HttpListenerResponse res, string url)
        {
            TResponse proxy = new TResponse(res);

            if (DoesMatch(url))
            {
                
            }

            return null;
        }
    }
}

