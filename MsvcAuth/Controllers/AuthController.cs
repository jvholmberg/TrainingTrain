using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MsvcAuth.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	[Produces("application/json")]
    public class AuthController : ControllerBase
    {

        public AuthController()
        {
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
		public IActionResult Authenticate([FromHeader]string msvcAuthUserId, [FromHeader]string msvcAuthUserRole)
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