using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Xrm.Application.Interfaces;
using Xrm.Domain.DTO;

namespace PhysicalHttpRequestExecutor
{
    public class HttpRequestExecutor : IHttpRequestExecutor
    {
        public HttpResponseBodyAndCode Post(string url, string requestBody = "", Dictionary<string, object> extraHeaders = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";

            ProcessExtraHeaders(request, extraHeaders);

            byte[] data = Encoding.UTF8.GetBytes(requestBody);

            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            return GetResponse(request);
        }

        public HttpResponseBodyAndCode Put(string url, string requestBody = "", Dictionary<string, object> extraHeaders = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";

            ProcessExtraHeaders(request, extraHeaders);

            byte[] data = Encoding.UTF8.GetBytes(requestBody);

            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            return GetResponse(request);
        }

        public HttpResponseBodyAndCode Get(string url, Dictionary<string, object> extraHeaders)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.PreAuthenticate = true;

            ProcessExtraHeaders(request, extraHeaders);

            return GetResponse(request);
        }

        private static HttpResponseBodyAndCode GetResponse(HttpWebRequest request)
        {
            int statusCode = 0;
            string body = "";
            List<byte> bytes = new List<byte>();

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                {
                    int b = 0;
                    while(true)                            
                    {
                        b = stream.ReadByte();
                        if (b == -1) { break; }

                        bytes.Add((byte)b);
                    }

                    statusCode = (int)response.StatusCode;
                    body = UTF8Encoding.UTF8.GetString(bytes.ToArray());
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        string errMessage = reader.ReadToEnd();

                        statusCode = (int)((HttpWebResponse)ex.Response).StatusCode;
                        body = $"Web exception: {errMessage}{Environment.NewLine}{Environment.NewLine}Exception: {ex}";
                    }
                }
                else if (ex.Status == WebExceptionStatus.NameResolutionFailure)
                {
                    statusCode = 404;
                    body = $"Exception: {ex}";
                }
                else
                {
                    statusCode = 500;
                    body = $"Exception: {ex}";
                }
            }
            catch (Exception ex)
            {
                statusCode = 500;
                body = $"Exception: {ex}";
            }

            return new HttpResponseBodyAndCode
            {
                Code = statusCode,
                Body = body,
                Bytes = bytes.ToArray() ?? new byte[0]
            };
        }

        private static void ProcessExtraHeaders(HttpWebRequest request, Dictionary<string, object> extraHeaders)
        {
            if (extraHeaders != null)
            {
                foreach (string key in extraHeaders.Keys)
                {
                    if (key == "Content-Type")
                    {
                        request.ContentType = (string)extraHeaders[key];
                    }
                    else if (key == "Accept")
                    {
                        request.Accept = (string)extraHeaders[key];
                    }
                    else if (key == "ContentLength")
                    {
                        request.ContentLength = (int)extraHeaders[key];
                    }
                    else if (key == "Date")
                    {
                        request.Date = (DateTime)extraHeaders[key];
                    }
                    else
                    {
                        request.Headers.Add(key, (string)extraHeaders[key]);
                    }
                }
            }
        }

        public HttpResponseBodyAndCode Delete(string url, string requestBody = "", Dictionary<string, object> extraHeaders = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "DELETE";
            request.PreAuthenticate = true;

            ProcessExtraHeaders(request, extraHeaders);

            byte[] data = Encoding.UTF8.GetBytes(requestBody);

            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            return GetResponse(request);
        }
    }
}
