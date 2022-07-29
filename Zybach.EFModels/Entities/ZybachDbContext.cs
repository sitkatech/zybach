using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

public partial class ZybachDbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WellPumpingSummary>().HasNoKey();
        modelBuilder.Entity<MonthlyWaterVolumeSummary>().HasNoKey();
    }
    public virtual DbSet<WellPumpingSummary> WellPumpingSummaries { get; set; }
    public virtual DbSet<MonthlyWaterVolumeSummary> MonthlyWaterVolumeSummaries { get; set; }
}