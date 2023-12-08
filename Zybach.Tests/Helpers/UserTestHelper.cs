using System;
using Zybach.Models.DataTransferObjects;
using Zybach.Models.DataTransferObjects.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zybach.Tests.Helpers
{
    public static class UserTestHelper
    {
        public static UserUpsertDto GetTestUserUpsertDto(int? roleID = 1)
        {
            var newUserDto = new UserUpsertDto()
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Email = $"{Guid.NewGuid()}@sitkatech.com",
                OrganizationName = Guid.NewGuid().ToString(),
                PhoneNumber = Guid.NewGuid().ToString().Substring(0, 10),
                ReceiveSupportEmails = true,
                RoleID = roleID
            };

            return newUserDto;
        }

        public static void AssertUserDtosAreEqualAndNotNull(UserDto expectedValues, UserDto valuesToCheck)
        {
            Assert.IsNotNull(expectedValues);
            Assert.IsNotNull(valuesToCheck);
            Assert.AreEqual(expectedValues.FirstName, valuesToCheck.FirstName);
            Assert.AreEqual(expectedValues.LastName, valuesToCheck.LastName);
            Assert.AreEqual(expectedValues.Email, valuesToCheck.Email);
            Assert.AreEqual(expectedValues.LoginName, valuesToCheck.LoginName);
            Assert.AreEqual(expectedValues.Company, valuesToCheck.Company);
            Assert.AreEqual(expectedValues.Phone, valuesToCheck.Phone);
            Assert.AreEqual(expectedValues.ReceiveSupportEmails, valuesToCheck.ReceiveSupportEmails);
            Assert.AreEqual(expectedValues.Role.RoleID, valuesToCheck.Role.RoleID);
            Assert.AreEqual(expectedValues.UserGuid, valuesToCheck.UserGuid);
        }

        public static void AssertUserDtosAreEqualAndNotNull(UserUpsertDto expectedValues, UserDto valuesToCheck)
        {
            Assert.IsNotNull(expectedValues);
            Assert.IsNotNull(valuesToCheck);
            Assert.AreEqual(expectedValues.FirstName, valuesToCheck.FirstName);
            Assert.AreEqual(expectedValues.LastName, valuesToCheck.LastName);
            Assert.AreEqual(expectedValues.Email, valuesToCheck.Email);
            Assert.AreEqual(expectedValues.OrganizationName, valuesToCheck.Company);
            Assert.AreEqual(expectedValues.PhoneNumber, valuesToCheck.Phone);
            Assert.AreEqual(expectedValues.ReceiveSupportEmails, valuesToCheck.ReceiveSupportEmails);
            Assert.AreEqual(expectedValues.RoleID, valuesToCheck.Role.RoleID);
        }
    }
}