using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

public partial class ZybachDbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WellPumpingSummary>().HasNoKey();
        modelBuilder.Entity<AgHubIrrigationUnitMonthlyWaterVolumeSummary>().HasNoKey();
        modelBuilder.Entity<AgHubIrrigationUnitSummary>().HasNoKey();
    }
    public virtual DbSet<WellPumpingSummary> WellPumpingSummaries { get; set; }
    public virtual DbSet<AgHubIrrigationUnitMonthlyWaterVolumeSummary> MonthlyWaterVolumeSummaries { get; set; }
    public virtual DbSet<AgHubIrrigationUnitSummary> AgHubIrrigationUnitSummaries { get; set; }
}