using System;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DataAccess.Implementations;
using DataAccess.DbModels;

namespace DataAccess.DataAccess.Interfaces
{
    public class TokenRepository : ITokenRepository
    {
        public RefreshToken GetToken(string token)
        {
            using (var context = new ISControlDbContext())
            {
                return context.RefreshTokens.FirstOrDefault(x => x.Token == token && x.ExpiresAt > DateTime.Now);
            }
        }

        public async Task DeleteOutdatedTokens()
        {
            using (var context = new ISControlDbContext())
            {
                var tokens = context.RefreshTokens.Where(x => x.ExpiresAt <= DateTime.Now).ToList();
                context.RefreshTokens.RemoveRange(tokens);
                await context.SaveChangesAsync();
            }
        }

        public async Task AddToken(RefreshToken token)
        {
            using (var context = new ISControlDbContext())
            {
                context.RefreshTokens.Add(token);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteToken(string token)
        {
            using (var context = new ISControlDbContext())
            {
                var refreshToken = context.RefreshTokens.FirstOrDefault(x => x.Token == token);
                if (refreshToken != null)
                {
                    context.RefreshTokens.Remove(refreshToken);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}