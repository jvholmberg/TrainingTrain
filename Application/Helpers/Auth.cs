using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Helpers
{
	public interface IAuth
	{
		int GetUserIdFromAuthorizationHeader(string authorization);
		string GetTokenFromAuthorizationHeader(string authorization);
		string CreateToken(int id, string secret, out DateTime validUntil);
	}

	public class Auth : IAuth
	{
		public int GetUserIdFromAuthorizationHeader(string authorization)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			
			// Get payload
			var encoded = GetTokenFromAuthorizationHeader(authorization);
			var decoded = tokenHandler.ReadJwtToken(encoded);
			var payload = decoded.Payload;
			var uniqueName = payload.GetValueOrDefault("unique_name") as string;

			// Return id
			if (int.TryParse(uniqueName, out int id))
			{
				return id;
			}
			throw new Exception();
		}

		public string GetTokenFromAuthorizationHeader(string authorization)
		{
			var token = authorization.Replace("Bearer ", "");
			return token;
		}

		public string CreateToken(int id, string secret, out DateTime validUntil)
		{
			var tokenHandler = new JwtSecurityTokenHandler();

			validUntil = DateTime.UtcNow.AddDays(3);

			var key = Encoding.ASCII.GetBytes(secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, id.ToString())
				}),
				Expires = validUntil,
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var securitytoken = tokenHandler.CreateToken(tokenDescriptor);
			var token = tokenHandler.WriteToken(securitytoken);

			return token;
		}
	}
}
