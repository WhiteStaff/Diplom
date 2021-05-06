using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Infrastructure;

namespace OAuth
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new System.NotImplementedException();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            context.SetToken(GetRandomKey());
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new System.NotImplementedException();
        }

        public Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            throw new System.NotImplementedException();
        }

        private static string GetRandomKey()
        {
            const int keyLength = 32;
            using (var crypto = new RNGCryptoServiceProvider())
            {
                var buffer = new byte[keyLength];
                crypto.GetBytes(buffer);

                return Convert.ToBase64String(buffer);

            }
        }
    }
}