using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web.Mvc
{
    public static class CustomExtension
    {
        public static string TrimOneEndChar(this string s)
        {
            return s.Substring(0, s.Length - 1);
        }
    }
}