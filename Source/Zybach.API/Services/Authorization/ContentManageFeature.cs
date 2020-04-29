using Zybach.EFModels.Entities;

namespace Zybach.API.Services.Authorization
{
    public class ContentManageFeature : BaseAuthorizationAttribute
    {
        public ContentManageFeature() : base(new[] { RoleEnum.Admin })
        {
        }
    }
}