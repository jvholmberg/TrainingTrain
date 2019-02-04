using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MsvcUser.Controllers
{
	[Authorize]
	[ApiController]
	[Route("users")]
	[Produces("application/json")]
    public class UsersController : ControllerBase
    {
		
		private readonly Services.IUserService _UserService;

		public UsersController(Services.IUserService userService)
        {
			_UserService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetByHeader([FromHeader]Views.Request.Headers headers)
        {
            try
            {
                // Return requested user
                var res = await _UserService.GetById(headers.UserId);
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
		public async Task<IActionResult> GetAll([FromHeader]Views.Request.Headers headers)
		{
			try
			{
				// Return all users
				var res = await _UserService.GetAll(headers.UserId, headers.UserRole);
				return Ok(res);
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
		}

		//[AllowAnonymous]
		//[HttpPost]
		//public async Task<IActionResult> Create([FromBody]Views.Users.CreateRequest req)
		//{
		//	try
		//	{
		//		var res = await _UserService.Create(req);
		//		return Ok(res);
		//	}
		//	catch
		//	{
		//		return BadRequest();
		//	}
		//}

	}
}