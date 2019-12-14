using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace ApiServer_Example.Helpers
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseStaticFileAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<StaticFileAuthorizeMiddleware>();
        }
    }
}
