using System;

namespace Zybach.Models.DataTransferObjects.User
{
    public class UserCreateDto: UserUpsertDto
    {
        public string LoginName { get; set; }
        public Guid UserGuid { get; set; }
    }
}