using Zybach.EFModels.Entities;

namespace Zybach.API.Services.Authorization
{
    public class UserManageFeature : BaseAuthorizationAttribute
    {
        public UserManageFeature() : base(new []{RoleEnum.Admin})
        {
        }
    }
}