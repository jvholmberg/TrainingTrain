using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Services
{
	public interface ILanguageService
	{
		Task<IEnumerable<ViewModels.Language>> GetAll();
	}

	public class LanguageService : ILanguageService
	{
		private readonly Data.ApplicationContext _Context;

		public LanguageService(IOptions<Helpers.AppSettings> appSettings, Data.ApplicationContext context)
		{
			_Context = context;
		}
		
		public async Task<IEnumerable<ViewModels.Language>> GetAll()
		{
			try
			{
				var languages = await _Context.Language.ToArrayAsync();
				return languages.Select(lng => new ViewModels.Language(lng));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}
