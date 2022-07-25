using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xrm.Application.Interfaces;
using Xrm.Domain.DTO;

namespace Xrm.UnitTests.Fakes
{
    public class FakeHttpRequestExecutor : IHttpRequestExecutor
    {
        private List<Request> KnownRequests { get; set; } = new List<Request>();

        public List<Request> ExecutedRequests { get; set; } = new List<Request>();

        public void AddKnownRequest(HttpMethod method, string url, string body, HttpResponseBodyAndCode response)
        {
            KnownRequests.Add(new Request
            {
                Method = method,
                Url = url,
                Body = body,
                Response = response
            });
        }

        public HttpResponseBodyAndCode Delete(string url, string requestBody, Dictionary<string, object> extraHeaders)
        {
            return TryExecute(HttpMethod.Delete, url, requestBody);
        }

        public HttpResponseBodyAndCode Get(string url, Dictionary<string, object> extraHeaders)
        {
            return TryExecute(HttpMethod.Get, url, null);
        }

        public HttpResponseBodyAndCode Post(string url, string requestBody, Dictionary<string, object> extraHeaders)
        {
            return TryExecute(HttpMethod.Post, url, requestBody);
        }

        public HttpResponseBodyAndCode Put(string url, string requestBody, Dictionary<string, object> extraHeaders)
        {
            return TryExecute(HttpMethod.Put, url, requestBody);
        }

        private HttpResponseBodyAndCode TryExecute(HttpMethod method, string url, string body)
        {
            Request request = FindRequest(method, url, body);
            ExecutedRequests.Add(request);
            return request.Response;
        }

        private Request FindRequest(HttpMethod method, string url, string body)
        {
            Console.WriteLine($"Trying to execute Method: {method} URL: {url} Body: {body}");

            var request = KnownRequests
                            .Where(r =>
                                r.Method == method
                                && (r.Url ?? "") == (url ?? "")
                                && (r.Body ?? "") == (body ?? "")
                            ).FirstOrDefault();

            if (request == null)
            {
                throw new Exception($"No request found for Method: {method} URL: {url} Body: {body}");
            }

            return request;
        }

        public class Request
        {
            public HttpMethod Method { get; set; }

            public string Url { get; set; }

            public string Body { get; set; }

            public HttpResponseBodyAndCode Response { get; set; }
        }
    }
}
