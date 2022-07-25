using System;

namespace Xrm.Application.Helpers
{
    public static class CrmUlrParser
    {
        /// <summary>
        /// Gets record ID from URL
        /// </summary>
        /// <param name="url">Record URL like: https://dsndev.crm4.dynamics.com:443/main.aspx?etc=2&id=1f1f23b1-8ade-ea11-a813-000d3ab4f434&histKey=980689625&newWindow=true&pagetype=entityrecord</param>
        /// <returns>Id, like: 1f1f23b1-8ade-ea11-a813-000d3ab4f434</returns>
        public static Guid? IdFromUrl(string url)
        {
            try
            {
                string[] split = url.Split('?');
                split = split[1].Split('&');

                foreach (string parameter in split)
                {
                    if (parameter.StartsWith("id"))
                    {
                        string[] subSplit = parameter.Split('=');
                        return Guid.Parse(subSplit[1]);
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

    }
}
