using System;
using System.Net;
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
        /// <returns>A built wit response based on the JSON of the request</returns>
        public WitResponse Query(string query)
        {
            var request = BuildWitRequest("/message", Method.GET);
            request.AddParameter("q", query, ParameterType.QueryString);

            var response = _client.Execute(request);

            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                Console.WriteLine("Awesome!");
            }
            Console.WriteLine(response.Content);

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
            request.AddParameter("v", WIT_VERSION, ParameterType.QueryString);
            return request;
        }
    }
}
