using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using Analytics;

namespace TModules.Core
{

    //http://www.codehosting.net/blog/BlogEngine/post/Simple-C-Web-Server.aspx
    class EmbeddedServer
    {
        private TConsole _logger = new TConsole (typeof(EmbeddedServer));

        HttpListener listener = new HttpListener();

        public Dictionary<string, Action<LitJson.JsonData>> Prefixes = new Dictionary<string, Action<LitJson.JsonData>>();

        public void Start()
        {
            if (listener.IsListening)
                Stop();

            foreach (var pair in Prefixes)
            {
                listener.Prefixes.Add("http://localhost:1234" + pair.Key);
            }

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
                                _logger.Debug(ctx.Request.Url.AbsoluteUri);
                                string response = "Hello world!";
                                byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(response);

                                ctx.Response.ContentLength64 = b.Length;
                                ctx.Response.OutputStream.Write(b, 0, b.Length);
                            }
                            catch { }
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
