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
		Task<ViewModels.User> GetById(string authorization, int id);
		Task<IEnumerable<ViewModels.User>> GetAll(string authorization);
		Task<Views.Users.CreateResponse> Create(Views.Users.CreateRequest req);
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
		
		public async Task<ViewModels.User> GetById(string authorization, int id)
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

						return new ViewModels.User(requestedUser);
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

		public async Task<IEnumerable<ViewModels.User>> GetAll(string authorization)
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

						return requestedUsers.Select(usr => new ViewModels.User(usr));
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

		public async Task<Views.Users.CreateResponse> Create(Views.Users.CreateRequest req)
		{
			try
			{
				// Check if username is already taken
				var existingUser = await GetByUsername(req.Username);
				var errors = new List<string>();

				// Add errors to list
				if (existingUser != null)
				{
					errors.Add("err_username");
				}
				if (!req.Password.Equals(req.PasswordVerify))
				{
					errors.Add("err_password");
				}

				// If no errors create new user
				if (errors.Count() == 0)
				{
					var role = await _Context.Roles.SingleOrDefaultAsync(rle => rle.Name == "user");
					var lang = await _Context.Language.SingleOrDefaultAsync(lng => lng.Code == "en-EN");
					var now = DateTime.UtcNow;

					// Create new user
					var user = await _Context.Users.AddAsync(new Entities.User
					{
						Activated = false,
						Username = req.Username,
						Password = req.Password,
						Role = role,
						Language = lang,
						CreatedAt = now,
					});

					// Save user
					await _Context.SaveChangesAsync();
				}

				return new Views.Users.CreateResponse
				{
					Username = req.Username,
					Errors = errors,
				};
			}
			catch (Exception e)
			{
				throw e;
			}
			
		}

		private async Task<Entities.User> GetByUsername(string username)
		{
			try
			{
				var user = await _Context.Users.SingleOrDefaultAsync(usr => usr.Username.Equals(username));
				return user;
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}
