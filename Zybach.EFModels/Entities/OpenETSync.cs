using System;

namespace Zybach.EFModels.Entities;

public partial class OpenETSync
{
    public DateTime ReportedDate => new(Year, Month, 1);
}