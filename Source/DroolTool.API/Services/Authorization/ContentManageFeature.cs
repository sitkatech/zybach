using DroolTool.EFModels.Entities;

namespace DroolTool.API.Services.Authorization
{
    public class ContentManageFeature : BaseAuthorizationAttribute
    {
        public ContentManageFeature() : base(new[] { RoleEnum.Admin })
        {
        }
    }
}