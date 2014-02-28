using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Gatsby
{
    public class HttpServer
    {
        HttpListener listener;

        public HttpServer()
        {
            this.listener = new HttpListener();
            this.listener.Prefixes.Add("http://localhost:8080/");
        }

        public void Start(string destination, string baseUrl)
        {
            this.listener.Start();

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var context = this.listener.GetContext();

                    Task.Factory.StartNew((state) =>
                    {
                        var requestContext = (HttpListenerContext)state;
                        string url = requestContext.Request.Url.LocalPath;

                        if (!url.StartsWith(baseUrl))
                        {
                            context.Response.StatusCode = 404;
                            context.Response.Close();
                            return;
                        }

                        url = url.Substring(baseUrl.Length);

                        if (url.Length == 0)
                            url = "index.html";

                        if (url.EndsWith("/"))
                            url += "index.html";

                        string filePath = Path.Combine(destination, url.Replace('/', Path.DirectorySeparatorChar));

                        if (File.Exists(filePath))
                        {
                            byte[] content = File.ReadAllBytes(filePath);
                            
                            requestContext.Response.ContentType = MimeMapping.GetMimeMapping(Path.GetFileName(filePath));
                            requestContext.Response.ContentLength64 = content.Length;
                            requestContext.Response.OutputStream.Write(content, 0, content.Length);
                            requestContext.Response.Close();
                        }
                        else
                        {
                            context.Response.StatusCode = 404;
                            context.Response.Close();
                        }
                    }, context);
                }
            }, TaskCreationOptions.LongRunning);
        }

        private static byte[] ReadAll(Stream stream)
        {
            byte[] buffer = new byte[8192];
            int bytesRead = 1;
            List<byte> bytes = new List<byte>();

            while (bytesRead > 0)
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                bytes.AddRange(new ArraySegment<byte>(buffer, 0, bytesRead).Array);
            }
            return bytes.ToArray();
        }
    }
}
