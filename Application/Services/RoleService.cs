using Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Services
{
	public interface IRoleService
	{
		Task<IEnumerable<ViewModels.Role>> GetAll(string authorization);
	}

	public class RoleService : IRoleService
	{
		private readonly Helpers.AppSettings _AppSettings;
		private readonly Data.ApplicationContext _Context;

		public RoleService(IOptions<Helpers.AppSettings> appSettings, Data.ApplicationContext context)
		{
			_AppSettings = appSettings.Value;
			_Context = context;
		}

		public async Task<IEnumerable<ViewModels.Role>> GetAll(string authorization)
		{
			try
			{
				if (_AuthHelper.TryParse(authorization, out IDictionary<string, string> dict))
				{
					if (dict.TryGetValue(ClaimTypes.Name, out string userId)
					&& dict.TryGetValue(ClaimTypes.Role, out string userRole))
					{
						var user = await _Context.Users
							.Include(rle => rle.Role)
							.SingleOrDefaultAsync(usr =>
								usr.Id.Equals(int.Parse(userId))
								&& usr.Role.Name.Equals("admin"));
						
						if (user == null)
						{
							throw new NotAllowedException($"userId: {userId} with role {userRole} is not allowed");
						}

						var roles = await _Context.Roles.ToListAsync();
						return roles.Select(rle => new ViewModels.Role(rle));
					}
				}
				throw new JwtTokenException($"Could not parse jwt-token");
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}
