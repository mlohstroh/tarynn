using System;
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

        public JsonData MakePostRequest(string resource, string jsonPost, JsonData _data)
        {
            JsonData data = new JsonData();

            data["method"] = "POST";
            data["resource"] = resource;
            data["post_data"] = JsonMapper.ToObject(jsonPost);

            return data;
        }

        private RestRequest BuildRequest(string resource, Method method = Method.GET)
        {
            RestRequest r = new RestRequest(resource, method);
            r.RequestFormat = DataFormat.Json;
            r.AddHeader("Content-Type", "application/json");
            return r;
        }

        public JsonData SetHost(string host, JsonData data)
        {
            _client = new RestClient(host);

            return data;
        }
    }
}
