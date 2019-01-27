using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Application.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	[Produces("application/json")]
    public class AuthController : ControllerBase
    {

        private readonly Services.IAuthService _AuthService;

        public AuthController(Services.IAuthService authorizationService)
        {
			_AuthService = authorizationService;
        }

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Authenticate([FromBody]Views.Auth.AuthenticateRequest input)
		{
			try
			{
				var res = await _AuthService.Authenticate(input.Username, input.Password);
				return Ok(res);
			}
			catch
			{
				return BadRequest();
			}
		}
		
		[HttpGet]
		public IActionResult Authenticate([FromHeader]string authorization)
		{
			return Ok();
		}

		[AllowAnonymous]
		[HttpPut("{refreshToken}")]
		public async Task<IActionResult> Refresh([FromRoute] string refreshToken)
		{
			try
			{
				var res = await _AuthService.Authenticate(refreshToken);
				return Ok(res);
			}
			catch
			{
				return BadRequest();
			}
		}
		
		[HttpDelete("{id}")]
		public async Task<IActionResult> Destroy([FromHeader]string authorization, [FromRoute] int id)
		{
			try
			{
				var destroyed = await _AuthService.Destroy(authorization, id);
				if (destroyed)
				{
					return Ok();
				}
				else
				{
					return Unauthorized();
				}
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}