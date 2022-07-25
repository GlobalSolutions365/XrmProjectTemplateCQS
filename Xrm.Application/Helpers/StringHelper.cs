using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xrm.Application.Helpers
{
    public static class StringHelper
    {
        public static string Left(this string str, int length)
        {
            str = (str ?? string.Empty);
            return str.Substring(0, Math.Min(length, str.Length));
        }

        public static string Right(this string str, int length)
        {
            str = (str ?? string.Empty);
            return (str.Length >= length)
                ? str.Substring(str.Length - length, length)
                : str;
        }

        public static bool IsValidExtension(this string str, params string[] extension)
        {
            var splitsStr = str.Split('.');
            if (splitsStr.Length > 0)
                return extension.Contains(splitsStr[splitsStr.Length - 1].ToLower());
            return false;
        }

        public static bool ToBool(this string str)
        {
            return Convert.ToBoolean(str.ToLower());
        }
    }
}
