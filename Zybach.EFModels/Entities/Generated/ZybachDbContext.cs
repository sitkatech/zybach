using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Zybach.EFModels.Entities
{
    public partial class ZybachDbContext : DbContext
    {
        public ZybachDbContext()
        {
        }

        public ZybachDbContext(DbContextOptions<ZybachDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AgHubIrrigationUnit> AgHubIrrigationUnits { get; set; }
        public virtual DbSet<AgHubIrrigationUnitWaterYearMonthETDatum> AgHubIrrigationUnitWaterYearMonthETData { get; set; }
        public virtual DbSet<AgHubIrrigationUnitWaterYearMonthPrecipitationDatum> AgHubIrrigationUnitWaterYearMonthPrecipitationData { get; set; }
        public virtual DbSet<AgHubWell> AgHubWells { get; set; }
        public virtual DbSet<AgHubWellIrrigatedAcre> AgHubWellIrrigatedAcres { get; set; }
        public virtual DbSet<AgHubWellIrrigatedAcreStaging> AgHubWellIrrigatedAcreStagings { get; set; }
        public virtual DbSet<AgHubWellStaging> AgHubWellStagings { get; set; }
        public virtual DbSet<ChemicalFormulation> ChemicalFormulations { get; set; }
        public virtual DbSet<ChemicalUnit> ChemicalUnits { get; set; }
        public virtual DbSet<ChemigationInjectionValve> ChemigationInjectionValves { get; set; }
        public virtual DbSet<ChemigationInspection> ChemigationInspections { get; set; }
        public virtual DbSet<ChemigationInspectionFailureReason> ChemigationInspectionFailureReasons { get; set; }
        public virtual DbSet<ChemigationMainlineCheckValve> ChemigationMainlineCheckValves { get; set; }
        public virtual DbSet<ChemigationPermit> ChemigationPermits { get; set; }
        public virtual DbSet<ChemigationPermitAnnualRecord> ChemigationPermitAnnualRecords { get; set; }
        public virtual DbSet<ChemigationPermitAnnualRecordApplicator> ChemigationPermitAnnualRecordApplicators { get; set; }
        public virtual DbSet<ChemigationPermitAnnualRecordChemicalFormulation> ChemigationPermitAnnualRecordChemicalFormulations { get; set; }
        public virtual DbSet<CropType> CropTypes { get; set; }
        public virtual DbSet<CustomRichText> CustomRichTexts { get; set; }
        public virtual DbSet<FieldDefinition> FieldDefinitions { get; set; }
        public virtual DbSet<FileResource> FileResources { get; set; }
        public virtual DbSet<GeoOptixSensorStaging> GeoOptixSensorStagings { get; set; }
        public virtual DbSet<GeoOptixWell> GeoOptixWells { get; set; }
        public virtual DbSet<GeoOptixWellStaging> GeoOptixWellStagings { get; set; }
        public virtual DbSet<OpenETGoogleBucketResponseEvapotranspirationDatum> OpenETGoogleBucketResponseEvapotranspirationData { get; set; }
        public virtual DbSet<OpenETGoogleBucketResponsePrecipitationDatum> OpenETGoogleBucketResponsePrecipitationData { get; set; }
        public virtual DbSet<OpenETSyncHistory> OpenETSyncHistories { get; set; }
        public virtual DbSet<ReportTemplate> ReportTemplates { get; set; }
        public virtual DbSet<RobustReviewScenarioGETRunHistory> RobustReviewScenarioGETRunHistories { get; set; }
        public virtual DbSet<Sensor> Sensors { get; set; }
        public virtual DbSet<SensorAnomaly> SensorAnomalies { get; set; }
        public virtual DbSet<StreamFlowZone> StreamFlowZones { get; set; }
        public virtual DbSet<SupportTicket> SupportTickets { get; set; }
        public virtual DbSet<SupportTicketComment> SupportTicketComments { get; set; }
        public virtual DbSet<SupportTicketNotification> SupportTicketNotifications { get; set; }
        public virtual DbSet<Tillage> Tillages { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<WaterLevelInspection> WaterLevelInspections { get; set; }
        public virtual DbSet<WaterLevelMeasuringEquipment> WaterLevelMeasuringEquipments { get; set; }
        public virtual DbSet<WaterQualityInspection> WaterQualityInspections { get; set; }
        public virtual DbSet<WaterYearMonth> WaterYearMonths { get; set; }
        public virtual DbSet<Well> Wells { get; set; }
        public virtual DbSet<WellParticipation> WellParticipations { get; set; }
        public virtual DbSet<WellSensorMeasurement> WellSensorMeasurements { get; set; }
        public virtual DbSet<WellSensorMeasurementStaging> WellSensorMeasurementStagings { get; set; }
        public virtual DbSet<WellWaterQualityInspectionType> WellWaterQualityInspectionTypes { get; set; }
        public virtual DbSet<vOpenETMostRecentSyncHistoryForYearAndMonth> vOpenETMostRecentSyncHistoryForYearAndMonths { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AgHubIrrigationUnitWaterYearMonthETDatum>(entity =>
            {
                entity.HasOne(d => d.AgHubIrrigationUnit)
                    .WithMany(p => p.AgHubIrrigationUnitWaterYearMonthETData)
                    .HasForeignKey(d => d.AgHubIrrigationUnitID)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.WaterYearMonth)
                    .WithMany(p => p.AgHubIrrigationUnitWaterYearMonthETData)
                    .HasForeignKey(d => d.WaterYearMonthID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AgHubIrrigationUnitWaterYearMonthPrecipitationDatum>(entity =>
            {
                entity.HasOne(d => d.AgHubIrrigationUnit)
                    .WithMany(p => p.AgHubIrrigationUnitWaterYearMonthPrecipitationData)
                    .HasForeignKey(d => d.AgHubIrrigationUnitID)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.WaterYearMonth)
                    .WithMany(p => p.AgHubIrrigationUnitWaterYearMonthPrecipitationData)
                    .HasForeignKey(d => d.WaterYearMonthID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AgHubWell>(entity =>
            {
                entity.HasOne(d => d.Well)
                    .WithOne(p => p.AgHubWell)
                    .HasForeignKey<AgHubWell>(d => d.WellID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AgHubWellIrrigatedAcre>(entity =>
            {
                entity.HasOne(d => d.AgHubWell)
                    .WithMany(p => p.AgHubWellIrrigatedAcres)
                    .HasForeignKey(d => d.AgHubWellID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ChemicalUnit>(entity =>
            {
                entity.Property(e => e.ChemicalUnitID).ValueGeneratedNever();
            });

            modelBuilder.Entity<ChemigationInjectionValve>(entity =>
            {
                entity.Property(e => e.ChemigationInjectionValveID).ValueGeneratedNever();
            });

            modelBuilder.Entity<ChemigationInspection>(entity =>
            {
                entity.HasOne(d => d.ChemigationPermitAnnualRecord)
                    .WithMany(p => p.ChemigationInspections)
                    .HasForeignKey(d => d.ChemigationPermitAnnualRecordID)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.InspectorUser)
                    .WithMany(p => p.ChemigationInspections)
                    .HasForeignKey(d => d.InspectorUserID)
                    .HasConstraintName("FK_ChemigationInspection_User_InspectorUserID_UserID");
            });

            modelBuilder.Entity<ChemigationInspectionFailureReason>(entity =>
            {
                entity.Property(e => e.ChemigationInspectionFailureReasonID).ValueGeneratedNever();
            });

            modelBuilder.Entity<ChemigationMainlineCheckValve>(entity =>
            {
                entity.Property(e => e.ChemigationMainlineCheckValveID).ValueGeneratedNever();
            });

            modelBuilder.Entity<ChemigationPermitAnnualRecord>(entity =>
            {
                entity.HasOne(d => d.ChemigationPermit)
                    .WithMany(p => p.ChemigationPermitAnnualRecords)
                    .HasForeignKey(d => d.ChemigationPermitID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ChemigationPermitAnnualRecordApplicator>(entity =>
            {
                entity.HasOne(d => d.ChemigationPermitAnnualRecord)
                    .WithMany(p => p.ChemigationPermitAnnualRecordApplicators)
                    .HasForeignKey(d => d.ChemigationPermitAnnualRecordID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ChemigationPermitAnnualRecordChemicalFormulation>(entity =>
            {
                entity.HasOne(d => d.ChemicalFormulation)
                    .WithMany(p => p.ChemigationPermitAnnualRecordChemicalFormulations)
                    .HasForeignKey(d => d.ChemicalFormulationID)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ChemicalUnit)
                    .WithMany(p => p.ChemigationPermitAnnualRecordChemicalFormulations)
                    .HasForeignKey(d => d.ChemicalUnitID)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ChemigationPermitAnnualRecord)
                    .WithMany(p => p.ChemigationPermitAnnualRecordChemicalFormulations)
                    .HasForeignKey(d => d.ChemigationPermitAnnualRecordID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<CropType>(entity =>
            {
                entity.Property(e => e.CropTypeID).ValueGeneratedNever();
            });

            modelBuilder.Entity<FileResource>(entity =>
            {
                entity.HasOne(d => d.CreateUser)
                    .WithMany(p => p.FileResources)
                    .HasForeignKey(d => d.CreateUserID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FileResource_User_CreateUserID_UserID");
            });

            modelBuilder.Entity<GeoOptixWell>(entity =>
            {
                entity.HasOne(d => d.Well)
                    .WithOne(p => p.GeoOptixWell)
                    .HasForeignKey<GeoOptixWell>(d => d.WellID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<OpenETSyncHistory>(entity =>
            {
                entity.HasOne(d => d.WaterYearMonth)
                    .WithMany(p => p.OpenETSyncHistories)
                    .HasForeignKey(d => d.WaterYearMonthID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ReportTemplate>(entity =>
            {
                entity.HasOne(d => d.FileResource)
                    .WithMany(p => p.ReportTemplates)
                    .HasForeignKey(d => d.FileResourceID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<RobustReviewScenarioGETRunHistory>(entity =>
            {
                entity.HasOne(d => d.CreateByUser)
                    .WithMany(p => p.RobustReviewScenarioGETRunHistories)
                    .HasForeignKey(d => d.CreateByUserID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RobustReviewScenarioGETRunHistory_User_CreateByUserID_UserID");
            });

            modelBuilder.Entity<SensorAnomaly>(entity =>
            {
                entity.HasOne(d => d.Sensor)
                    .WithMany(p => p.SensorAnomalies)
                    .HasForeignKey(d => d.SensorID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<SupportTicket>(entity =>
            {
                entity.HasOne(d => d.AssigneeUser)
                    .WithMany(p => p.SupportTicketAssigneeUsers)
                    .HasForeignKey(d => d.AssigneeUserID)
                    .HasConstraintName("FK_SupportTicket_User_AssigneeUserID_UserID");

                entity.HasOne(d => d.CreatorUser)
                    .WithMany(p => p.SupportTicketCreatorUsers)
                    .HasForeignKey(d => d.CreatorUserID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SupportTicket_User_CreatorUserID_UserID");

                entity.HasOne(d => d.Well)
                    .WithMany(p => p.SupportTickets)
                    .HasForeignKey(d => d.WellID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<SupportTicketComment>(entity =>
            {
                entity.HasOne(d => d.CreatorUser)
                    .WithMany(p => p.SupportTicketComments)
                    .HasForeignKey(d => d.CreatorUserID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SupportTicketComment_User_CreatorUserID_UserID");

                entity.HasOne(d => d.SupportTicket)
                    .WithMany(p => p.SupportTicketComments)
                    .HasForeignKey(d => d.SupportTicketID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<SupportTicketNotification>(entity =>
            {
                entity.HasOne(d => d.SupportTicket)
                    .WithMany(p => p.SupportTicketNotifications)
                    .HasForeignKey(d => d.SupportTicketID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Tillage>(entity =>
            {
                entity.Property(e => e.TillageID).ValueGeneratedNever();
            });

            modelBuilder.Entity<WaterLevelInspection>(entity =>
            {
                entity.HasOne(d => d.InspectorUser)
                    .WithMany(p => p.WaterLevelInspections)
                    .HasForeignKey(d => d.InspectorUserID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WaterLevelInspection_User_InspectorUserID_UserID");

                entity.HasOne(d => d.Well)
                    .WithMany(p => p.WaterLevelInspections)
                    .HasForeignKey(d => d.WellID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<WaterLevelMeasuringEquipment>(entity =>
            {
                entity.Property(e => e.WaterLevelMeasuringEquipmentID).ValueGeneratedNever();
            });

            modelBuilder.Entity<WaterQualityInspection>(entity =>
            {
                entity.HasOne(d => d.InspectorUser)
                    .WithMany(p => p.WaterQualityInspections)
                    .HasForeignKey(d => d.InspectorUserID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WaterQualityInspection_User_InspectorUserID_UserID");

                entity.HasOne(d => d.Well)
                    .WithMany(p => p.WaterQualityInspections)
                    .HasForeignKey(d => d.WellID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Well>(entity =>
            {
                entity.HasOne(d => d.StreamflowZone)
                    .WithMany(p => p.Wells)
                    .HasForeignKey(d => d.StreamflowZoneID)
                    .HasConstraintName("FK_Well_StreamFlowZone_StreamFlowZoneID");
            });

            modelBuilder.Entity<WellParticipation>(entity =>
            {
                entity.Property(e => e.WellParticipationID).ValueGeneratedNever();
            });

            modelBuilder.Entity<WellWaterQualityInspectionType>(entity =>
            {
                entity.HasOne(d => d.Well)
                    .WithMany(p => p.WellWaterQualityInspectionTypes)
                    .HasForeignKey(d => d.WellID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<vOpenETMostRecentSyncHistoryForYearAndMonth>(entity =>
            {
                entity.ToView("vOpenETMostRecentSyncHistoryForYearAndMonth");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
