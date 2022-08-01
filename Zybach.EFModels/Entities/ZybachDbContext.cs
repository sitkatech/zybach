using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

public partial class ZybachDbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WellPumpingSummary>().HasNoKey();
        modelBuilder.Entity<AgHubIrrigationUnitMonthlyWaterVolumeSummary>().HasNoKey();
    }
    public virtual DbSet<WellPumpingSummary> WellPumpingSummaries { get; set; }
    public virtual DbSet<AgHubIrrigationUnitMonthlyWaterVolumeSummary> MonthlyWaterVolumeSummaries { get; set; }
}