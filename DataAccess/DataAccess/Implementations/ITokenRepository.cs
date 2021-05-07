using System.Threading.Tasks;
using DataAccess.DbModels;

namespace DataAccess.DataAccess.Implementations
{
    public interface ITokenRepository
    {
        RefreshToken GetToken(string token);

        Task DeleteOutdatedTokens();

        Task AddToken(RefreshToken token);

        Task DeleteToken(string token);
    }
}