using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
	public interface IUsersService
	{
		Task<Views.Users.User> GetById(string authorization, int id);
		Task<IEnumerable<Views.Users.User>> GetAll(string authorization);
	}

	public class UsersService : IUsersService
	{
		private readonly Helpers.AppSettings _AppSettings;
		private readonly Data.ApplicationContext _Context;
		private readonly Helpers.AuthorizationHelper _AuthorizationHelper;

		public UsersService(IOptions<Helpers.AppSettings> appSettings, Data.ApplicationContext context)
		{
			_AppSettings = appSettings.Value;
			_Context = context;
			_AuthorizationHelper = new Helpers.AuthorizationHelper();
		}
		
		public async Task<Views.Users.User> GetById(string authorization, int id)
		{
			try
			{
				if (_AuthorizationHelper.TryParse(authorization, out IDictionary<string, string> dict))
				{
					string userId;
					string userRole;

					if (dict.TryGetValue(Helpers.AuthorizationClaim.UserId, out userId)
					&& dict.TryGetValue(Helpers.AuthorizationClaim.UserRole, out userRole))
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
				if (_AuthorizationHelper.TryParse(authorization, out IDictionary<string, string> dict))
				{
					string userId;
					string userRole;

					if (dict.TryGetValue(Helpers.AuthorizationClaim.UserId, out userId)
					&& dict.TryGetValue(Helpers.AuthorizationClaim.UserRole, out userRole))
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
