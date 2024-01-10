using System.Collections.Generic;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public class Roles
    {
        public static IEnumerable<RoleDto> List(ZybachDbContext dbContext)
        {
            return Role.AllAsDto;
        }

        public static RoleDto GetByRoleID(ZybachDbContext dbContext, int roleID)
        {
            return Role.AllAsDtoLookupDictionary[roleID];
        }
    }
}
