using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects.Role;

namespace Zybach.EFModels.Entities
{
    public partial class Role
    {
        public static IEnumerable<RoleDto> List(ZybachDbContext dbContext)
        {
            var roles = dbContext.Role
                .AsNoTracking()
                .Select(x => x.AsDto());

            return roles;
        }

        public static RoleDto GetByRoleID(ZybachDbContext dbContext, int roleID)
        {
            var role = dbContext.Role
                .AsNoTracking()
                .FirstOrDefault(x => x.RoleID == roleID);

            return role?.AsDto();
        }
    }

    public enum RoleEnum
    {
        Admin = 1,
        LandOwner = 2,
        Unassigned = 3,
        Disabled = 4
    }
}
