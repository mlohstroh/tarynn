using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Xml.Linq;
using Analytics;
using TRouter;

namespace TModules.Core
{

    //http://www.codehosting.net/blog/BlogEngine/post/Simple-C-Web-Server.aspx
    public class EmbeddedServer
    {
        private TConsole _logger = new TConsole (typeof(EmbeddedServer));

        HttpListener listener = new HttpListener();

        private Router _router;

        public void Start(Router r)
        {
            _router = r;

            if (listener.IsListening)
                Stop();

            listener.Prefixes.Add("http://localhost:1234/");

            listener.Start();
        }

        public void Stop()
        {
            if (listener.IsListening)
            {
                listener.Stop();
                listener.Close();
            }
        }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                _logger.Debug("Starting console");
                try
                {
                    while (listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                TResponse res = _router.MatchRoute(ctx.Request.Url.AbsolutePath, new TRequest(ctx.Request), 
                                    ctx.Response);

                                // a response will be issued if there was a route for it
                                if (res != null)
                                {
                                    if (!string.IsNullOrEmpty(res.RedirectURL))
                                    {
                                        // handle redirect
                                        ctx.Response.Redirect(res.RedirectURL);
                                    }
                                    else
                                    {
                                        // write the response
                                        byte[] bytes = res.ContentEncoding.GetBytes(res.ResponseBody);
                                        ctx.Response.StatusCode = res.StatusCode;
                                        ctx.Response.Headers = res.Headers;
                                        ctx.Response.ContentType = res.ContentType;
                                        ctx.Response.ContentEncoding = res.ContentEncoding;

                                        ctx.Response.ContentLength64 = bytes.Length;
                                        ctx.Response.OutputStream.Write(bytes, 0, bytes.Length);
                                    }
                                }
                                else
                                {
                                    ctx.Response.StatusCode = 404;
                                    ctx.Response.ContentType = "application/text";
                                    byte[] bytes = ctx.Response.ContentEncoding.GetBytes("404 Not Found");
                                    ctx.Response.ContentLength64 = bytes.Length;
                                    ctx.Response.OutputStream.Write(bytes, 0, bytes.Length);
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.ErrorFormat("Exception: {0}. {1}", ex.Message, ex.StackTrace);
                            }
                            finally
                            {
                                ctx.Response.OutputStream.Close();
                            }
                        }, listener.GetContext());
                    }
                }
                catch { }
            });
        }
    }
}
