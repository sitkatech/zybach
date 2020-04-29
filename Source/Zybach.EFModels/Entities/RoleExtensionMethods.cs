using Zybach.Models.DataTransferObjects.Role;

namespace Zybach.EFModels.Entities
{
    public static class RoleExtensionMethods
    {
        public static RoleDto AsDto(this Role role)
        {
            return new RoleDto()
            {
                RoleID = role.RoleID,
                RoleName = role.RoleName,
                RoleDisplayName = role.RoleDisplayName
            };
        }
    }
}