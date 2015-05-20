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
                _logger.Debug("Starting http server");
                try
                {
                    while (listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                ProcessRequest(ctx);
                                LogResponse(ctx);
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

        private void ProcessRequest(HttpListenerContext ctx)
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

                    WriteResponse(ctx.Response, bytes);
                }
            }
            else
            {
                ctx.Response.StatusCode = 404;
                ctx.Response.ContentType = "application/text";
                if (ctx.Response.ContentEncoding == null)
                    ctx.Response.ContentEncoding = Encoding.ASCII;

                byte[] bytes = ctx.Response.ContentEncoding.GetBytes("404 Not Found");
                WriteResponse(ctx.Response, bytes);
            }
        }

        private void WriteResponse(HttpListenerResponse res, byte[] body)
        {
            res.ContentLength64 = body.Length;
            res.OutputStream.Write(body, 0, body.Length);
        }

        private void LogResponse(HttpListenerContext ctx)
        {
            _logger.InfoFormat("{0} Status: {1} {2}", ctx.Request.HttpMethod, ctx.Response.StatusCode, ctx.Request.Url.AbsolutePath);
        }
    }
}
