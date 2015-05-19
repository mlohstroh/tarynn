using System;
using System.Collections.Generic;
using System.Net;

namespace TRouter
{
    public class Router
    {
        private List<Route> _routes = new List<Route> ();

        public Router ()
        {

        }

        public void AddRoute(Route r)
        {
            if (_routes.Contains(r))
            {
                throw new Exception(string.Format("Duplicate route {0}", r.RawUrl));
            }
            _routes.Add(r);
        }

        public void AddRoutes(List<Route> routes)
        {
            routes.ForEach(AddRoute);
        }

        // remove routes

        public TResponse MatchRoute(string url, TRequest req, HttpListenerResponse res)
        {
            foreach (var route in _routes)
            {
                if (route.Method == req.Method && route.DoesMatch(url))
                {
                    TResponse proxyResponse = route.TryExecute(req, res, url);
                    return proxyResponse;
                }
            }
            return null;
        }
    }
}

