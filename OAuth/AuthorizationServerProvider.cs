using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.Owin.Security.OAuth;

namespace OAuth
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            return Task.Run(() => context.Validated());
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (var db = new ISControlDbContext())
            {
                try
                {
                    var form = await context.Request.ReadFormAsync();
                    var user = db.Employee.FirstOrDefault(x =>
                        x.Email ==  context.UserName && x.Password == context.Password);
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                    context.Validated(identity);
                }
                catch (Exception e)
                {
                    context.SetError("Unauthorized", e.Message);
                }
            }
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            return base.GrantRefreshToken(context);
        }
    }
}