using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	[Produces("application/json")]
    public class UsersController : ControllerBase
    {

		private readonly Services.IAuthorizationService _AuthorizationService;
		private readonly Services.IUsersService _UserService;

		public UsersController(Services.IAuthorizationService authService, Services.IUsersService userService)
        {
			_AuthorizationService = authService;
			_UserService = userService;
        }

		// GET: /users/{id}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromHeader]string authorization, [FromRoute]int id)
		{
			try
			{

				// Return requested user
				var res = await _userService.GetById(id);
				return Ok(res);
			}
			catch
			{
				return BadRequest();
			}
		}

		// GET: /users
		[HttpGet]
		public async Task<IActionResult> GetAll([FromHeader]string authorization)
		{
			try
			{
				// Get id from token
				var tokenId = _authHelper.GetUserIdFromAuthorizationHeader(authorization);

				// Get requesting user
				var user = await _userService.GetById(tokenId);
				
				if (!user.Role.Special)
				{
					return Unauthorized();
				}

				// Return requested users
				var users = await _userService.GetAll();
				var res = users.Select(x => new Views.User(x));
				return Ok(res);
			}
			catch
			{
				return BadRequest();
			}
		}

	}
}