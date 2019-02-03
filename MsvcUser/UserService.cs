using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MsvcUser.Services
{
	public interface IUserService
    {

    }

	public class UserService : IUserService
	{
		private readonly Context.UserContext _Context;
		private readonly Helpers.UserHelper _UserHelper;

		public UserService(Context.UserContext context)
		{
			_Context = context;
			_UserHelper = new Helpers.UserHelper()
        }

        public async Task<Views.User> GetById(int id)
        {
            try
            {
                // Get entity from db
                var user = await _Context.Users
                    .Include(usr => usr.Language)
                    .SingleOrDefaultAsync(usr => usr.Id.Equals(id));

                // No entity was found
                if (user == null)
                {
                    return null;
                }

                return new Views.User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,

                    
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Views.User> GetAll(int id)
        {

        }

        public async Task<Views.User> Create(int id)
        {

        }

        public async Task<Views.User> Update(int id)
        {

        }


    }
}
