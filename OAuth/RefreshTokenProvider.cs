using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DataAccess.DataAccess.Implementations;
using DataAccess.DbModels;
using Microsoft.Owin.Security.Infrastructure;

namespace OAuth
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        private static readonly TimeSpan TokenLifetime = TimeSpan.Parse(ConfigurationManager.AppSettings["Security.RefreshTokenLifetime"]);
        private readonly ITokenRepository _tokenRepository;

        public RefreshTokenProvider(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new System.NotImplementedException();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var token = new RefreshToken
            {
                Token = GetRandomKey(),
                ExpiresAt = DateTime.Now.Add(TokenLifetime),
                ProtectedData = context.SerializeTicket()
            };
            await _tokenRepository.AddToken(token);
            await _tokenRepository.DeleteOutdatedTokens();
            context.SetToken(token.Token);
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new System.NotImplementedException();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var token = _tokenRepository.GetToken(context.Token);
            if (token != null)
            {
                context.DeserializeTicket(token.ProtectedData);
                await _tokenRepository.DeleteToken(token.Token);
            }
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