using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("OpenETDataType")]
    public partial class OpenETDataType
    {
        public OpenETDataType()
        {
            OpenETSyncHistories = new HashSet<OpenETSyncHistory>();
        }

        [Key]
        public int OpenETDataTypeID { get; set; }
        [Required]
        [StringLength(50)]
        public string OpenETDataTypeName { get; set; }
        [Required]
        [StringLength(100)]
        public string OpenETDataTypeDisplayName { get; set; }
        [Required]
        [StringLength(20)]
        public string OpenETDataTypeVariableName { get; set; }

        [InverseProperty(nameof(OpenETSyncHistory.OpenETDataType))]
        public virtual ICollection<OpenETSyncHistory> OpenETSyncHistories { get; set; }
    }
}
