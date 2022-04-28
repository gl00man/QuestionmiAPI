using Questionmi.Models;
using System.Linq;

namespace Questionmi.Repositories
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly DatabaseContext _context;

        public AuthorizationRepository(DatabaseContext context)
        {
            _context = context;
        }

        public bool ValidateToken(string token)
        {
            return _context.Tokens
                .Where(t => t.AccessToken == token)
                .FirstOrDefault() != null;
        }
    }
}
