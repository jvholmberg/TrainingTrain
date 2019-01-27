using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Services
{
	public interface IUserService
	{
		Task<Views.Users.User> GetById(string authorization, int id);
		Task<IEnumerable<Views.Users.User>> GetAll(string authorization);
	}

	public class UserService : IUserService
	{
		private readonly Helpers.AppSettings _AppSettings;
		private readonly Data.ApplicationContext _Context;
		private readonly Helpers.AuthHelper _AuthHelper;

		public UserService(IOptions<Helpers.AppSettings> appSettings, Data.ApplicationContext context)
		{
			_AppSettings = appSettings.Value;
			_Context = context;
			_AuthHelper = new Helpers.AuthHelper(appSettings.Value);
		}
		
		public async Task<Views.Users.User> GetById(string authorization, int id)
		{
			try
			{
				if (_AuthHelper.TryParse(authorization, out IDictionary<string, string> dict))
				{

					if (dict.TryGetValue(ClaimTypes.Name, out string userId)
					&& dict.TryGetValue(ClaimTypes.Role, out string userRole))
					{
						if (!userRole.Equals("admin") && !userId.Equals(id))
						{
							throw new Exception();
						}

						var requestedUser = await _Context.Users
							.Include(usr => usr.Role)
							.Include(usr => usr.Language)
							.SingleOrDefaultAsync(x => x.Id.Equals(id));

						return new Views.Users.User(requestedUser);
					}
					else
					{
						throw new Exception();
					}
				}
				else
				{
					throw new Exception();
				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<IEnumerable<Views.Users.User>> GetAll(string authorization)
		{
			try
			{
				if (_AuthHelper.TryParse(authorization, out IDictionary<string, string> dict))
				{
					if (dict.TryGetValue(ClaimTypes.Name, out string userId)
					&& dict.TryGetValue(ClaimTypes.Role, out string userRole))
					{
						var user = await _Context.Users.FindAsync(userId);

						if (!user.Role.Name.Equals("admin"))
						{
							throw new Exception();
						}

						var requestedUsers = await _Context.Users
							.Include(usr => usr.Role)
							.Include(usr => usr.Language)
							.ToListAsync();

						return requestedUsers.Select(usr => new Views.Users.User(usr));
					}
					else
					{
						throw new Exception();
					}
				}
				else
				{
					throw new Exception();
				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}
