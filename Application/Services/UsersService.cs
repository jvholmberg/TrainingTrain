using Application.Data;
using Application.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public interface IUsersService
	{
		Task<Views.User> GetByToken(string token);
		Task<Views.User> GetById(int id);
		Task<Views.User> GetByEmail(string email);
	}

	public class UsersService : IUsersService
	{
		private readonly AppSettings _appSettings;
		private readonly ApplicationContext _context;

		public UsersService(IOptions<AppSettings> appSettings, ApplicationContext context)
		{
			_appSettings = appSettings.Value;
			_context = context;
		}

		public async Task<Views.User> GetByToken(string token)
		{
			throw new NotImplementedException();
		}

		public async Task<Views.User> GetById(int id)
		{
			var user = await _context.Users
				.Include(usr => usr.Role)
				.Include(usr => usr.Language)
				.SingleOrDefaultAsync(x => x.Id == id);

			if (user == null)
			{
				throw new Exception();
			}

			return new Views.User(user);
		}

		public async Task<Views.User> GetByEmail(string email)
		{
			throw new NotImplementedException();
		}
	}
}
