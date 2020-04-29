using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zybach.EFModels.Entities
{
    public partial class ZybachRole
    {
        [Key]
        public int ZybachRoleID { get; set; }
        [Required]
        [StringLength(100)]
        public string ZybachRoleName { get; set; }
        [Required]
        [StringLength(100)]
        public string ZybachRoleDisplayName { get; set; }
        [StringLength(255)]
        public string ZybachRoleDescription { get; set; }
    }
}
