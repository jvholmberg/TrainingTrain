using Application.Entities;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Application.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	[Produces("application/json")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
			_authService = authService;
        }
		
		// POST: /users/logout
		[HttpPost("logout")]
		public async Task<IActionResult> Logout([FromHeader]string authorization)
		{
			try
			{
				var res = await _authService.Logout(authorization);
				return Ok(res);
			}
			catch
			{
				return BadRequest();
			}
		}

		// POST: /users/renew
		[HttpPost("renew")]
		public async Task<IActionResult> Renew([FromHeader]string authorization)
		{
			try
			{
				var res = await _authService.Renew(authorization);
				return Ok(res);
			}
			catch
			{
				return Unauthorized();
			}
		}

		// POST: /users/authenticate
		[HttpPost("authenticate")]
		public async Task<IActionResult> Authenticate([FromHeader]string authorization)
		{
			try
			{
				var res = await _authService.Authenticate(authorization);
				return Ok(res);
			}
			catch
			{
				return Unauthorized();
			}
		}

		// POST: /users/login
		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody]Views.LoginRequest input)
		{
			try
			{
				var res = await _authService.Login(input);
				return Ok(res);
			}
			catch
			{
				return BadRequest();
			}
		}

		// POST: /users/register
		[AllowAnonymous]
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody]Views.RegisterRequest input)
		{
			try
			{
				var res = await _authService.Register(input);
				return Ok(res);
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}