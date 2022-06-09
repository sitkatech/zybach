using Zybach.EFModels.Entities;

namespace Zybach.API.Services.Authorization
{
    public class ZybachViewFeature : BaseAuthorizationAttribute
    {
        public ZybachViewFeature() : base(new []{RoleEnum.Normal, RoleEnum.Admin, RoleEnum.WaterDataProgramSupportOnly})
        {
        }
    }
}