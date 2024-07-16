using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("BlobResource")]
[Index("BlobResourceCanonicalName", Name = "AK_BlobResource_CanonicalName", IsUnique = true)]
[Index("BlobResourceGUID", Name = "AK_BlobResource_FileResourceGUID", IsUnique = true)]
public partial class BlobResource
{
    [Key]
    public int BlobResourceID { get; set; }

    public Guid BlobResourceGUID { get; set; }

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string BlobResourceCanonicalName { get; set; }

    [Required]
    [StringLength(255)]
    [Unicode(false)]
    public string OriginalBaseFilename { get; set; }

    [Required]
    [StringLength(255)]
    [Unicode(false)]
    public string OriginalFileExtension { get; set; }

    public int CreateUserID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [ForeignKey("CreateUserID")]
    [InverseProperty("BlobResources")]
    public virtual User CreateUser { get; set; }
}
