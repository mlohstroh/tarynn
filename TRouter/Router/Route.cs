using System;
using System.Net;
using System.Net.Http;

namespace TRouter
{
    public class Route
    {
        public ParameterString ParameterString { get; private set; }
        public Action<TRequest, TResponse> DelegateFunction { get; private set; }

        public HttpVerb Method { get; private set; }

        public string RawUrl { get; private set; }

        public Route(HttpVerb method, string url, Action<TRequest, TResponse> func)
        {
            Method = method;
            ParameterString = new ParameterString(url);
            DelegateFunction = func;
            RawUrl = url;
        }

        public bool DoesMatch(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            return ParameterString.DoesMatch(url);
        }

        public TResponse TryExecute(TRequest req, HttpListenerResponse res, string url)
        {            
            TResponse proxyRes = new TResponse(res);

            if (DoesMatch(url))
            {
                req.Params = ParameterString;
                DelegateFunction(req, proxyRes);
            }

            return proxyRes;
        }

        public override bool Equals(object obj)
        {
            if (obj is Route)
                return (obj as Route).RawUrl == RawUrl && (obj as Route).Method == Method;
            else
                return base.Equals(obj);
        }
    }
}

