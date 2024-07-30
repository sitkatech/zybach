using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

public partial class ZybachDbContext : DbContext
{
    public ZybachDbContext(DbContextOptions<ZybachDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AgHubIrrigationUnit> AgHubIrrigationUnits { get; set; }

    public virtual DbSet<AgHubIrrigationUnitGeometry> AgHubIrrigationUnitGeometries { get; set; }

    public virtual DbSet<AgHubIrrigationUnitOpenETDatum> AgHubIrrigationUnitOpenETData { get; set; }

    public virtual DbSet<AgHubIrrigationUnitRunoff> AgHubIrrigationUnitRunoffs { get; set; }

    public virtual DbSet<AgHubWell> AgHubWells { get; set; }

    public virtual DbSet<AgHubWellIrrigatedAcre> AgHubWellIrrigatedAcres { get; set; }

    public virtual DbSet<AgHubWellIrrigatedAcreStaging> AgHubWellIrrigatedAcreStagings { get; set; }

    public virtual DbSet<AgHubWellStaging> AgHubWellStagings { get; set; }

    public virtual DbSet<BlobResource> BlobResources { get; set; }

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

    public virtual DbSet<OpenETSync> OpenETSyncs { get; set; }

    public virtual DbSet<OpenETSyncHistory> OpenETSyncHistories { get; set; }

    public virtual DbSet<OpenETWaterMeasurement> OpenETWaterMeasurements { get; set; }

    public virtual DbSet<PaigeWirelessPulse> PaigeWirelessPulses { get; set; }

    public virtual DbSet<PrismDailyRecord> PrismDailyRecords { get; set; }

    public virtual DbSet<PrismMonthlySync> PrismMonthlySyncs { get; set; }

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

    public virtual DbSet<Well> Wells { get; set; }

    public virtual DbSet<WellGroup> WellGroups { get; set; }

    public virtual DbSet<WellGroupWell> WellGroupWells { get; set; }

    public virtual DbSet<WellParticipation> WellParticipations { get; set; }

    public virtual DbSet<WellSensorMeasurement> WellSensorMeasurements { get; set; }

    public virtual DbSet<WellSensorMeasurementStaging> WellSensorMeasurementStagings { get; set; }

    public virtual DbSet<WellWaterQualityInspectionType> WellWaterQualityInspectionTypes { get; set; }

    public virtual DbSet<vGeoServerAgHubIrrigationUnit> vGeoServerAgHubIrrigationUnits { get; set; }

    public virtual DbSet<vOpenETMostRecentSyncHistoryForYearAndMonth> vOpenETMostRecentSyncHistoryForYearAndMonths { get; set; }

    public virtual DbSet<vSensor> vSensors { get; set; }

    public virtual DbSet<vSensorLatestBatteryVoltage> vSensorLatestBatteryVoltages { get; set; }

    public virtual DbSet<vWellSensorMeasurementFirstAndLatestForSensor> vWellSensorMeasurementFirstAndLatestForSensors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AgHubIrrigationUnit>(entity =>
        {
            entity.HasKey(e => e.AgHubIrrigationUnitID).HasName("PK_AgHubIrrigationUnit_AgHubIrrigationUnitID");
        });

        modelBuilder.Entity<AgHubIrrigationUnitGeometry>(entity =>
        {
            entity.HasKey(e => e.AgHubIrrigationUnitGeometryID).HasName("PK_AgHubIrrigationUnitGeometry_AgHubIrrigationUnitGeometryID");

            entity.HasOne(d => d.AgHubIrrigationUnit).WithOne(p => p.AgHubIrrigationUnitGeometry).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<AgHubIrrigationUnitOpenETDatum>(entity =>
        {
            entity.HasKey(e => e.AgHubIrrigationUnitOpenETDatumID).HasName("PK_AgHubIrrigationUnitOpenETDatum_AgHubIrrigationUnitOpenETDatumID");

            entity.HasOne(d => d.AgHubIrrigationUnit).WithMany(p => p.AgHubIrrigationUnitOpenETData).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<AgHubIrrigationUnitRunoff>(entity =>
        {
            entity.HasKey(e => e.AgHubIrrigationUnitRunoffID).HasName("PK_AgHubIrrigationUnitRunoff_AgHubIrrigationUnitRunoffID");

            entity.HasOne(d => d.AgHubIrrigationUnit).WithMany(p => p.AgHubIrrigationUnitRunoffs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AgHubIrrigationUnitRunoff_AgHubIrrigationUnitID");
        });

        modelBuilder.Entity<AgHubWell>(entity =>
        {
            entity.HasKey(e => e.AgHubWellID).HasName("PK_AgHubWell_AgHubWellID");

            entity.HasOne(d => d.Well).WithOne(p => p.AgHubWell).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<AgHubWellIrrigatedAcre>(entity =>
        {
            entity.HasKey(e => e.AgHubWellIrrigatedAcreID).HasName("PK_AgHubWellIrrigatedAcre_AgHubWellIrrigatedAcreID");

            entity.HasOne(d => d.AgHubWell).WithMany(p => p.AgHubWellIrrigatedAcres).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<AgHubWellIrrigatedAcreStaging>(entity =>
        {
            entity.HasKey(e => e.AgHubWellIrrigatedAcreStagingID).HasName("PK_AgHubWellIrrigatedAcreStaging_AgHubWellIrrigatedAcreStagingID");
        });

        modelBuilder.Entity<AgHubWellStaging>(entity =>
        {
            entity.HasKey(e => e.AgHubWellStagingID).HasName("PK_AgHubWellStaging_AgHubWellStagingID");
        });

        modelBuilder.Entity<BlobResource>(entity =>
        {
            entity.HasKey(e => e.BlobResourceID).HasName("PK_BlobResource_FileResourceID");

            entity.HasOne(d => d.CreateUser).WithMany(p => p.BlobResources)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BlobResource_User_CreateUserID_UserID");
        });

        modelBuilder.Entity<ChemicalFormulation>(entity =>
        {
            entity.HasKey(e => e.ChemicalFormulationID).HasName("PK_ChemicalFormulation_ChemicalFormulationID");
        });

        modelBuilder.Entity<ChemicalUnit>(entity =>
        {
            entity.HasKey(e => e.ChemicalUnitID).HasName("PK_ChemicalUnit_ChemicalUnitID");

            entity.Property(e => e.ChemicalUnitID).ValueGeneratedNever();
        });

        modelBuilder.Entity<ChemigationInjectionValve>(entity =>
        {
            entity.HasKey(e => e.ChemigationInjectionValveID).HasName("PK_ChemigationInjectionValve_ChemigationInjectionValveID");

            entity.Property(e => e.ChemigationInjectionValveID).ValueGeneratedNever();
        });

        modelBuilder.Entity<ChemigationInspection>(entity =>
        {
            entity.HasKey(e => e.ChemigationInspectionID).HasName("PK_ChemigationInspection_ChemigationInspectionID");

            entity.HasOne(d => d.ChemigationPermitAnnualRecord).WithMany(p => p.ChemigationInspections).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.InspectorUser).WithMany(p => p.ChemigationInspections).HasConstraintName("FK_ChemigationInspection_User_InspectorUserID_UserID");
        });

        modelBuilder.Entity<ChemigationInspectionFailureReason>(entity =>
        {
            entity.HasKey(e => e.ChemigationInspectionFailureReasonID).HasName("PK_ChemigationInspectionFailureReason_ChemigationInspectionFailureReasonID");

            entity.Property(e => e.ChemigationInspectionFailureReasonID).ValueGeneratedNever();
        });

        modelBuilder.Entity<ChemigationMainlineCheckValve>(entity =>
        {
            entity.HasKey(e => e.ChemigationMainlineCheckValveID).HasName("PK_ChemigationMainlineCheckValve_ChemigationMainlineCheckValveID");

            entity.Property(e => e.ChemigationMainlineCheckValveID).ValueGeneratedNever();
        });

        modelBuilder.Entity<ChemigationPermit>(entity =>
        {
            entity.HasKey(e => e.ChemigationPermitID).HasName("PK_ChemigationPermit_ChemigationPermitID");
        });

        modelBuilder.Entity<ChemigationPermitAnnualRecord>(entity =>
        {
            entity.HasKey(e => e.ChemigationPermitAnnualRecordID).HasName("PK_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordID");

            entity.HasOne(d => d.ChemigationPermit).WithMany(p => p.ChemigationPermitAnnualRecords).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ChemigationPermitAnnualRecordApplicator>(entity =>
        {
            entity.HasKey(e => e.ChemigationPermitAnnualRecordApplicatorID).HasName("PK_ChemigationPermitAnnualRecordApplicator_ChemigationPermitAnnualRecordApplicatorID");

            entity.HasOne(d => d.ChemigationPermitAnnualRecord).WithMany(p => p.ChemigationPermitAnnualRecordApplicators).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ChemigationPermitAnnualRecordChemicalFormulation>(entity =>
        {
            entity.HasKey(e => e.ChemigationPermitAnnualRecordChemicalFormulationID).HasName("PK_ChemigationPermitAnnualRecordChemicalFormulation_ChemigationPermitAnnualRecordChemicalFormulationID");

            entity.HasOne(d => d.ChemicalFormulation).WithMany(p => p.ChemigationPermitAnnualRecordChemicalFormulations).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.ChemicalUnit).WithMany(p => p.ChemigationPermitAnnualRecordChemicalFormulations).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.ChemigationPermitAnnualRecord).WithMany(p => p.ChemigationPermitAnnualRecordChemicalFormulations).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<CropType>(entity =>
        {
            entity.HasKey(e => e.CropTypeID).HasName("PK_CropType_CropTypeID");

            entity.Property(e => e.CropTypeID).ValueGeneratedNever();
        });

        modelBuilder.Entity<CustomRichText>(entity =>
        {
            entity.HasKey(e => e.CustomRichTextID).HasName("PK_CustomRichText_CustomRichTextID");
        });

        modelBuilder.Entity<FieldDefinition>(entity =>
        {
            entity.HasKey(e => e.FieldDefinitionID).HasName("PK_FieldDefinition_FieldDefinitionID");
        });

        modelBuilder.Entity<FileResource>(entity =>
        {
            entity.HasKey(e => e.FileResourceID).HasName("PK_FileResource_FileResourceID");

            entity.HasOne(d => d.CreateUser).WithMany(p => p.FileResources)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FileResource_User_CreateUserID_UserID");
        });

        modelBuilder.Entity<GeoOptixSensorStaging>(entity =>
        {
            entity.HasKey(e => e.GeoOptixSensorStagingID).HasName("PK_GeoOptixSensorStaging_GeoOptixSensorStagingID");
        });

        modelBuilder.Entity<GeoOptixWell>(entity =>
        {
            entity.HasKey(e => e.GeoOptixWellID).HasName("PK_GeoOptixWell_GeoOptixWellID");

            entity.HasOne(d => d.Well).WithOne(p => p.GeoOptixWell).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<GeoOptixWellStaging>(entity =>
        {
            entity.HasKey(e => e.GeoOptixWellStagingID).HasName("PK_GeoOptixWellStaging_GeoOptixWellStagingID");
        });

        modelBuilder.Entity<OpenETSync>(entity =>
        {
            entity.HasKey(e => e.OpenETSyncID).HasName("PK_OpenET_OpenETSyncID");
        });

        modelBuilder.Entity<OpenETSyncHistory>(entity =>
        {
            entity.HasKey(e => e.OpenETSyncHistoryID).HasName("PK_OpenETSyncHistory_OpenETSyncHistoryID");
        });

        modelBuilder.Entity<OpenETWaterMeasurement>(entity =>
        {
            entity.HasKey(e => e.OpenETWaterMeasurementID).HasName("PK_OpenETWaterMeasurement_OpenETWaterMeasurementID");
        });

        modelBuilder.Entity<PaigeWirelessPulse>(entity =>
        {
            entity.HasKey(e => e.PaigeWirelessPulseID).HasName("PK_PaigeWirelessPulse_WellID");
        });

        modelBuilder.Entity<PrismDailyRecord>(entity =>
        {
            entity.HasKey(e => e.PrismDailyRecordID).HasName("PK_PrismDailyRecord_PrismDailyRecordID");

            entity.Property(e => e.PrismSyncStatusID).HasDefaultValue(1);

            entity.HasOne(d => d.BlobResource).WithMany(p => p.PrismDailyRecords).HasConstraintName("FK_PrismDailyRecord_BlobResourceID");

            entity.HasOne(d => d.PrismMonthlySync).WithMany(p => p.PrismDailyRecords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrismDailyRecord_PrismMonthlySyncID");
        });

        modelBuilder.Entity<PrismMonthlySync>(entity =>
        {
            entity.HasKey(e => e.PrismMonthlySyncID).HasName("PK_PrismMonthlySync_PrismMonthlySyncID");

            entity.Property(e => e.PrismSyncStatusID).HasDefaultValue(1);
            entity.Property(e => e.RunoffCalculationStatusID).HasDefaultValue(1);

            entity.HasOne(d => d.FinalizeByUser).WithMany(p => p.PrismMonthlySyncFinalizeByUsers).HasConstraintName("FK_PrismMonthlySync_FinalizeByUserID");

            entity.HasOne(d => d.LastRunoffCalculatedByUser).WithMany(p => p.PrismMonthlySyncLastRunoffCalculatedByUsers).HasConstraintName("FK_PrismMonthlySync_LastRunoffCalculatedByUserID");

            entity.HasOne(d => d.LastSynchronizedByUser).WithMany(p => p.PrismMonthlySyncLastSynchronizedByUsers).HasConstraintName("FK_PrismMonthlySync_LastSynchronizedByUserID");
        });

        modelBuilder.Entity<ReportTemplate>(entity =>
        {
            entity.HasKey(e => e.ReportTemplateID).HasName("PK_ReportTemplate_ReportTemplateID");

            entity.HasOne(d => d.FileResource).WithMany(p => p.ReportTemplates).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<RobustReviewScenarioGETRunHistory>(entity =>
        {
            entity.HasKey(e => e.RobustReviewScenarioGETRunHistoryID).HasName("PK_RobustReviewScenarioGETRunHistory_RobustReviewScenarioGETRunHistoryID");

            entity.HasOne(d => d.CreateByUser).WithMany(p => p.RobustReviewScenarioGETRunHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RobustReviewScenarioGETRunHistory_User_CreateByUserID_UserID");
        });

        modelBuilder.Entity<Sensor>(entity =>
        {
            entity.HasKey(e => e.SensorID).HasName("PK_Sensor_SensorID");
        });

        modelBuilder.Entity<SensorAnomaly>(entity =>
        {
            entity.HasKey(e => e.SensorAnomalyID).HasName("PK_SensorAnomaly_SensorAnomalyID");

            entity.HasOne(d => d.Sensor).WithMany(p => p.SensorAnomalies).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<StreamFlowZone>(entity =>
        {
            entity.HasKey(e => e.StreamFlowZoneID).HasName("PK_StreamFlowZone_StreamFlowZoneID");
        });

        modelBuilder.Entity<SupportTicket>(entity =>
        {
            entity.HasKey(e => e.SupportTicketID).HasName("PK_SupportTicket_SupportTicketID");

            entity.HasOne(d => d.AssigneeUser).WithMany(p => p.SupportTicketAssigneeUsers).HasConstraintName("FK_SupportTicket_User_AssigneeUserID_UserID");

            entity.HasOne(d => d.CreatorUser).WithMany(p => p.SupportTicketCreatorUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupportTicket_User_CreatorUserID_UserID");

            entity.HasOne(d => d.Well).WithMany(p => p.SupportTickets).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<SupportTicketComment>(entity =>
        {
            entity.HasKey(e => e.SupportTicketCommentID).HasName("PK_SupportTicketComment_SupportTicketCommentID");

            entity.HasOne(d => d.CreatorUser).WithMany(p => p.SupportTicketComments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SupportTicketComment_User_CreatorUserID_UserID");

            entity.HasOne(d => d.SupportTicket).WithMany(p => p.SupportTicketComments).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<SupportTicketNotification>(entity =>
        {
            entity.HasKey(e => e.SupportTicketNotificationID).HasName("PK_SupportTicketNotification_SupportTicketNotificationID");

            entity.HasOne(d => d.SupportTicket).WithMany(p => p.SupportTicketNotifications).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Tillage>(entity =>
        {
            entity.HasKey(e => e.TillageID).HasName("PK_Tillage_TillageID");

            entity.Property(e => e.TillageID).ValueGeneratedNever();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserID).HasName("PK_User_UserID");
        });

        modelBuilder.Entity<WaterLevelInspection>(entity =>
        {
            entity.HasKey(e => e.WaterLevelInspectionID).HasName("PK_WaterLevelInspection_WaterLevelInspectionID");

            entity.HasOne(d => d.InspectorUser).WithMany(p => p.WaterLevelInspections)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WaterLevelInspection_User_InspectorUserID_UserID");

            entity.HasOne(d => d.Well).WithMany(p => p.WaterLevelInspections).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<WaterLevelMeasuringEquipment>(entity =>
        {
            entity.HasKey(e => e.WaterLevelMeasuringEquipmentID).HasName("PK_WaterLevelMeasuringEquipment_WaterLevelMeasuringEquipmentID");

            entity.Property(e => e.WaterLevelMeasuringEquipmentID).ValueGeneratedNever();
        });

        modelBuilder.Entity<WaterQualityInspection>(entity =>
        {
            entity.HasKey(e => e.WaterQualityInspectionID).HasName("PK_WaterQualityInspection_WaterQualityInspectionID");

            entity.HasOne(d => d.InspectorUser).WithMany(p => p.WaterQualityInspections)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WaterQualityInspection_User_InspectorUserID_UserID");

            entity.HasOne(d => d.Well).WithMany(p => p.WaterQualityInspections).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Well>(entity =>
        {
            entity.HasKey(e => e.WellID).HasName("PK_Well_WellID");

            entity.HasOne(d => d.StreamflowZone).WithMany(p => p.Wells).HasConstraintName("FK_Well_StreamFlowZone_StreamFlowZoneID");
        });

        modelBuilder.Entity<WellGroup>(entity =>
        {
            entity.HasKey(e => e.WellGroupID).HasName("PK_WellGroup_WellID");
        });

        modelBuilder.Entity<WellGroupWell>(entity =>
        {
            entity.HasKey(e => e.WellGroupWellID).HasName("PK_WellGroupWell_WellGroupWellID");

            entity.HasOne(d => d.WellGroup).WithMany(p => p.WellGroupWells).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Well).WithMany(p => p.WellGroupWells).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<WellParticipation>(entity =>
        {
            entity.HasKey(e => e.WellParticipationID).HasName("PK_WellParticipation_WellParticipationID");

            entity.Property(e => e.WellParticipationID).ValueGeneratedNever();
        });

        modelBuilder.Entity<WellSensorMeasurement>(entity =>
        {
            entity.HasKey(e => e.WellSensorMeasurementID).HasName("PK_WellSensorMeasurement_WellSensorMeasurementID");
        });

        modelBuilder.Entity<WellSensorMeasurementStaging>(entity =>
        {
            entity.HasKey(e => e.WellSensorMeasurementStagingID).HasName("PK_WellSensorMeasurementStaging_WellSensorMeasurementStagingID");
        });

        modelBuilder.Entity<WellWaterQualityInspectionType>(entity =>
        {
            entity.HasKey(e => e.WellWaterQualityInspectionTypeID).HasName("PK_WellWaterQualityInspectionType_WellWaterQualityInspectionTypeID");

            entity.HasOne(d => d.Well).WithMany(p => p.WellWaterQualityInspectionTypes).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<vGeoServerAgHubIrrigationUnit>(entity =>
        {
            entity.ToView("vGeoServerAgHubIrrigationUnit");
        });

        modelBuilder.Entity<vOpenETMostRecentSyncHistoryForYearAndMonth>(entity =>
        {
            entity.ToView("vOpenETMostRecentSyncHistoryForYearAndMonth");
        });

        modelBuilder.Entity<vSensor>(entity =>
        {
            entity.ToView("vSensor");
        });

        modelBuilder.Entity<vSensorLatestBatteryVoltage>(entity =>
        {
            entity.ToView("vSensorLatestBatteryVoltage");
        });

        modelBuilder.Entity<vWellSensorMeasurementFirstAndLatestForSensor>(entity =>
        {
            entity.ToView("vWellSensorMeasurementFirstAndLatestForSensor");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
