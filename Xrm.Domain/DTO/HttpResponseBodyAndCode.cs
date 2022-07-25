using System.Collections.Generic;

namespace Xrm.Domain.DTO
{
    public class HttpResponseBodyAndCode
    {
        public int Code { get; set; }
        public string Body { get; set; }

        public bool IsOk => Code >= 200 && Code < 300;

        public byte[] Bytes { get; set; }
    }
}
