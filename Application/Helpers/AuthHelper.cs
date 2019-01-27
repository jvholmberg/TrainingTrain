using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Helpers
{

	public interface IAuthHelper
	{
		JwtSecurityToken Deserialize(string authorization);
		bool TryParse(string authorization, out IDictionary<string, string> dict);
		bool TryParse(JwtSecurityToken token, out IDictionary<string, string> dict);
		string CreateToken(int id, string role, DateTime expiry);
		string CreateRefreshToken();
	}

	public class AuthHelper : IAuthHelper
	{
		private readonly AppSettings _AppSettings;
		private readonly JwtSecurityTokenHandler _TokenHandler;

		public AuthHelper(AppSettings appSettings)
		{

			_AppSettings = appSettings;
			_TokenHandler = new JwtSecurityTokenHandler();
		}

		public JwtSecurityToken Deserialize(string authorization)
		{
			try
			{
				var token = authorization.Replace("Bearer ", "");
				var deserialized = _TokenHandler.ReadJwtToken(token);
				return deserialized;
			}
			catch
			{
				throw new Exception();
			}
		}

		public bool TryParse(string authorization, out IDictionary<string, string> dict)
		{
			try
			{
				var deserializedObj = Deserialize(authorization);
				if (TryParse(deserializedObj, out dict))
				{
					return true;
				}
				else
				{
					dict = null;
					return false;
				}
			}
			catch
			{
				dict = null;
				return false;
			}
		}

		public bool TryParse(JwtSecurityToken token, out IDictionary<string, string> dict)
		{
			try
			{
				var payload = token.Payload;
				var userId = payload.GetValueOrDefault("unique_name") as string;
				var userRole = payload.GetValueOrDefault("role") as string;

				dict = new Dictionary<string, string>();
				dict.Add(ClaimTypes.Name, userId);
				dict.Add(ClaimTypes.Role, userRole);
				return true;
			}
			catch
			{
				dict = null;
				return false;
			}
		}
			
		public string CreateToken(int id, string role, DateTime expiry)
		{
			try
			{
				// Get secret from config
				var secret = _AppSettings.Secret;

				var securityKey = Encoding.ASCII.GetBytes(secret);
				var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256Signature);
				var tokenDescriptor = new SecurityTokenDescriptor
				{
					Subject = new ClaimsIdentity(new Claim[]
					{
						new Claim(ClaimTypes.Name, id.ToString()),
						new Claim(ClaimTypes.Role, role),
					}),
					Expires = expiry,
					SigningCredentials = signingCredentials
				};
				var securitytoken = _TokenHandler.CreateToken(tokenDescriptor);
				var token = _TokenHandler.WriteToken(securitytoken);
				return token;
			}
			catch
			{
				throw new Exception();
			}
		}
		public string CreateRefreshToken()
		{
			try
			{
				var guid = Guid.NewGuid().ToString();
				return guid;
			}
			catch
			{
				throw new Exception();
			}
		}
	}
}
