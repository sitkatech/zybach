using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects.User;

namespace Zybach.API.Services.Authorization
{
    public class LoggedInUnclassifiedFeature : AuthorizeAttribute, IAuthorizationFilter
    {
        public LoggedInUnclassifiedFeature() : base()
        {
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

        }
    }
}