using System;

namespace Zybach.Models.DataTransferObjects;

public partial class OpenETSyncDto
{
    public DateTime? LastSuccessfulSyncDate { get; set; }
    public DateTime? LastSyncDate { get; set; }
    public string LastSyncMessage { get; set; }
    public bool HasInProgressSync { get; set; }
}