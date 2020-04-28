﻿using DroolTool.Models.DataTransferObjects.Role;
using System;

namespace DroolTool.Models.DataTransferObjects.User
{
    public class UserDto : UserSimpleDto
    {
        public Guid? UserGuid { get; set; }
        public string Phone { get; set; }
        public string LoginName { get; set; }
        public RoleDto Role { get; set; }
        public DateTime? DisclaimerAcknowledgedDate { get; set; }
        public bool ReceiveSupportEmails { get; set; }
    }
}
