using System;
using System.Collections.Generic;
using Xrm.Domain.DTO;

namespace Xrm.Application.Interfaces
{
    public interface IHttpRequestExecutor
    {
        HttpResponseBodyAndCode Post(string url, string requestBody, Dictionary<string, object> extraHeaders);

        HttpResponseBodyAndCode Put(string url, string requestBody, Dictionary<string, object> extraHeaders);

        HttpResponseBodyAndCode Get(string url, Dictionary<string, object> extraHeaders);

        HttpResponseBodyAndCode Delete(string url, string requestBody, Dictionary<string, object> extraHeaders);
    }
}
