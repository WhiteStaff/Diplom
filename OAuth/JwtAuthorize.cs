using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;
using Common.Models.Enums;

namespace OAuth
{
    public class JwtAuthorize : AuthorizeAttribute
    {
        private readonly UserRole[] _requiredRoles;

        public JwtAuthorize(params UserRole[] roles)
        {
            _requiredRoles = roles;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var claims = GetClaims(actionContext);
            var role = claims.FindFirst(ClaimTypes.Role)?.Value;
            if (role == null)
            {
                return false;
            }

            return _requiredRoles.Select(x => x.ToString()).Contains(role);
        }

        private static ClaimsPrincipal GetClaims(HttpActionContext actionContext)
        {
            return actionContext.RequestContext.Principal as ClaimsPrincipal ?? throw new Exception("ClaimsPrincipal is null.");
        }
    }
}