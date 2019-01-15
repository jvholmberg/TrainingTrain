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
    public class UsersController : ControllerBase
    {

		private readonly IAuthService _authService;
        private readonly IUsersService _userService;

        public UsersController(IAuthService authService, IUsersService userService)
        {
			_authService = authService;
			_userService = userService;
        }

		// GET: /users/{id}
		[AllowAnonymous]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute]int id)
		{
			try
			{
				var res = await _userService.GetById(id);
				return Ok(res);
			}
			catch
			{
				return BadRequest();
			}
		}

	}
}