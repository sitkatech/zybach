using Zybach.EFModels.Entities;

namespace Zybach.API.Services.Authorization
{
    public class UserViewFeature : BaseAuthorizationAttribute
    {
        public UserViewFeature() : base(new []{RoleEnum.Admin, RoleEnum.Normal, RoleEnum.WaterDataProgramSupportOnly, RoleEnum.Unassigned})
        {
        }
    }
}