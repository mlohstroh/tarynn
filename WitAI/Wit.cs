using System;
using System.Linq;
using System.Net;
using LitJson;
using RestSharp;

namespace WitAI
{
    public class Wit
    {
        /// <summary>
        /// Access Token for your Wit app
        /// </summary>
        public string AccessToken { get; private set; }

        /// <summary>
        /// Hostname constant for API
        /// </summary>
        private const string WIT_HOSTNAME = "https://api.wit.ai";

        /// <summary>
        /// Current version of the API
        /// </summary>
        private const string WIT_VERSION = "20150409";

        private RestClient _client = new RestClient(WIT_HOSTNAME);
        public Wit(string accessToken)
        {
            if(string.IsNullOrEmpty((accessToken)))
                throw new ArgumentNullException("Wit Access is null.");

            AccessToken = accessToken;
        }
        
        /// <summary>
        /// Submit a query to wit and get a response
        /// </summary>
        /// <param name="query">The query to sumit</param>
        /// <remarks>If an error occurs </remarks>
        /// <returns>A built wit response based on the JSON of the request</returns>
        public WitResponse Query(string query, dynamic context = null, string msgId = null, int numberOfOutcomes = 1)
        {
            var request = BuildWitRequest("/message", Method.GET);
            request.AddParameter("q", query, ParameterType.QueryString);
            // Query is the only parameter required.
            if (context != null)
                request.AddParameter("context", context, ParameterType.QueryString);
            if (!string.IsNullOrEmpty(msgId))
                request.AddParameter("msg_id", msgId, ParameterType.QueryString);
            if (numberOfOutcomes < 0)
                request.AddParameter("n", numberOfOutcomes, ParameterType.QueryString);

            var response = _client.Execute(request);
            
            if (response.ErrorException == null)
            {
                JsonData data = JsonMapper.ToObject(response.Content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    WitResponse res = new WitResponse(data, response.Content);
                    return res;
                }
                else
                    throw new WitException((string) data["error"]);
            }
            else
                throw new WitException(string.Format("Something happened to the request. Error: {0}", response.ErrorMessage));

            return null;
        }

        /// <summary>
        /// Shortcut for building a wit request. It puts the version and the auth token
        /// </summary>
        /// <param name="url">URL to hit</param>
        /// <param name="m">HTTP Method</param>
        /// <returns></returns>
        private RestRequest BuildWitRequest(string url, Method m)
        {
            var request = new RestRequest("/message", Method.GET);
            request.AddHeader("Authorization", string.Format("Bearer {0}", AccessToken));
            request.AddHeader("Accept", "application/json");
            request.AddParameter("v", WIT_VERSION, ParameterType.QueryString);
            return request;
        }
    }
}
