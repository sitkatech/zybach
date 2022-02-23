using Zybach.EFModels.Entities;

namespace Zybach.API.Services.Authorization
{
    public class ZybachEditFeature : BaseAuthorizationAttribute
    {
        public ZybachEditFeature() : base(new[] { RoleEnum.Normal, RoleEnum.Admin })
        {
        }
    }
}