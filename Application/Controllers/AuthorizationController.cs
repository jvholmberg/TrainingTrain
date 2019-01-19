using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Application.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	[Produces("application/json")]
    public class AuthorizationController : ControllerBase
    {

        private readonly Services.IAuthorizationService _AuthorizationService;

        public AuthorizationController(Services.IAuthorizationService authorizationService)
        {
			_AuthorizationService = authorizationService;
        }

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Create([FromBody]string username, [FromBody]string password)
		{
			try
			{
				return Ok();
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpGet]
		public async Task<IActionResult> Validate([FromHeader]string authorization)
		{
			try
			{
				return Ok();
			}
			catch
			{
				return BadRequest();
			}
		}
		
		[HttpDelete]
		public async Task<IActionResult> Destroy([FromHeader]string authorization)
		{
			try
			{
				return Ok();
			}
			catch
			{
				return Unauthorized();
			}
		}
	}
}