using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
	public interface IAuthService
	{
		Task<Views.LogoutResponse> Logout(string authorization);
		Task<Views.AuthenticateResponse> Authenticate(string authorization);
		Task<Views.RenewResponse> Renew(string authorization);
		Task<Views.LoginResponse> Login(Views.LoginRequest input);
		Task<Views.RegisterResponse> Register(Views.RegisterRequest input);
	}

	public class AuthService : IAuthService
	{
		private readonly Helpers.AppSettings _appSettings;
		private readonly Helpers.IAuth _authHelper;
		private readonly Data.ApplicationContext _context;

		public AuthService(IOptions<Helpers.AppSettings> appSettings, Data.ApplicationContext context)
		{
			_appSettings = appSettings.Value;
			_authHelper = new Helpers.Auth();
			_context = context;
		}

		public async Task<Views.LogoutResponse> Logout(string authorization)
		{
			throw new NotImplementedException();
		}

		public async Task<Views.AuthenticateResponse> Authenticate(string authorization)
		{
			var tokenId = _authHelper.GetUserIdFromAuthorizationHeader(authorization);
			var token = _authHelper.GetTokenFromAuthorizationHeader(authorization);

			var user = await _context.Users.FindAsync(tokenId);
			if (user.Token != token)
			{
				throw new Exception();
			}

			return new Views.AuthenticateResponse
			{
				Email = user.Email,
				Token = user.Token,
				TokenExpires = user.TokenExpires
			};
		}

		public async Task<Views.RenewResponse> Renew(string authorization)
		{
			var currentTokenId = _authHelper.GetUserIdFromAuthorizationHeader(authorization);
			var currentToken = _authHelper.GetTokenFromAuthorizationHeader(authorization);
			
			var user = await _context.Users.FindAsync(currentTokenId);
			if (user.Token != currentToken)
			{
				throw new Exception();
			}
			// Create new token
			DateTime validUntil;
			var token = _authHelper.CreateToken(user.Id, _appSettings.Secret, out validUntil);

			// Save token on user entity
			user.Token = token;
			user.TokenExpires = validUntil;
			await _context.SaveChangesAsync();

			return new Views.RenewResponse
			{
				Email = user.Email,
				Token = user.Token,
				TokenExpires = validUntil
			};
		}

		public async Task<Views.LoginResponse> Login(Views.LoginRequest input)
		{
			// Check if data has been provided
			if (string.IsNullOrEmpty(input.Email) || string.IsNullOrEmpty(input.Password))
			{
				throw new Exception();
			}

			// Find user in context
			var user = _context.Users.SingleOrDefault(x => x.Email == input.Email);

			// Check if user exists
			if (user == null)
			{
				throw new Exception();
			}

			// Check if password is correct
			if (user.Password != input.Password)
			{
				throw new Exception();
			}

			// Create new token
			DateTime validUntil;
			var token = _authHelper.CreateToken(user.Id, _appSettings.Secret, out validUntil);

			// Save token on user entity
			user.Token = token;
			user.TokenExpires = validUntil;
			await _context.SaveChangesAsync();

			return new Views.LoginResponse
			{
				Email = user.Email,
				Token = user.Token,
				TokenExpires = validUntil
			};
		}

		public async Task<Views.RegisterResponse> Register(Views.RegisterRequest input)
		{
			// Check if data has been provided and that passwords match
			if (string.IsNullOrEmpty(input.Email) || string.IsNullOrEmpty(input.Password) || input.Password != input.PasswordVerify)
			{
				throw new Exception();
			}

			// Find user with email in context
			var existingUser = _context.Users.SingleOrDefault(x => x.Email == input.Email);

			// Check if existing user exists
			if (existingUser != null)
			{
				throw new Exception();
			}
			
			var newUser = new Entities.User
			{
				Email = input.Email,
				Password = input.Password,
				FirstName = input.FirstName,
				LastName = input.LastName,
				CreatedAt = DateTime.UtcNow,
				LastModified = DateTime.UtcNow,
				Activated = false,
			};
			await _context.AddAsync(newUser);
			await _context.SaveChangesAsync();

			return new Views.RegisterResponse
			{
				Email = newUser.Email
			};
		}

	}
}
