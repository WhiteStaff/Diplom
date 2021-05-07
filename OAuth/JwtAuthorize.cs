using System;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;
using Common.Models.Enums;

namespace OAuth
{
    public class JwtAuthorize : AuthorizeAttribute
    {
        private readonly UserRole _requiredRole;

        public JwtAuthorize(UserRole role)
        {
            _requiredRole = role;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var claims = GetClaims(actionContext);
            var role = claims.FindFirst(ClaimTypes.Role)?.Value;
            if (role == null)
            {
                return false;
            }

            return role == _requiredRole.ToString();
        }

        private static ClaimsPrincipal GetClaims(HttpActionContext actionContext)
        {
            return actionContext.RequestContext.Principal as ClaimsPrincipal ?? throw new Exception("ClaimsPrincipal is null.");
        }
    }
}