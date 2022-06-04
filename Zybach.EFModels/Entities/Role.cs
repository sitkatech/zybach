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
            return Role.AllAsDto;
        }

        public static RoleDto GetByRoleID(ZybachDbContext dbContext, int roleID)
        {
            return Role.AllAsDtoLookupDictionary[roleID];
        }
    }
}
