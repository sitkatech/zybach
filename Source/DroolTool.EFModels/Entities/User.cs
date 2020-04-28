using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DroolTool.API.Util;
using DroolTool.Models.DataTransferObjects;
using DroolTool.Models.DataTransferObjects.User;

namespace DroolTool.EFModels.Entities
{
    public partial class User
    {
        public static UserDto CreateNewUser(DroolToolDbContext dbContext, UserUpsertDto userToCreate, string loginName, Guid userGuid)
        {
            if (!userToCreate.RoleID.HasValue)
            {
                return null;
            }

            var user = new User
            {
                UserGuid = userGuid,
                LoginName = loginName,
                Email = userToCreate.Email,
                FirstName = userToCreate.FirstName,
                LastName = userToCreate.LastName,
                IsActive = true,
                RoleID = userToCreate.RoleID.Value,
                CreateDate = DateTime.UtcNow,
            };

            dbContext.User.Add(user);
            dbContext.SaveChanges();
            dbContext.Entry(user).Reload();

            return GetByUserID(dbContext, user.UserID);
        }

        public static IEnumerable<UserDetailedDto> List(DroolToolDbContext dbContext)
        {
            // right now we are assuming a parcel can only be associated to one user
            var parcels = dbContext.User.Include(x => x.Role).OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList()
                .Select(user =>
                {
                    var userDetailedDto = new UserDetailedDto()
                    {
                        UserID = user.UserID,
                        UserGuid = user.UserGuid,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        LoginName = user.LoginName,
                        RoleID = user.RoleID,
                        RoleDisplayName = user.Role.RoleDisplayName,
                        Phone = user.Phone,
                        HasActiveTrades = false,
                        AcreFeetOfWaterPurchased = 0,
                        AcreFeetOfWaterSold = 0,
                        ReceiveSupportEmails = user.ReceiveSupportEmails
                    };
                    return userDetailedDto;
                }).ToList();
            return parcels;
        }

        public static IEnumerable<UserDto> ListByRole(DroolToolDbContext dbContext, RoleEnum roleEnum)
        {
            var users = GetUserImpl(dbContext)
                .Where(x => x.IsActive && x.RoleID == (int) roleEnum)
                .OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
                .Select(x => x.AsDto())
                .AsEnumerable();

            return users;
        }

        public static IEnumerable<string> GetEmailAddressesForAdminsThatReceiveSupportEmails(DroolToolDbContext dbContext)
        {
            var users = GetUserImpl(dbContext)
                .Where(x => x.IsActive && x.RoleID == (int) RoleEnum.Admin && x.ReceiveSupportEmails)
                .Select(x => x.Email)
                .AsEnumerable();

            return users;
        }

        public static UserDto GetByUserID(DroolToolDbContext dbContext, int userID)
        {
            var user = GetUserImpl(dbContext).SingleOrDefault(x => x.UserID == userID);
            return user?.AsDto();
        }

        public static List<UserDto> GetByUserID(DroolToolDbContext dbContext, List<int> userIDs)
        {
            return GetUserImpl(dbContext).Where(x => userIDs.Contains(x.UserID)).Select(x=>x.AsDto()).ToList();
            
        }

        public static UserDto GetByUserGuid(DroolToolDbContext dbContext, Guid userGuid)
        {
            var user = GetUserImpl(dbContext)
                .SingleOrDefault(x => x.UserGuid == userGuid);

            return user?.AsDto();
        }

        private static IQueryable<User> GetUserImpl(DroolToolDbContext dbContext)
        {
            return dbContext.User
                .Include(x => x.Role)
                .AsNoTracking();
        }

        public static UserDto GetByEmail(DroolToolDbContext dbContext, string email)
        {
            var user = GetUserImpl(dbContext).SingleOrDefault(x => x.Email == email);
            return user?.AsDto();
        }

        public static UserDto UpdateUserEntity(DroolToolDbContext dbContext, int userID, UserUpsertDto userEditDto)
        {
            if (!userEditDto.RoleID.HasValue)
            {
                return null;
            }

            var user = dbContext.User
                .Include(x => x.Role)
                .Single(x => x.UserID == userID);

            user.RoleID = userEditDto.RoleID.Value;
            user.ReceiveSupportEmails = userEditDto.RoleID.Value == 1 && userEditDto.ReceiveSupportEmails;
            user.UpdateDate = DateTime.UtcNow;

            dbContext.SaveChanges();
            dbContext.Entry(user).Reload();
            return GetByUserID(dbContext, userID);
        }

        public static UserDto UpdateUserGuid(DroolToolDbContext dbContext, int userID, Guid userGuid)
        {
            var user = dbContext.User
                .Single(x => x.UserID == userID);

            user.UserGuid = userGuid;
            user.UpdateDate = DateTime.UtcNow;

            dbContext.SaveChanges();
            dbContext.Entry(user).Reload();
            return GetByUserID(dbContext, userID);
        }

        public static List<ErrorMessage> ValidateUpdate(DroolToolDbContext dbContext, UserUpsertDto userEditDto, int userID)
        {
            var result = new List<ErrorMessage>();
            if (!userEditDto.RoleID.HasValue)
            {
                result.Add(new ErrorMessage() { Type = "Role ID", Message = "Role ID is required." });
            }

            return result;
        }

        public static bool ValidateAllExist(DroolToolDbContext dbContext, List<int> userIDs)
        {
            return dbContext.User.Count(x => userIDs.Contains(x.UserID)) == userIDs.Distinct().Count();
        }
    }
}