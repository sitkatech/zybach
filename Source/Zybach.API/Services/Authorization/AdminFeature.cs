using Zybach.EFModels.Entities;

namespace Zybach.API.Services.Authorization
{
    public class AdminFeature : BaseAuthorizationAttribute
    {
        public AdminFeature() : base(new []{RoleEnum.Admin})
        {
        }
    }
}