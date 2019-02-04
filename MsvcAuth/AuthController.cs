using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MsvcAuth.Controllers
{
	[Authorize]
	[ApiController]
	[Route("auth")]
	[Produces("application/json")]
    public class AuthController : ControllerBase
    {
		private readonly Services.IAuthService _AuthService;

        public AuthController(Services.IAuthService authService)
        {
			_AuthService = authService;
        }

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Authenticate([FromBody]Views.MsvcAuthLoginRequestBody req)
		{
			try
			{
				var res = await _AuthService.Authenticate(req.Username, req.Password);
				return Ok(res);
			}
			catch
			{
				return BadRequest();
			}
		}
		
		[HttpGet]
		public IActionResult Authenticate()
		{
			return Ok();
		}

		[AllowAnonymous]
		[HttpPut("{refreshToken}")]
		public async Task<IActionResult> Refresh([FromHeader]Views.MsvcAuthRequestHeaders headers, [FromRoute] string refreshToken)
		{
			try
			{
				int.TryParse(headers.MsvcAuthUserId, out int msvcAuthUserId);
				var res = await _AuthService.Authenticate(msvcAuthUserId, headers.MsvcAuthUserRole, refreshToken);
				return Ok(res);
			}
			catch
			{
				return BadRequest();
			}
		}
		
		[HttpDelete("{id}")]
		public async Task<IActionResult> Destroy([FromHeader]Views.MsvcAuthRequestHeaders headers, string id)
		{
			try
			{
				var destroyed = await _AuthService.Destroy(headers.MsvcAuthUserId, id);
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