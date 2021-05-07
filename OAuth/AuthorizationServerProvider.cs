using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAccess.DataAccess.Interfaces;
using Microsoft.Owin.Security.OAuth;

namespace OAuth
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IUserRepository userRepository;

        public AuthorizationServerProvider(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            return Task.Run(() => context.Validated());
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var user = userRepository.GetUser(context.UserName, context.Password);
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                context.Validated(identity);
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