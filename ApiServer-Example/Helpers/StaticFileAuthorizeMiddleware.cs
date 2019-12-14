using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiServer_Example.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace ApiServer_Example.Helpers
{
    public class StaticFileAuthorizeMiddleware
    {
        private readonly RequestDelegate _next;

        public StaticFileAuthorizeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IFileRepository fileRepository)
        {
            var authorizationHeader = context.Request.Headers["Authorization"];

            if (authorizationHeader == StringValues.Empty)
                await _next.Invoke(context);
            else
            {
                var clientIdString = authorizationHeader.ToString().GetClientId();
                var fileIdString = context.Request.Path.ToString().Replace("/","");
                var requestedFile = fileRepository.GetFileByIdAsync(Guid.Parse(fileIdString));
                //if(requestedFile == null)

            }
        }
    }
}
