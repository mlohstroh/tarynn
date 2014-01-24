﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModules.Core;
using RestSharp;
using LitJson;

namespace TModules.DefaultModules
{
    public class ProxyModule : TModules.Core.TModule
    {
        private RestClient _client;

        public ProxyModule(ModuleManager host)
            : base("Proxy", host)
        {
            _client = new RestClient();
        }

        public JsonData MakeGetRequest(string resourcePath)
        {
            return null;
        }

        private RestRequest BuildRequest(string resource)
        {
            RestRequest r = new RestRequest(resource, Method.GET);
            r.RequestFormat = DataFormat.Json;
            r.AddHeader("Content-Type", "application/json");
            return r;
        }
    }
}
