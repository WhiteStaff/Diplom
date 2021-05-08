using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Common.Helpers;
using DataAccess.DataAccess.UserRepository;
using Microsoft.Owin.Security.OAuth;

namespace OAuth
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IUserRepository _userRepository;

        public AuthorizationServerProvider(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            return Task.Run(() => context.Validated());
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var user = _userRepository.GetUser(context.UserName, context.Password.HashPassword());
                if (user != null)
                {
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                    identity.AddClaim(new Claim(ClaimTypes.Role, user.UserRole.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    context.Validated(identity);
                }
            }
            catch (Exception e)
            {
                context.SetError("Unauthorized", e.Message);
            }
        }

        public override async Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var email = context.Ticket.Identity.FindFirst(ClaimTypes.Email)?.Value;
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Email, email));
            context.Validated(identity);
        }
    }
}