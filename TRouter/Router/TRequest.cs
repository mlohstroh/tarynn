using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LitJson;

namespace TRouter
{
    public enum HttpVerb
    {
        Get = 0,
        Post,
        Delete,
        Patch,
    }

    public class TRequest
    {
        //
        // Summary:
        //     Gets the System.Uri object requested by the client.
        //
        // Returns:
        //     A System.Uri object that identifies the resource requested by the client.
        public Uri Url { get; private set; }

        //
        // Summary:
        //     Gets the MIME type of the body data included in the request.
        //
        // Returns:
        //     A System.String that contains the text of the request's Content-Type header.
        public string ContentType { get; private set; }

        /// <summary>
        /// This is the automatically parsed JSON data if the content type matches
        /// </summary>
        public JsonData Body { get; private set; }

        /// <summary>
        /// This is in case you want to implement your own parser
        /// </summary>
        public string RawBody { get; private set; }

        public HttpVerb Method { get; private set; }
        public TRequest(HttpListenerRequest req)
        {
            Url = req.Url;
            ContentType = req.ContentType;
            ParseHttpMethod(req.HttpMethod);

            if (req.HasEntityBody)
            {
                // parse it only as JSON

                // TODO: Fix this. This is terrible. Two copies for a completely unknown data size
                using (MemoryStream stream = new MemoryStream())
                {
                    req.InputStream.CopyTo(stream);
                    byte[] buffer = stream.ToArray();
                    string body = req.ContentEncoding.GetString(buffer);

                    RawBody = body;
                    if (req.ContentType.Contains("application/json"))
                    {
                        try
                        {
                            Body = JsonMapper.ToObject(RawBody);
                        }
                        catch (Exception)
                        {
                            // stupid
                            Body = RawBody;
                        }
                    }
                }
            }
        }

        private void ParseHttpMethod(string method)
        {
            Method = (HttpVerb) Enum.Parse(typeof (HttpVerb), method, true);
        }
    }
}
