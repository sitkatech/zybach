using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DroolTool.EFModels.Entities
{
    public partial class DroolToolRole
    {
        [Key]
        public int DroolToolRoleID { get; set; }
        [Required]
        [StringLength(100)]
        public string DroolToolRoleName { get; set; }
        [Required]
        [StringLength(100)]
        public string DroolToolRoleDisplayName { get; set; }
        [StringLength(255)]
        public string DroolToolRoleDescription { get; set; }
    }
}
