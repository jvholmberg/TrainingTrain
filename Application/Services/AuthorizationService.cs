using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
	public interface IAuthorizationService
	{
		Task<Views.Authorization.AuthenticateResponse> Authenticate(string username, string password);
		Task<Views.Authorization.AuthenticateResponse> Authenticate(string authorization);
	}

	public class AuthorizationService : IAuthorizationService
	{
		private readonly Helpers.AppSettings _AppSettings;
		private readonly Helpers.IAuthorizationHelper _AuthorizationHelper;
		private readonly Data.ApplicationContext _Context;

		public AuthorizationService(IOptions<Helpers.AppSettings> appSettings, Data.ApplicationContext context)
		{
			_AppSettings = appSettings.Value;
			_AuthorizationHelper = new Helpers.AuthorizationHelper();
			_Context = context;
		}

		public async Task<Views.Authorization.AuthenticateResponse> Authenticate(string authorization)
		{
			try
			{
				if (_AuthorizationHelper.TryParse(authorization, out IDictionary<string, string> dict))
				{
					string tokenType;
					string userId;
					string userRole;

					if (dict.TryGetValue(Helpers.AuthorizationClaim.TokenType, out tokenType)
					&& dict.TryGetValue(Helpers.AuthorizationClaim.UserId, out userId)
					&& dict.TryGetValue(Helpers.AuthorizationClaim.UserRole, out userRole))
					{
						var id = int.Parse(userId);
						var user = await _Context.Users.FindAsync(id);

						if (tokenType.Equals("refresh_token"))
						{
							// Get settings from config
							var secret = _AppSettings.Secret;

							// create new access token
							var accessTokenExpiry = DateTime.UtcNow.AddDays(3);
							var accessToken = _AuthorizationHelper.CreateToken("access_token", id, userRole, secret, accessTokenExpiry);

							// Create new refresh token
							var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);
							var refreshToken = _AuthorizationHelper.CreateToken("refresh_token", id, userRole, secret, refreshTokenExpiry);

							// Update entity with new tokens
							user.AccessToken = accessToken;
							user.RefreshToken = refreshToken;

							// Save updated entity
							await _Context.SaveChangesAsync();

							// Return created tokens
							return new Views.Authorization.AuthenticateResponse
							{
								AccessToken = accessToken,
								RefreshToken = refreshToken,
							};
						}
						else
						{
							return new Views.Authorization.AuthenticateResponse
							{
								AccessToken = user.AccessToken,
								RefreshToken = user.RefreshToken,
							};
						}
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

		public async Task<Views.Authorization.AuthenticateResponse> Authenticate(string username, string password)
		{
			try
			{
				var user = _Context.Users.SingleOrDefault(x => x.Username.Equals(username));

				// Check if passwords match
				if (!user.Password.Equals(password))
				{
					throw new Exception();
				}

				// Get settings from config
				var secret = _AppSettings.Secret;

				// create new access token
				var accessTokenExpiry = DateTime.UtcNow.AddDays(3);
				var accessToken = _AuthorizationHelper.CreateToken("access_token", user.Id, user.Role.Name, secret, accessTokenExpiry);

				// Create new refresh token
				var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);
				var refreshToken = _AuthorizationHelper.CreateToken("refresh_token", user.Id, user.Role.Name, secret, refreshTokenExpiry);

				// Update entity with new tokens
				user.AccessToken = accessToken;
				user.RefreshToken = refreshToken;

				// Save updated entity
				await _Context.SaveChangesAsync();

				// Return created tokens
				return new Views.Authorization.AuthenticateResponse
				{
					AccessToken = accessToken,
					RefreshToken = refreshToken,
				};
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}
