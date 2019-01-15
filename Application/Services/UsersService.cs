using Application.Data;
using Application.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
	public interface IUsersService
	{
		Task<Entities.User> GetById(int id);
		Task<IEnumerable<Entities.User>> GetAll();
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
		
		public async Task<Entities.User> GetById(int id)
		{
			var user = await _context.Users
				.Include(usr => usr.Role)
				.Include(usr => usr.Language)
				.SingleOrDefaultAsync(x => x.Id == id);

			if (user == null)
			{
				throw new Exception();
			}

			return user;
		}

		public async Task<IEnumerable<Entities.User>> GetAll()
		{
			var users = await _context.Users
				.Include(usr => usr.Role)
				.Include(usr => usr.Language)
				.ToListAsync();
			return users;
		}
	}
}
