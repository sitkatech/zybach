using System.Collections.Generic;
using System.Linq;
using Zybach.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    public partial class Role
    {
        public static IEnumerable<RoleDto> List(ZybachDbContext dbContext)
        {
            var roles = dbContext.Roles
                .AsNoTracking()
                .Select(x => x.AsDto());

            return roles;
        }

        public static RoleDto GetByRoleID(ZybachDbContext dbContext, int roleID)
        {
            var role = dbContext.Roles
                .AsNoTracking()
                .FirstOrDefault(x => x.RoleID == roleID);

            return role?.AsDto();
        }
    }

    public enum RoleEnum
    {
        Admin = 1,
        Normal = 2,
        Unassigned = 3,
        Disabled = 4,
        WaterDataProgramReadOnly = 5
    }
}
