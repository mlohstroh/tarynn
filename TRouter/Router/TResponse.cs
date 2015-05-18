using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TRouter
{
    /// <summary>
    /// This class is a proxy class since we don't want to expose the 
    /// stream functionality to the end user. We should do that ourselves.
    /// </summary>
    public class TResponse
    {
        // Summary:
        //     Gets or sets the System.Text.Encoding for this response's System.Net.HttpListenerResponse.OutputStream.
        //
        // Returns:
        //     An System.Text.Encoding object suitable for use with the data in the System.Net.HttpListenerResponse.OutputStream
        //     property, or null if no encoding is specified.
        public Encoding ContentEncoding { get; set; }

        //
        // Summary:
        //     Gets or sets the MIME type of the content returned.
        //
        // Returns:
        //     A System.String instance that contains the text of the response's Content-Type
        //     header.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The value specified for a set operation is null.
        //
        //   System.ArgumentException:
        //     The value specified for a set operation is an empty string ("").
        //
        //   System.ObjectDisposedException:
        //     This object is closed.
        public string ContentType { get; set; }

        //
        // Summary:
        //     Gets or sets the collection of header name/value pairs returned by the server.
        //
        // Returns:
        //     A System.Net.WebHeaderCollection instance that contains all the explicitly
        //     set HTTP headers to be included in the response.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The System.Net.WebHeaderCollection instance specified for a set operation
        //     is not valid for a response.
        public WebHeaderCollection Headers { get; set; }

        //
        // Summary:
        //     Gets or sets the HTTP status code to be returned to the client.
        //
        // Returns:
        //     An System.Int32 value that specifies the HTTP status code for the requested
        //     resource. The default is System.Net.HttpStatusCode.OK, indicating that the
        //     server successfully processed the client's request and included the requested
        //     resource in the response body.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     This object is closed.
        //
        //   System.Net.ProtocolViolationException:
        //     The value specified for a set operation is not valid. Valid values are between
        //     100 and 999 inclusive.
        public int StatusCode { get; set; }

        /// <summary>
        /// Init the response class from a http response
        /// </summary>
        /// <param name="res"></param>
        public TResponse(HttpListenerResponse res)
        {
            ContentEncoding = res.ContentEncoding;
            ContentType = res.ContentType;
            Headers = res.Headers;
            StatusCode = res.StatusCode;
        }

        internal string RedirectURL { get; private set; }

        private TResponse(string redir)
        {
            RedirectURL = redir;
        }

        /// <summary>
        /// Creates a TResponse that will redirect the web client
        /// </summary>
        /// <param name="url">URL to redirect to</param>
        /// <returns></returns>
        public static TResponse Redirect(string url)
        {
            return new TResponse(url);
        }
    }
}
