using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Application.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Produces("application/json")]
	public class LanguagesController : ControllerBase
	{
		
		private readonly Services.ILanguageService _LanguageService;

		public LanguagesController(Services.ILanguageService languageService)
		{
			_LanguageService = languageService;
		}
		
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				var res = await _LanguageService.GetAll();
				return Ok(res);
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}