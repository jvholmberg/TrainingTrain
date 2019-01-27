using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Services
{
	public interface IAuthService
	{
		Task<Views.Auth.AuthenticateResponse> Authenticate(string username, string password);
		Task<Views.Auth.AuthenticateResponse> Authenticate(string refreshToken);
		Task<bool> Destroy(string authorization, int id);
	}

	public class AuthService : IAuthService
	{
		private readonly Helpers.IAuthHelper _AuthHelper;
		private readonly Data.ApplicationContext _Context;

		public AuthService(IOptions<Helpers.AppSettings> appSettings, Data.ApplicationContext context)
		{
			_AuthHelper = new Helpers.AuthHelper(appSettings.Value);
			_Context = context;
		}

		public async Task<Views.Auth.AuthenticateResponse> Authenticate(string token)
		{
			try
			{
				// null value is not valid
				if (token.Equals(null))
				{
					throw new Exception();
				}

				// Get user from token
				var user = _Context.Users
					.Include(usr => usr.Role)
					.SingleOrDefault(usr => usr.RefreshToken.Equals(token));

				// Token did not match any user
				if (user == null)
				{
					throw new Exception();
				}

				return await CreateTokensForUser(user);
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<Views.Auth.AuthenticateResponse> Authenticate(string username, string password)
		{
			try
			{
				var user = _Context.Users
					.Include(usr => usr.Role)
					.SingleOrDefault(usr => usr.Username.Equals(username));

				// Check if passwords match
				if (!user.Password.Equals(password))
				{
					throw new Exception();
				}

				return await CreateTokensForUser(user);
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<bool> Destroy(string authorization, int id)
		{
			try
			{
				// Get payload from access token
				if (_AuthHelper.TryParse(authorization, out IDictionary<string, string> dict))
				{
					// Get userId
					if (dict.TryGetValue(ClaimTypes.Name, out string userIdAsString))
					{
						// Parse userId as int
						var userId = int.Parse(userIdAsString);

						// Token id and requested id differ, terminate
						if (!id.Equals(userId))
						{
							return false;
						}

						// Get user
						var user = await _Context.Users.FindAsync(userId);

						// Remove refresh token
						user.RefreshToken = null;

						// Save updated user
						await _Context.SaveChangesAsync();
						
						return true;
					}
				}
				return false;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		private async Task<Views.Auth.AuthenticateResponse> CreateTokensForUser(Entities.User user)
		{
			try
			{
				// Get expiry
				var expiry = DateTime.UtcNow.AddHours(1);

				// Create new access token
				var accessToken = _AuthHelper.CreateToken(user.Id, user.Role.Name, expiry);

				// Create new refresh token
				var refreshToken = _AuthHelper.CreateRefreshToken();

				// Update user with new refresh token
				user.RefreshToken = refreshToken;

				// Save updated user
				await _Context.SaveChangesAsync();

				// Return created tokens and expiry
				return new Views.Auth.AuthenticateResponse
				{
					AccessToken = accessToken,
					RefreshToken = refreshToken,
					Expiry = expiry.ToString(),
				};
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}
