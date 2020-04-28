using DroolTool.EFModels.Entities;

namespace DroolTool.API.Services.Authorization
{
    public class UserManageFeature : BaseAuthorizationAttribute
    {
        public UserManageFeature() : base(new []{RoleEnum.Admin})
        {
        }
    }
}