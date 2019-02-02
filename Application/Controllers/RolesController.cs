using Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Application.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	[Produces("application/json")]
	public class RolesController : ControllerBase
	{
		private readonly Services.IRoleService _RoleService;

		public RolesController(Services.IRoleService roleService)
		{
			_RoleService = roleService;
		}
		
		[HttpGet]
		public async Task<IActionResult> GetAll([FromHeader]string authorization)
		{
			try
			{
				var res = await _RoleService.GetAll(authorization);
				return Ok(res);
			}
			catch (NotAllowedException e)
			{
				return Unauthorized();
			}
			catch (JwtTokenException e)
			{
				return BadRequest(e);
			}
		}
	}
}