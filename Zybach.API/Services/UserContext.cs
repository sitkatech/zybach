using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Services
{
    public class UserContext
    {
        public UserDto User { get; set; }

        private UserContext(UserDto user)
        {
            User = user;
        }

        public static UserDto GetUserFromHttpContext(ZybachDbContext dbContext, HttpContext httpContext)
        {

            var claimsPrincipal = httpContext.User;
            if (!claimsPrincipal.Claims.Any())
            {
                return null;
            }

            var userGuid = Guid.Parse(claimsPrincipal.Claims.Single(c => c.Type == "sub").Value);
            var keystoneUser = Users.GetByUserGuid(dbContext, userGuid);
            return keystoneUser;
        }
    }
}