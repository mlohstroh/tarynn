using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TRouter
{
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

        public TRequest(HttpListenerRequest req)
        {
            
        }
    }
}
