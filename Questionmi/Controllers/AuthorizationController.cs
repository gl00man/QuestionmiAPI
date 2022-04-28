using Microsoft.AspNetCore.Mvc;
using Questionmi.Repositories;

namespace Questionmi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationRepository _authRepository;
        public AuthorizationController(IAuthorizationRepository authRepostitory)
        {
            _authRepository = authRepostitory;
        }
    }
}
