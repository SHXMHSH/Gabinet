using System;
using Microsoft.AspNetCore.Http;

namespace Gabinet.Extension
{
    public static class PathAndQueryExtension
    {
        public static string PathAndQuery(this HttpRequest request)
        {
          return   request.QueryString.HasValue ? string.Format("{0}{1}", request.Path, request.QueryString) :
                request.Path.ToString();
        }

    }
}
