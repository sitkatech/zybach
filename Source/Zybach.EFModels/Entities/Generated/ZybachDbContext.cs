using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

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

        public virtual DbSet<AgHubWell> AgHubWells { get; set; }
        public virtual DbSet<AgHubWellIrrigatedAcre> AgHubWellIrrigatedAcres { get; set; }
        public virtual DbSet<AgHubWellIrrigatedAcreStaging> AgHubWellIrrigatedAcreStagings { get; set; }
        public virtual DbSet<AgHubWellStaging> AgHubWellStagings { get; set; }
        public virtual DbSet<ChemicalFormulation> ChemicalFormulations { get; set; }
        public virtual DbSet<ChemicalUnit> ChemicalUnits { get; set; }
        public virtual DbSet<ChemigationInjectionUnitType> ChemigationInjectionUnitTypes { get; set; }
        public virtual DbSet<ChemigationInjectionValve> ChemigationInjectionValves { get; set; }
        public virtual DbSet<ChemigationInspection> ChemigationInspections { get; set; }
        public virtual DbSet<ChemigationInspectionFailureReason> ChemigationInspectionFailureReasons { get; set; }
        public virtual DbSet<ChemigationInspectionStatus> ChemigationInspectionStatuses { get; set; }
        public virtual DbSet<ChemigationInspectionType> ChemigationInspectionTypes { get; set; }
        public virtual DbSet<ChemigationInterlockType> ChemigationInterlockTypes { get; set; }
        public virtual DbSet<ChemigationLowPressureValve> ChemigationLowPressureValves { get; set; }
        public virtual DbSet<ChemigationMainlineCheckValve> ChemigationMainlineCheckValves { get; set; }
        public virtual DbSet<ChemigationPermit> ChemigationPermits { get; set; }
        public virtual DbSet<ChemigationPermitAnnualRecord> ChemigationPermitAnnualRecords { get; set; }
        public virtual DbSet<ChemigationPermitAnnualRecordApplicator> ChemigationPermitAnnualRecordApplicators { get; set; }
        public virtual DbSet<ChemigationPermitAnnualRecordChemicalFormulation> ChemigationPermitAnnualRecordChemicalFormulations { get; set; }
        public virtual DbSet<ChemigationPermitAnnualRecordFeeType> ChemigationPermitAnnualRecordFeeTypes { get; set; }
        public virtual DbSet<ChemigationPermitAnnualRecordStatus> ChemigationPermitAnnualRecordStatuses { get; set; }
        public virtual DbSet<ChemigationPermitStatus> ChemigationPermitStatuses { get; set; }
        public virtual DbSet<County> Counties { get; set; }
        public virtual DbSet<CropType> CropTypes { get; set; }
        public virtual DbSet<CustomRichText> CustomRichTexts { get; set; }
        public virtual DbSet<CustomRichTextType> CustomRichTextTypes { get; set; }
        public virtual DbSet<DatabaseMigration> DatabaseMigrations { get; set; }
        public virtual DbSet<FieldDefinition> FieldDefinitions { get; set; }
        public virtual DbSet<FieldDefinitionType> FieldDefinitionTypes { get; set; }
        public virtual DbSet<FileResource> FileResources { get; set; }
        public virtual DbSet<FileResourceMimeType> FileResourceMimeTypes { get; set; }
        public virtual DbSet<GeoOptixSensorStaging> GeoOptixSensorStagings { get; set; }
        public virtual DbSet<GeoOptixWell> GeoOptixWells { get; set; }
        public virtual DbSet<GeoOptixWellStaging> GeoOptixWellStagings { get; set; }
        public virtual DbSet<MeasurementType> MeasurementTypes { get; set; }
        public virtual DbSet<ReportTemplate> ReportTemplates { get; set; }
        public virtual DbSet<ReportTemplateModel> ReportTemplateModels { get; set; }
        public virtual DbSet<ReportTemplateModelType> ReportTemplateModelTypes { get; set; }
        public virtual DbSet<RobustReviewScenarioGETRunHistory> RobustReviewScenarioGETRunHistories { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Sensor> Sensors { get; set; }
        public virtual DbSet<SensorType> SensorTypes { get; set; }
        public virtual DbSet<StreamFlowZone> StreamFlowZones { get; set; }
        public virtual DbSet<Tillage> Tillages { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<WaterLevelInspection> WaterLevelInspections { get; set; }
        public virtual DbSet<WaterLevelMeasuringEquipment> WaterLevelMeasuringEquipments { get; set; }
        public virtual DbSet<WaterQualityInspection> WaterQualityInspections { get; set; }
        public virtual DbSet<WaterQualityInspectionType> WaterQualityInspectionTypes { get; set; }
        public virtual DbSet<Well> Wells { get; set; }
        public virtual DbSet<WellParticipation> WellParticipations { get; set; }
        public virtual DbSet<WellSensorMeasurement> WellSensorMeasurements { get; set; }
        public virtual DbSet<WellSensorMeasurementStaging> WellSensorMeasurementStagings { get; set; }
        public virtual DbSet<WellUse> WellUses { get; set; }
        public virtual DbSet<WellWaterQualityInspectionType> WellWaterQualityInspectionTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AgHubWell>(entity =>
            {
                entity.Property(e => e.AgHubRegisteredUser).IsUnicode(false);

                entity.Property(e => e.FieldName).IsUnicode(false);

                entity.Property(e => e.WellTPID).IsUnicode(false);

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

            modelBuilder.Entity<AgHubWellIrrigatedAcreStaging>(entity =>
            {
                entity.Property(e => e.WellRegistrationID).IsUnicode(false);
            });

            modelBuilder.Entity<AgHubWellStaging>(entity =>
            {
                entity.Property(e => e.AgHubRegisteredUser).IsUnicode(false);

                entity.Property(e => e.FieldName).IsUnicode(false);

                entity.Property(e => e.WellRegistrationID).IsUnicode(false);

                entity.Property(e => e.WellTPID).IsUnicode(false);
            });

            modelBuilder.Entity<ChemicalFormulation>(entity =>
            {
                entity.Property(e => e.ChemicalFormulationDisplayName).IsUnicode(false);

                entity.Property(e => e.ChemicalFormulationName).IsUnicode(false);
            });

            modelBuilder.Entity<ChemicalUnit>(entity =>
            {
                entity.Property(e => e.ChemicalUnitID).ValueGeneratedNever();

                entity.Property(e => e.ChemicalUnitLowercaseShortName).IsUnicode(false);

                entity.Property(e => e.ChemicalUnitName).IsUnicode(false);

                entity.Property(e => e.ChemicalUnitPluralName).IsUnicode(false);
            });

            modelBuilder.Entity<ChemigationInjectionUnitType>(entity =>
            {
                entity.Property(e => e.ChemigationInjectionUnitTypeID).ValueGeneratedNever();

                entity.Property(e => e.ChemigationInjectionUnitTypeDisplayName).IsUnicode(false);

                entity.Property(e => e.ChemigationInjectionUnitTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<ChemigationInjectionValve>(entity =>
            {
                entity.Property(e => e.ChemigationInjectionValveID).ValueGeneratedNever();

                entity.Property(e => e.ChemigationInjectionValveDisplayName).IsUnicode(false);

                entity.Property(e => e.ChemigationInjectionValveName).IsUnicode(false);
            });

            modelBuilder.Entity<ChemigationInspection>(entity =>
            {
                entity.Property(e => e.InspectionNotes).IsUnicode(false);

                entity.HasOne(d => d.ChemigationInspectionStatus)
                    .WithMany(p => p.ChemigationInspections)
                    .HasForeignKey(d => d.ChemigationInspectionStatusID)
                    .OnDelete(DeleteBehavior.ClientSetNull);

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

                entity.Property(e => e.ChemigationInspectionFailureReasonDisplayName).IsUnicode(false);

                entity.Property(e => e.ChemigationInspectionFailureReasonName).IsUnicode(false);
            });

            modelBuilder.Entity<ChemigationInspectionStatus>(entity =>
            {
                entity.Property(e => e.ChemigationInspectionStatusID).ValueGeneratedNever();

                entity.Property(e => e.ChemigationInspectionStatusDisplayName).IsUnicode(false);

                entity.Property(e => e.ChemigationInspectionStatusName).IsUnicode(false);
            });

            modelBuilder.Entity<ChemigationInspectionType>(entity =>
            {
                entity.Property(e => e.ChemigationInspectionTypeID).ValueGeneratedNever();

                entity.Property(e => e.ChemigationInspectionTypeDisplayName).IsUnicode(false);

                entity.Property(e => e.ChemigationInspectionTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<ChemigationInterlockType>(entity =>
            {
                entity.Property(e => e.ChemigationInterlockTypeID).ValueGeneratedNever();

                entity.Property(e => e.ChemigationInterlockTypeDisplayName).IsUnicode(false);

                entity.Property(e => e.ChemigationInterlockTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<ChemigationLowPressureValve>(entity =>
            {
                entity.Property(e => e.ChemigationLowPressureValveID).ValueGeneratedNever();

                entity.Property(e => e.ChemigationLowPressureValveDisplayName).IsUnicode(false);

                entity.Property(e => e.ChemigationLowPressureValveName).IsUnicode(false);
            });

            modelBuilder.Entity<ChemigationMainlineCheckValve>(entity =>
            {
                entity.Property(e => e.ChemigationMainlineCheckValveID).ValueGeneratedNever();

                entity.Property(e => e.ChemigationMainlineCheckValveDisplayName).IsUnicode(false);

                entity.Property(e => e.ChemigationMainlineCheckValveName).IsUnicode(false);
            });

            modelBuilder.Entity<ChemigationPermit>(entity =>
            {
                entity.HasOne(d => d.ChemigationPermitStatus)
                    .WithMany(p => p.ChemigationPermits)
                    .HasForeignKey(d => d.ChemigationPermitStatusID)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.County)
                    .WithMany(p => p.ChemigationPermits)
                    .HasForeignKey(d => d.CountyID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ChemigationPermitAnnualRecord>(entity =>
            {
                entity.Property(e => e.AnnualNotes).IsUnicode(false);

                entity.Property(e => e.ApplicantCity).IsUnicode(false);

                entity.Property(e => e.ApplicantCompany).IsUnicode(false);

                entity.Property(e => e.ApplicantEmail).IsUnicode(false);

                entity.Property(e => e.ApplicantFirstName).IsUnicode(false);

                entity.Property(e => e.ApplicantLastName).IsUnicode(false);

                entity.Property(e => e.ApplicantMailingAddress).IsUnicode(false);

                entity.Property(e => e.ApplicantMobilePhone).IsUnicode(false);

                entity.Property(e => e.ApplicantPhone).IsUnicode(false);

                entity.Property(e => e.ApplicantState).IsUnicode(false);

                entity.Property(e => e.ApplicantZipCode).IsUnicode(false);

                entity.Property(e => e.PivotName).IsUnicode(false);

                entity.Property(e => e.TownshipRangeSection).IsUnicode(false);

                entity.HasOne(d => d.ChemigationInjectionUnitType)
                    .WithMany(p => p.ChemigationPermitAnnualRecords)
                    .HasForeignKey(d => d.ChemigationInjectionUnitTypeID)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ChemigationPermitAnnualRecordStatus)
                    .WithMany(p => p.ChemigationPermitAnnualRecords)
                    .HasForeignKey(d => d.ChemigationPermitAnnualRecordStatusID)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ChemigationPermit)
                    .WithMany(p => p.ChemigationPermitAnnualRecords)
                    .HasForeignKey(d => d.ChemigationPermitID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ChemigationPermitAnnualRecordApplicator>(entity =>
            {
                entity.Property(e => e.ApplicatorName).IsUnicode(false);

                entity.Property(e => e.HomePhone).IsUnicode(false);

                entity.Property(e => e.MobilePhone).IsUnicode(false);

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

            modelBuilder.Entity<ChemigationPermitAnnualRecordFeeType>(entity =>
            {
                entity.Property(e => e.ChemigationPermitAnnualRecordFeeTypeID).ValueGeneratedNever();

                entity.Property(e => e.ChemigationPermitAnnualRecordFeeTypeDisplayName).IsUnicode(false);

                entity.Property(e => e.ChemigationPermitAnnualRecordFeeTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<ChemigationPermitAnnualRecordStatus>(entity =>
            {
                entity.Property(e => e.ChemigationPermitAnnualRecordStatusID).ValueGeneratedNever();

                entity.Property(e => e.ChemigationPermitAnnualRecordStatusDisplayName).IsUnicode(false);

                entity.Property(e => e.ChemigationPermitAnnualRecordStatusName).IsUnicode(false);
            });

            modelBuilder.Entity<ChemigationPermitStatus>(entity =>
            {
                entity.Property(e => e.ChemigationPermitStatusDisplayName).IsUnicode(false);

                entity.Property(e => e.ChemigationPermitStatusName).IsUnicode(false);
            });

            modelBuilder.Entity<County>(entity =>
            {
                entity.Property(e => e.CountyDisplayName).IsUnicode(false);

                entity.Property(e => e.CountyName).IsUnicode(false);
            });

            modelBuilder.Entity<CropType>(entity =>
            {
                entity.Property(e => e.CropTypeID).ValueGeneratedNever();

                entity.Property(e => e.CropTypeDisplayName).IsUnicode(false);

                entity.Property(e => e.CropTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<CustomRichText>(entity =>
            {
                entity.Property(e => e.CustomRichTextContent).IsUnicode(false);

                entity.HasOne(d => d.CustomRichTextType)
                    .WithMany(p => p.CustomRichTexts)
                    .HasForeignKey(d => d.CustomRichTextTypeID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<CustomRichTextType>(entity =>
            {
                entity.Property(e => e.CustomRichTextTypeID).ValueGeneratedNever();

                entity.Property(e => e.CustomRichTextTypeDisplayName).IsUnicode(false);

                entity.Property(e => e.CustomRichTextTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<DatabaseMigration>(entity =>
            {
                entity.HasKey(e => e.DatabaseMigrationNumber)
                    .HasName("PK_DatabaseMigration_DatabaseMigrationNumber");

                entity.Property(e => e.DatabaseMigrationNumber).ValueGeneratedNever();
            });

            modelBuilder.Entity<FieldDefinition>(entity =>
            {
                entity.Property(e => e.FieldDefinitionValue).IsUnicode(false);

                entity.HasOne(d => d.FieldDefinitionType)
                    .WithMany(p => p.FieldDefinitions)
                    .HasForeignKey(d => d.FieldDefinitionTypeID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<FieldDefinitionType>(entity =>
            {
                entity.Property(e => e.FieldDefinitionTypeID).ValueGeneratedNever();

                entity.Property(e => e.FieldDefinitionTypeDisplayName).IsUnicode(false);

                entity.Property(e => e.FieldDefinitionTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<FileResource>(entity =>
            {
                entity.Property(e => e.OriginalBaseFilename).IsUnicode(false);

                entity.Property(e => e.OriginalFileExtension).IsUnicode(false);

                entity.HasOne(d => d.CreateUser)
                    .WithMany(p => p.FileResources)
                    .HasForeignKey(d => d.CreateUserID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FileResource_User_CreateUserID_UserID");

                entity.HasOne(d => d.FileResourceMimeType)
                    .WithMany(p => p.FileResources)
                    .HasForeignKey(d => d.FileResourceMimeTypeID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<FileResourceMimeType>(entity =>
            {
                entity.Property(e => e.FileResourceMimeTypeID).ValueGeneratedNever();

                entity.Property(e => e.FileResourceMimeTypeContentTypeName).IsUnicode(false);

                entity.Property(e => e.FileResourceMimeTypeDisplayName).IsUnicode(false);

                entity.Property(e => e.FileResourceMimeTypeIconNormalFilename).IsUnicode(false);

                entity.Property(e => e.FileResourceMimeTypeIconSmallFilename).IsUnicode(false);

                entity.Property(e => e.FileResourceMimeTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<GeoOptixSensorStaging>(entity =>
            {
                entity.Property(e => e.SensorName).IsUnicode(false);

                entity.Property(e => e.SensorType).IsUnicode(false);

                entity.Property(e => e.WellRegistrationID).IsUnicode(false);
            });

            modelBuilder.Entity<GeoOptixWell>(entity =>
            {
                entity.HasOne(d => d.Well)
                    .WithOne(p => p.GeoOptixWell)
                    .HasForeignKey<GeoOptixWell>(d => d.WellID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<GeoOptixWellStaging>(entity =>
            {
                entity.Property(e => e.WellRegistrationID).IsUnicode(false);
            });

            modelBuilder.Entity<MeasurementType>(entity =>
            {
                entity.Property(e => e.MeasurementTypeID).ValueGeneratedNever();

                entity.Property(e => e.MeasurementTypeDisplayName).IsUnicode(false);

                entity.Property(e => e.MeasurementTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<ReportTemplate>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.DisplayName).IsUnicode(false);

                entity.HasOne(d => d.FileResource)
                    .WithMany(p => p.ReportTemplates)
                    .HasForeignKey(d => d.FileResourceID)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ReportTemplateModel)
                    .WithMany(p => p.ReportTemplates)
                    .HasForeignKey(d => d.ReportTemplateModelID)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ReportTemplateModelType)
                    .WithMany(p => p.ReportTemplates)
                    .HasForeignKey(d => d.ReportTemplateModelTypeID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ReportTemplateModel>(entity =>
            {
                entity.Property(e => e.ReportTemplateModelID).ValueGeneratedNever();

                entity.Property(e => e.ReportTemplateModelDescription).IsUnicode(false);

                entity.Property(e => e.ReportTemplateModelDisplayName).IsUnicode(false);

                entity.Property(e => e.ReportTemplateModelName).IsUnicode(false);
            });

            modelBuilder.Entity<ReportTemplateModelType>(entity =>
            {
                entity.Property(e => e.ReportTemplateModelTypeID).ValueGeneratedNever();

                entity.Property(e => e.ReportTemplateModelTypeDescription).IsUnicode(false);

                entity.Property(e => e.ReportTemplateModelTypeDisplayName).IsUnicode(false);

                entity.Property(e => e.ReportTemplateModelTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<RobustReviewScenarioGETRunHistory>(entity =>
            {
                entity.Property(e => e.StatusHexColor).IsUnicode(false);

                entity.Property(e => e.StatusMessage).IsUnicode(false);

                entity.HasOne(d => d.CreateByUser)
                    .WithMany(p => p.RobustReviewScenarioGETRunHistories)
                    .HasForeignKey(d => d.CreateByUserID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RobustReviewScenarioGETRunHistory_User_CreateByUserID_UserID");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleID).ValueGeneratedNever();

                entity.Property(e => e.RoleDescription).IsUnicode(false);

                entity.Property(e => e.RoleDisplayName).IsUnicode(false);

                entity.Property(e => e.RoleName).IsUnicode(false);
            });

            modelBuilder.Entity<Sensor>(entity =>
            {
                entity.Property(e => e.SensorName).IsUnicode(false);
            });

            modelBuilder.Entity<SensorType>(entity =>
            {
                entity.Property(e => e.SensorTypeID).ValueGeneratedNever();

                entity.Property(e => e.SensorTypeDisplayName).IsUnicode(false);

                entity.Property(e => e.SensorTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<StreamFlowZone>(entity =>
            {
                entity.Property(e => e.StreamFlowZoneName).IsUnicode(false);
            });

            modelBuilder.Entity<Tillage>(entity =>
            {
                entity.Property(e => e.TillageID).ValueGeneratedNever();

                entity.Property(e => e.TillageDisplayName).IsUnicode(false);

                entity.Property(e => e.TillageName).IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Company).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.LoginName).IsUnicode(false);

                entity.Property(e => e.Phone).IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<WaterLevelInspection>(entity =>
            {
                entity.Property(e => e.Access).IsUnicode(false);

                entity.Property(e => e.Accuracy).IsUnicode(false);

                entity.Property(e => e.AgencyCode).IsUnicode(false);

                entity.Property(e => e.Crop).IsUnicode(false);

                entity.Property(e => e.InspectionNickname).IsUnicode(false);

                entity.Property(e => e.InspectionNotes).IsUnicode(false);

                entity.Property(e => e.LevelTypeCode).IsUnicode(false);

                entity.Property(e => e.MeasuringEquipment).IsUnicode(false);

                entity.Property(e => e.Method).IsUnicode(false);

                entity.Property(e => e.Party).IsUnicode(false);

                entity.Property(e => e.SourceAgency).IsUnicode(false);

                entity.Property(e => e.SourceCode).IsUnicode(false);

                entity.Property(e => e.TimeDatumCode).IsUnicode(false);

                entity.Property(e => e.TimeDatumReliability).IsUnicode(false);

                entity.Property(e => e.WaterLevelInspectionStatus).IsUnicode(false);

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
                entity.Property(e => e.WaterLevelMeasuringEquipmentDisplayName).IsUnicode(false);

                entity.Property(e => e.WaterLevelMeasuringEquipmentName).IsUnicode(false);
            });

            modelBuilder.Entity<WaterQualityInspection>(entity =>
            {
                entity.Property(e => e.InspectionNickname).IsUnicode(false);

                entity.Property(e => e.InspectionNotes).IsUnicode(false);

                entity.HasOne(d => d.InspectorUser)
                    .WithMany(p => p.WaterQualityInspections)
                    .HasForeignKey(d => d.InspectorUserID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WaterQualityInspection_User_InspectorUserID_UserID");

                entity.HasOne(d => d.WaterQualityInspectionType)
                    .WithMany(p => p.WaterQualityInspections)
                    .HasForeignKey(d => d.WaterQualityInspectionTypeID)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Well)
                    .WithMany(p => p.WaterQualityInspections)
                    .HasForeignKey(d => d.WellID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<WaterQualityInspectionType>(entity =>
            {
                entity.Property(e => e.WaterQualityInspectionTypeID).ValueGeneratedNever();

                entity.Property(e => e.WaterQualityInspectionTypeDisplayName).IsUnicode(false);

                entity.Property(e => e.WaterQualityInspectionTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<Well>(entity =>
            {
                entity.Property(e => e.AdditionalContactAddress).IsUnicode(false);

                entity.Property(e => e.AdditionalContactCity).IsUnicode(false);

                entity.Property(e => e.AdditionalContactName).IsUnicode(false);

                entity.Property(e => e.AdditionalContactState).IsUnicode(false);

                entity.Property(e => e.AdditionalContactZipCode).IsUnicode(false);

                entity.Property(e => e.Clearinghouse).IsUnicode(false);

                entity.Property(e => e.Notes).IsUnicode(false);

                entity.Property(e => e.OwnerAddress).IsUnicode(false);

                entity.Property(e => e.OwnerCity).IsUnicode(false);

                entity.Property(e => e.OwnerName).IsUnicode(false);

                entity.Property(e => e.OwnerState).IsUnicode(false);

                entity.Property(e => e.OwnerZipCode).IsUnicode(false);

                entity.Property(e => e.ScreenInterval).IsUnicode(false);

                entity.Property(e => e.SiteName).IsUnicode(false);

                entity.Property(e => e.SiteNumber).IsUnicode(false);

                entity.Property(e => e.TownshipRangeSection).IsUnicode(false);

                entity.Property(e => e.WellNickname).IsUnicode(false);

                entity.Property(e => e.WellRegistrationID).IsUnicode(false);

                entity.HasOne(d => d.StreamflowZone)
                    .WithMany(p => p.Wells)
                    .HasForeignKey(d => d.StreamflowZoneID)
                    .HasConstraintName("FK_Well_StreamFlowZone_StreamFlowZoneID");
            });

            modelBuilder.Entity<WellParticipation>(entity =>
            {
                entity.Property(e => e.WellParticipationID).ValueGeneratedNever();

                entity.Property(e => e.WellParticipationDisplayName).IsUnicode(false);

                entity.Property(e => e.WellParticipationName).IsUnicode(false);
            });

            modelBuilder.Entity<WellSensorMeasurement>(entity =>
            {
                entity.Property(e => e.SensorName).IsUnicode(false);

                entity.Property(e => e.WellRegistrationID).IsUnicode(false);

                entity.HasOne(d => d.MeasurementType)
                    .WithMany(p => p.WellSensorMeasurements)
                    .HasForeignKey(d => d.MeasurementTypeID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<WellSensorMeasurementStaging>(entity =>
            {
                entity.Property(e => e.SensorName).IsUnicode(false);

                entity.Property(e => e.WellRegistrationID).IsUnicode(false);

                entity.HasOne(d => d.MeasurementType)
                    .WithMany(p => p.WellSensorMeasurementStagings)
                    .HasForeignKey(d => d.MeasurementTypeID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<WellUse>(entity =>
            {
                entity.Property(e => e.WellUseID).ValueGeneratedNever();

                entity.Property(e => e.WellUseDisplayName).IsUnicode(false);

                entity.Property(e => e.WellUseName).IsUnicode(false);
            });

            modelBuilder.Entity<WellWaterQualityInspectionType>(entity =>
            {
                entity.HasOne(d => d.WaterQualityInspectionType)
                    .WithMany(p => p.WellWaterQualityInspectionTypes)
                    .HasForeignKey(d => d.WaterQualityInspectionTypeID)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Well)
                    .WithMany(p => p.WellWaterQualityInspectionTypes)
                    .HasForeignKey(d => d.WellID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
