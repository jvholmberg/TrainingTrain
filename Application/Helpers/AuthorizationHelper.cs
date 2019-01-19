using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Application.Helpers
{
	public class AuthorizationClaim
	{
		public static string TokenType = "token_type";
		public static string UserId = "user_id";
		public static string UserRole = "user_role";
	}

	public interface IAuthorizationHelper
	{
		JwtSecurityToken Deserialize(string authorization);
		bool TryParse(string authorization, out IDictionary<string, string> dict);
		bool TryParse(JwtSecurityToken token, out IDictionary<string, string> dict);
		string CreateToken(string type, int id, string role, string secret, DateTime expiry);
	}

	public class AuthorizationHelper : IAuthorizationHelper
	{
		private readonly JwtSecurityTokenHandler _TokenHandler;

		public AuthorizationHelper()
		{
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
				var tokenType = payload.GetValueOrDefault(AuthorizationClaim.TokenType) as string;
				var userId = payload.GetValueOrDefault(AuthorizationClaim.UserId) as string;
				var userRole = payload.GetValueOrDefault(AuthorizationClaim.UserRole) as string;

				dict = new Dictionary<string, string>();
				dict.Add(AuthorizationClaim.TokenType, tokenType);
				dict.Add(AuthorizationClaim.UserId, userId);
				dict.Add(AuthorizationClaim.UserRole, userRole);
				return true;
			}
			catch
			{
				dict = null;
				return false;
			}
		}
			
		public string CreateToken(string type, int id, string role, string secret, DateTime expiry)
		{
			try
			{
				var securityKey = Encoding.ASCII.GetBytes(secret);
				var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256Signature);
				var tokenDescriptor = new SecurityTokenDescriptor
				{
					Subject = new ClaimsIdentity(new Claim[]
					{
						new Claim(AuthorizationClaim.TokenType, type),
						new Claim(AuthorizationClaim.UserId, id.ToString()),
						new Claim(AuthorizationClaim.UserRole, role),
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
	}
}
