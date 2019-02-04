using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MsvcAuth.Services
{
	public interface IAuthService
	{
		Task<Views.MsvcAuthResponseBody> Authenticate(string username, string password);
		Task<Views.MsvcAuthResponseBody> Authenticate(int msvcAuthUserId, string msvcAuthUserRole, string refreshToken);
		Task<bool> Destroy(string msvcAuthUserId, string id);
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

		public async Task<Views.MsvcAuthResponseBody> Authenticate(string username, string password)
		{
			try
			{
				// Check if data was provided
				if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
				{
					return new Views.MsvcAuthResponseBody { Message = "empty_username_or_password" };
				}

                // Get entity from db
                var user = await _Context.User
                    .Include(usr => usr.Role)
                    .SingleOrDefaultAsync(e => e.Username.Equals(username));
                
				// Check if any entity was found
				if (user == null)
				{
					return new Views.MsvcAuthResponseBody { Message = "no_entity_found" };
				}

				// Check if passwords match
				if (!user.Password.Equals(password))
				{
					return new Views.MsvcAuthResponseBody { Message = "incorrect_password" };
				}

				// Create access token
				var accessToken = _AuthHelper.CreateAccessToken(user.Id, user.Role.Name, out DateTime expiry);

				// Create refresh token
				var refreshToken = _AuthHelper.CreateRefreshToken();

				// Update entity with new refresh token
				user.RefreshToken = refreshToken;

				// Save updated entity
				await _Context.SaveChangesAsync();

				return new Views.MsvcAuthResponseBody
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

		public async Task<Views.MsvcAuthResponseBody> Authenticate(int msvcAuthUserId, string msvcAuthUserRole, string refreshToken)
		{
			try
			{
				// Get entity from db
				var user = await _Context.User.FindAsync(msvcAuthUserId);

				// No entity was found
				if (user == null)
				{
					return new Views.MsvcAuthResponseBody { Message = "no_entity_found" };
				}

				// Check if refresh tokens match
				if (user.RefreshToken != refreshToken)
				{
					return new Views.MsvcAuthResponseBody { Message = "invalid_token" };
				}

				// Create access token
				var accessToken = _AuthHelper.CreateAccessToken(msvcAuthUserId, msvcAuthUserRole, out DateTime expiry);

				// Create refresh token
				var newRefreshToken = _AuthHelper.CreateRefreshToken();

				// Update entity with new refresh token
				user.RefreshToken = newRefreshToken;

				// Save updated entity
				await _Context.SaveChangesAsync();

				return new Views.MsvcAuthResponseBody
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

		public async Task<bool> Destroy(string msvcAuthUserId, string id)
		{
			try
			{
				if (msvcAuthUserId.Equals(id))
				{
					return false;
				}

				// Get entity from db

				int.TryParse(msvcAuthUserId, out int parsedMsvcAuthUserId);
				var user = await _Context.User.FindAsync(parsedMsvcAuthUserId);

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
