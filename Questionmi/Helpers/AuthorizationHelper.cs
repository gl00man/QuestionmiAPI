using Microsoft.AspNetCore.Http;
using Questionmi.Repositories;

namespace Questionmi.Helpers
{
    public static class AuthorizationHelper
    {
        public static string GetToken(this HttpContext context)
        {
            context.Request.Headers.TryGetValue("token", out var token);
            return token;
        }

        public static bool Authorize(this HttpContext context, IAuthorizationRepository authRepo)
        {
            var token = context.GetToken();
            return authRepo.ValidateToken(token);
        }
    }
}
