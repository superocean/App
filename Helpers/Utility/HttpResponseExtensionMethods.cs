using System.Web;
using System.Web.Caching;

namespace DealSearch.Helpers.Resources
{
    public static class HttpResponseExtensionMethods
    {
        public static void SetDefaultImageHeaders(this HttpResponseBase response)
        {
            response.Cache.SetCacheability(HttpCacheability.Public);
            response.Cache.SetExpires(Cache.NoAbsoluteExpiration);
            response.Cache.SetLastModifiedFromFileDependencies();
        }
    }
}