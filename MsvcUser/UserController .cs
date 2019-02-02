using Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MsvcUser.Controllers
{
	[Authorize]
	[ApiController]
	[Route("user")]
	[Produces("application/json")]
    public class UsersController : ControllerBase
    {
		
		private readonly Services.IUserService _UserService;

		public UsersController(Services.IUserService userService)
        {
			_UserService = userService;
        }
		
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromHeader]string authorization, [FromRoute]int id)
		{
			try
			{
				// Return requested user
				var res = await _UserService.GetById(authorization, id);
				return Ok(res);
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}
		
		[HttpGet]
		public async Task<IActionResult> GetAll([FromHeader]string authorization)
		{
			try
			{
				// Return all users
				var res = await _UserService.GetAll(authorization);
				return Ok(res);
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Create([FromBody]Views.Users.CreateRequest req)
		{
			try
			{
				var res = await _UserService.Create(req);
				return Ok(res);
			}
			catch
			{
				return BadRequest();
			}
		}

	}
}