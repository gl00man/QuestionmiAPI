using System.Threading.Tasks;

namespace Questionmi.Repositories
{
    public interface IAuthorizationRepository
    {
        bool ValidateToken(string token);
    }
}
