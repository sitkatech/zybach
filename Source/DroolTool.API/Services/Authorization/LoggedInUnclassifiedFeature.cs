using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using DroolTool.EFModels.Entities;
using DroolTool.Models.DataTransferObjects.User;

namespace DroolTool.API.Services.Authorization
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