using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MsvcAuth.Helpers
{
	public class AuthHelper
	{
		private readonly JwtSecurityTokenHandler _TokenHandler;

		public AuthHelper()
		{
			_TokenHandler = new JwtSecurityTokenHandler();
		}

		public string CreateAccessToken(int userId, string userRole, string secret, out DateTime expiry)
		{
			var securityKey = Encoding.ASCII.GetBytes(secret);
			var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256Signature);
			expiry = DateTime.UtcNow.AddHours(1);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
						new Claim(ClaimTypes.Name, userId.ToString()),
						new Claim(ClaimTypes.Role, userRole),
				}),
				Expires = expiry,
				SigningCredentials = signingCredentials
			};
			var securitytoken = _TokenHandler.CreateToken(tokenDescriptor);
			var accessToken = _TokenHandler.WriteToken(securitytoken);

			return accessToken;
		}

		public string CreateRefreshToken()
		{
			var refreshToken = Guid.NewGuid().ToString();
			return refreshToken;
		}
	}
}
