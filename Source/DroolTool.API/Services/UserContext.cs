using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using DroolTool.EFModels.Entities;
using DroolTool.Models.DataTransferObjects.User;

namespace DroolTool.API.Services
{
    public class UserContext
    {
        public UserDto User { get; set; }

        private UserContext(UserDto user)
        {
            User = user;
        }

        public static UserDto GetUserFromHttpContext(DroolToolDbContext dbContext, HttpContext httpContext)
        {

            var claimsPrincipal = httpContext.User;
            if (!claimsPrincipal.Claims.Any())
            {
                return null;
            }

            var userGuid = Guid.Parse(claimsPrincipal.Claims.Single(c => c.Type == "sub").Value);
            var keystoneUser = DroolTool.EFModels.Entities.User.GetByUserGuid(dbContext, userGuid);
            return keystoneUser;
        }
    }
}