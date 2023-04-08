using Feature.Core.AuthUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Core.Controllers
{
    public class CoreControllerBase : Controller
    {
        private const string HTTP_HEADER_AUTHORIZATION = "Authorization";
        private const string BearerStringForReplace = "bearer ";
        private CurrentUser _currentUser;


        public CurrentUser CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    BuildCurrentUser();
                }

                return _currentUser;
            }
            private set
            {
                _currentUser = value;
            }
        }

        private void BuildCurrentUser()
        {
            var caller = User as ClaimsPrincipal;
            CurrentUser = BuildCurrentUserFrom(caller, Request);
        }
        public static CurrentUser BuildCurrentUserFrom(ClaimsPrincipal caller, HttpRequest request)
        {
            if (caller != null
                && caller.Identity.IsAuthenticated)
            {
                var token = GetToken(request);
                var tokenHandler = new JwtSecurityTokenHandler();

                var jwtToken = tokenHandler.ReadJwtToken(token);

                var currentUser = new CurrentUser();
                currentUser.UserId = int.Parse(jwtToken.Claims.First(x => x.Type == "userId").Value);
                currentUser.UserName = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                currentUser.Name = jwtToken.Claims.First(x => x.Type == "name").Value;
                currentUser.LastName = jwtToken.Claims.First(x => x.Type == "lastName").Value;



                return currentUser;
            }

            return null;
        }

        public static string GetToken(HttpRequest request)
        {
            if (request == null)
            {
                return string.Empty;
            }

            StringValues headerValues;

            if (request.Headers.TryGetValue(HTTP_HEADER_AUTHORIZATION, out headerValues))
            {
                var authValue = headerValues.FirstOrDefault();
                var token = authValue.Substring(7, authValue.Length - 7).Trim();
                return token;
            }

            return string.Empty;
        }

    }
}
