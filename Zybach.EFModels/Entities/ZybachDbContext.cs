﻿using Microsoft.EntityFrameworkCore;
using Rio.EFModels.Entities;

namespace Zybach.EFModels.Entities;

public partial class ZybachDbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WellPumpingSummary>().HasNoKey();
    }
    public virtual DbSet<WellPumpingSummary> WellPumpingSummaries { get; set; }
}