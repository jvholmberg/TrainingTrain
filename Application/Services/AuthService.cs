using Application.Data;
using Application.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
		private readonly AppSettings _appSettings;
		private readonly ApplicationContext _context;

		public AuthService(IOptions<AppSettings> appSettings, ApplicationContext context)
		{
			_appSettings = appSettings.Value;
			_context = context;
		}

		public async Task<Views.LogoutResponse> Logout(string authorization)
		{
			throw new NotImplementedException();
		}

		public async Task<Views.AuthenticateResponse> Authenticate(string authorization)
		{
			var tokenHandler = new JwtSecurityTokenHandler();

			var encoded = authorization.Replace("Bearer ", "");
			var decoded = tokenHandler.ReadJwtToken(encoded);
			var payload = decoded.Payload;
			var uniqueName = payload.GetValueOrDefault("unique_name") as string;
			
			if (int.TryParse(uniqueName, out int id)) 
			{
				var user = await _context.Users.FindAsync(id);
				if (user.Token == encoded)
				{
					return new Views.AuthenticateResponse
					{
						Email = user.Email,
						Token = user.Token,
						ValidUntil = user.TokenValid
					};
				}
			}
			throw new Exception();
		}

		public async Task<Views.RenewResponse> Renew(string authorization)
		{
			var tokenHandler = new JwtSecurityTokenHandler();

			var encoded = authorization.Replace("Bearer ", "");
			var decoded = tokenHandler.ReadJwtToken(encoded);
			var payload = decoded.Payload;
			var uniqueName = payload.GetValueOrDefault("unique_name") as string;

			if (int.TryParse(uniqueName, out int id))
			{
				var user = await _context.Users.FindAsync(id);
				if (user.Token == encoded)
				{
					var validUntil = DateTime.UtcNow.AddDays(3);
					var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
					var tokenDescriptor = new SecurityTokenDescriptor
					{
						Subject = new ClaimsIdentity(new Claim[]
						{
							new Claim(ClaimTypes.Name, user.Id.ToString())
						}),
						Expires = validUntil,
						SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
					};
					var token = tokenHandler.CreateToken(tokenDescriptor);
					user.Token = tokenHandler.WriteToken(token);
					user.TokenValid = validUntil;

					await _context.SaveChangesAsync();

					return new Views.RenewResponse
					{
						Email = user.Email,
						Token = user.Token,
						ValidUntil = validUntil
					};
				}
			}
			throw new Exception();
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

			var tokenHandler = new JwtSecurityTokenHandler();
			var validUntil = DateTime.UtcNow.AddDays(3);
			var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.Id.ToString())
				}),
				Expires = validUntil,
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			user.Token = tokenHandler.WriteToken(token);
			user.TokenValid = validUntil;

			await _context.SaveChangesAsync();

			return new Views.LoginResponse
			{
				Email = user.Email,
				Token = user.Token,
				ValidUntil = validUntil
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
