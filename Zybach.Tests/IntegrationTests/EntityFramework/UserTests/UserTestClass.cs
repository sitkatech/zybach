using System;
using System.Collections.Generic;
using System.Linq;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;
using Zybach.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zybach.Tests.IntegrationTests.EntityFramework.UserTests
{
    public class UserTestClass : BaseTestClass
    {
        private static UserDto _newUser;

        protected static UserDto NewUser
        {
            get
            {
                if (_newUser == null)
                {
                    CreateNewUser();
                }

                return _newUser;
            }
        }

        protected static List<UserDto> UsersToCleanUp = new();

        private static void CreateNewUser()
        {
            var newUserDto = UserTestHelper.GetTestUserUpsertDto();
            var testUserGuid = Guid.NewGuid();
            var testUserLoginName = "Test User";
            _newUser = User.CreateNewUser(AssemblySteps.DbContext, newUserDto, testUserLoginName, testUserGuid);
            UserTestHelper.AssertUserDtosAreEqualAndNotNull(newUserDto, _newUser);
            UsersToCleanUp.Add(_newUser);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (AssemblySteps.Idempotent)
            {
                foreach (var userDto in UsersToCleanUp)
                {
                    var user = AssemblySteps.DbContext.Users.FirstOrDefault(x => x.UserID == userDto.UserID);
                    if (user != null)
                    {
                        AssemblySteps.DbContext.Users.Remove(user);
                        AssemblySteps.DbContext.SaveChanges();
                    }
                }
            }
        }
    }
}
