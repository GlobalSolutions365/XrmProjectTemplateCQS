using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xrm.Application.Helpers
{
    public static class HttpRequestHelper
    {
        public static string GenerateBasicAuthHeader(string username, string password)
        {
            return "Basic " +
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", username, password)
                    ));
        }
    }
}
