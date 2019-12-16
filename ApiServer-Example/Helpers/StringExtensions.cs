using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer_Example.Helpers
{
    public static class StringExtensions
    {
        public static Guid GetClientId(this string authorizationHeader)
        {
            var handler = new JwtSecurityTokenHandler();

            var header = authorizationHeader;
            var splitedHeader = header.Split(' ');
            var jsonToken = handler.ReadJwtToken(splitedHeader[1]);
            var clientIdString = jsonToken.Claims.First().Value;

            if(clientIdString == null)
                throw new Exception("Bad authorization header format!");

            if(!Guid.TryParse(clientIdString, out var clientId))
                throw new Exception("Bad authorization header format!");

            return clientId;
        }
    }
}
