using System;
using System.Linq;
using System.Threading.Tasks;

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
		private readonly string _Secret;

		public AuthService(Context.AuthContext context, string secret)
		{
			_Context = context;
			_AuthHelper = new Helpers.AuthHelper();
			_Secret = secret;
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
				var user = _Context.Auth.SingleOrDefault(e => e.Username.Equals(username));

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
				var accessToken = _AuthHelper.CreateAccessToken(user.Id, user.Role, _Secret, out DateTime expiry);

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
				var entity = await _Context.Auth.FindAsync(msvcAuthUserId);

				// No entity was found
				if (entity == null)
				{
					return new Views.MsvcAuthResponseBody { Message = "no_entity_found" };
				}

				// Check if refresh tokens match
				if (entity.RefreshToken != refreshToken)
				{
					return new Views.MsvcAuthResponseBody { Message = "invalid_token" };
				}

				// Create access token
				var accessToken = _AuthHelper.CreateAccessToken(msvcAuthUserId, msvcAuthUserRole, _Secret, out DateTime expiry);

				// Create refresh token
				var newRefreshToken = _AuthHelper.CreateRefreshToken();

				// Update entity with new refresh token
				entity.RefreshToken = newRefreshToken;

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
				var entity = await _Context.Auth.FindAsync(parsedMsvcAuthUserId);

				// Remove refresh token from entity
				entity.RefreshToken = null;

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
