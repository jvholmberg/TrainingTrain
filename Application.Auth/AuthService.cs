using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Services
{
	public interface IAuthService
	{
		Task<Views.Response.Login> Authenticate(string username, string password);
		Task<Views.Response.Refresh> Authenticate(int userId, string userRole, string refreshToken);
		Task<bool> Destroy(string userId, string id);
	}
	public class AuthService : IAuthService
	{
		private readonly Context.AuthContext _Context;
		private readonly Helpers.AuthHelper _AuthHelper;

		public AuthService(Context.AuthContext context)
		{
			_Context = context;
			_AuthHelper = new Helpers.AuthHelper();
		}

		public async Task<Views.Response.Login> Authenticate(string username, string password)
		{
			try
			{
				// Check if data was provided
				if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
				{
					return new Views.Response.Login { Message = "empty_username_or_password" };
				}

                // Get entity from db
                var user = await _Context.User
                    .Include(usr => usr.Role)
                    .SingleOrDefaultAsync(e => e.Username.Equals(username));
                
				// Check if any entity was found
				if (user == null)
				{
					return new Views.Response.Login { Message = "no_entity_found" };
				}

				// Check if passwords match
				if (!user.Password.Equals(password))
				{
					return new Views.Response.Login { Message = "incorrect_password" };
				}

				// Create access token
				var accessToken = _AuthHelper.CreateAccessToken(user.Id, user.Role.Name, out DateTime expiry);

				// Create refresh token
				var refreshToken = _AuthHelper.CreateRefreshToken();

				// Update entity with new refresh token
				user.RefreshToken = refreshToken;

				// Save updated entity
				await _Context.SaveChangesAsync();

				return new Views.Response.Login
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

		public async Task<Views.Response.Refresh> Authenticate(int userId, string userRole, string refreshToken)
		{
			try
			{
				// Get entity from db
				var user = await _Context.User.FindAsync(userId);

				// No entity was found
				if (user == null)
				{
					return new Views.Response.Refresh { Message = "no_entity_found" };
				}

				// Check if refresh tokens match
				if (user.RefreshToken != refreshToken)
				{
					return new Views.Response.Refresh { Message = "invalid_token" };
				}

				// Create access token
				var accessToken = _AuthHelper.CreateAccessToken(userId, userRole, out DateTime expiry);

				// Create refresh token
				var newRefreshToken = _AuthHelper.CreateRefreshToken();

				// Update entity with new refresh token
				user.RefreshToken = newRefreshToken;

				// Save updated entity
				await _Context.SaveChangesAsync();

				return new Views.Response.Refresh
				{
					AccessToken = accessToken,
					RefreshToken = newRefreshToken,
					Expiry = expiry.ToString(),
				};
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public async Task<bool> Destroy(string userId, string id)
		{
			try
			{
				if (userId.Equals(id))
				{
					return false;
				}

				// Get entity from db

				int.TryParse(userId, out int parsedUserId);
				var user = await _Context.User.FindAsync(parsedUserId);

				// Remove refresh token from entity
				user.RefreshToken = null;

				// Save updated entity
				await _Context.SaveChangesAsync();

				return true;

			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}
