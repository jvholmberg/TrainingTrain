using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MsvcUser.Helpers
{

	public interface IUserHelper
	{
	}

	public class UserHelper : IUserHelper
	{
		private readonly AppSettings _AppSettings;

		public UserHelper(AppSettings appSettings)
		{
			_AppSettings = appSettings;
		}
		
	}
}
