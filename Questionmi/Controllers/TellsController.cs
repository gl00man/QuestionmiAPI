using Microsoft.AspNetCore.Mvc;
using Questionmi.DTOs;
using Questionmi.Filters;
using Questionmi.Helpers;
using Questionmi.Models;
using Questionmi.Repositories;
using SellpanderAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Questionmi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TellsController : ControllerBase
    {
        private readonly ITellRepository _tellRepository;
        private readonly IAuthorizationRepository _authRepository;

        public TellsController(ITellRepository tellrepository, IAuthorizationRepository authRepository)
        {
            _tellRepository = tellrepository;
            _authRepository = authRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetTells([FromQuery] PaginationParams paginationParams, [FromQuery] TellsFilter tellsFilter)
        {
            if (!HttpContext.Authorize(_authRepository)) return Unauthorized();
            return Ok(await _tellRepository.Get(paginationParams, tellsFilter));
        }

        [HttpGet]
        [Route("/api/GetTellsForPost")]
        public async Task<ActionResult> GetTellsForPost()
        {
            if (!HttpContext.Authorize(_authRepository)) return Unauthorized();
            return Ok(await _tellRepository.GetForPost());
        }

        [HttpPost]
        public async Task<ActionResult> PostTell([FromBody] TellDto tell)
        {
            var userIp = HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString();
            return Ok(await _tellRepository.Create(userIp, tell));
        }

        [HttpPut]
        public async Task<ActionResult> PutTell([FromBody] Tell tell)
        {
            if (!HttpContext.Authorize(_authRepository)) return Unauthorized();
            await _tellRepository.Update(tell);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery(Name = "id"), Required(ErrorMessage = "id is required.")] int id )
        {
            if (!HttpContext.Authorize(_authRepository)) return Unauthorized();
            await _tellRepository.Delete(id);
            return Ok();
        }
    }
}