using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MsvcAuth.Services
{
	public interface IMsvcAuthService
	{
		Task<Views.MsvcAuthResponseBody> Authenticate(string username, string password);
		Task<Views.MsvcAuthResponseBody> Authenticate(string refreshToken);
		Task<Views.MsvcAuthResponseBody> Destroy(string authorization, int id);
	}
	public class MsvcAuthService : IMsvcAuthService
	{
		private readonly Context.AuthContext _Context;
		private readonly JwtSecurityTokenHandler _TokenHandler;

		public MsvcAuthService(Context.AuthContext context)
		{
			_Context = context;
			_TokenHandler = new JwtSecurityTokenHandler();
		}

		public async Task<Views.MsvcAuthResponseBody> Authenticate(string username, string password, string secret)
		{
			try
			{
				// Check if data was provided
				if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
				{
					return new Views.MsvcAuthResponseBody { Message = "empty_username_or_password" };
				}

				// Get user from db
				var user = _Context.Users.SingleOrDefault(usr => usr.Username.Equals(username));

				// Check if any user was found
				if (user == null)
				{
					return new Views.MsvcAuthResponseBody { Message = "no_user_found" };
				}

				// Check if passwords match
				if (!user.Password.Equals(password))
				{
					return new Views.MsvcAuthResponseBody { Message = "incorrect_password" };
				}

				// Create refresh token
				var securityKey = Encoding.ASCII.GetBytes(secret);
				var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256Signature);
				var tokenDescriptor = new SecurityTokenDescriptor
				{
					Subject = new ClaimsIdentity(new Claim[]
					{
						new Claim(ClaimTypes.Name, user.Id.ToString()),
						new Claim(ClaimTypes.Role, user.Role),
					}),
					Expires = expiry,
					SigningCredentials = signingCredentials
				};
				var securitytoken = _TokenHandler.CreateToken(tokenDescriptor);
				var token = _TokenHandler.WriteToken(securitytoken);

				return new Views.MsvcAuthResponseBody
				{
					AccessToken = "",
					RefreshToken = "",
					Expiry = "",
				};
				

			}
			catch (Exception e)
			{

				throw;
			}
		}

		public async Task<Views.MsvcAuthResponseBody> Authenticate(string refreshToken)
		{
			throw new NotImplementedException();
		}

		public async Task<Views.MsvcAuthResponseBody> Destroy(string authorization, int id)
		{
			throw new NotImplementedException();
		}
	}
}
