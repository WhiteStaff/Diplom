using System.Threading.Tasks;
using DataAccess.DbModels;

namespace DataAccess.DataAccess.TokenRepository
{
    public interface ITokenRepository
    {
        RefreshToken GetToken(string token);

        Task DeleteOutdatedTokens();

        Task AddToken(RefreshToken token);

        Task DeleteToken(string token);
    }
}