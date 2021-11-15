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
        public virtual DbSet<ChemigationCounty> ChemigationCounties { get; set; }
        public virtual DbSet<ChemigationInspection> ChemigationInspections { get; set; }
        public virtual DbSet<ChemigationPermit> ChemigationPermits { get; set; }
        public virtual DbSet<ChemigationPermitAnnualRecord> ChemigationPermitAnnualRecords { get; set; }
        public virtual DbSet<ChemigationPermitAnnualRecordStatus> ChemigationPermitAnnualRecordStatuses { get; set; }
        public virtual DbSet<ChemigationPermitStatus> ChemigationPermitStatuses { get; set; }
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
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Sensor> Sensors { get; set; }
        public virtual DbSet<SensorType> SensorTypes { get; set; }
        public virtual DbSet<StreamFlowZone> StreamFlowZones { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Well> Wells { get; set; }
        public virtual DbSet<WellSensorMeasurement> WellSensorMeasurements { get; set; }
        public virtual DbSet<WellSensorMeasurementStaging> WellSensorMeasurementStagings { get; set; }

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

            modelBuilder.Entity<ChemigationCounty>(entity =>
            {
                entity.Property(e => e.ChemigationCountyDisplayName).IsUnicode(false);

                entity.Property(e => e.ChemigationCountyName).IsUnicode(false);
            });

            modelBuilder.Entity<ChemigationInspection>(entity =>
            {
                entity.Property(e => e.ProtocolCanonicalName).IsUnicode(false);

                entity.Property(e => e.Status).IsUnicode(false);

                entity.Property(e => e.WellRegistrationID).IsUnicode(false);
            });

            modelBuilder.Entity<ChemigationPermit>(entity =>
            {
                entity.Property(e => e.TownshipRangeSection).IsUnicode(false);

                entity.HasOne(d => d.ChemigationCountyNavigation)
                    .WithMany(p => p.ChemigationPermits)
                    .HasForeignKey(d => d.ChemigationCounty)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChemigationPermit_ChemigationCounty_ChemigationCountyID");

                entity.HasOne(d => d.ChemigationPermitStatus)
                    .WithMany(p => p.ChemigationPermits)
                    .HasForeignKey(d => d.ChemigationPermitStatusID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ChemigationPermitAnnualRecord>(entity =>
            {
                entity.Property(e => e.ApplicantCity).IsUnicode(false);

                entity.Property(e => e.ApplicantFirstName).IsUnicode(false);

                entity.Property(e => e.ApplicantLastName).IsUnicode(false);

                entity.Property(e => e.ApplicantMailingAddress).IsUnicode(false);

                entity.Property(e => e.ApplicantState).IsUnicode(false);

                entity.Property(e => e.PivotName).IsUnicode(false);

                entity.HasOne(d => d.ChemigationPermitAnnualRecordStatus)
                    .WithMany(p => p.ChemigationPermitAnnualRecords)
                    .HasForeignKey(d => d.ChemigationPermitAnnualRecordStatusID)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ChemigationPermit)
                    .WithMany(p => p.ChemigationPermitAnnualRecords)
                    .HasForeignKey(d => d.ChemigationPermitID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ChemigationPermitAnnualRecordStatus>(entity =>
            {
                entity.Property(e => e.ChemigationPermitAnnualRecordStatusDisplayName).IsUnicode(false);

                entity.Property(e => e.ChemigationPermitAnnualRecordStatusName).IsUnicode(false);
            });

            modelBuilder.Entity<ChemigationPermitStatus>(entity =>
            {
                entity.Property(e => e.ChemigationPermitStatusDisplayName).IsUnicode(false);

                entity.Property(e => e.ChemigationPermitStatusName).IsUnicode(false);
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

            modelBuilder.Entity<Well>(entity =>
            {
                entity.Property(e => e.WellRegistrationID).IsUnicode(false);

                entity.HasOne(d => d.StreamflowZone)
                    .WithMany(p => p.Wells)
                    .HasForeignKey(d => d.StreamflowZoneID)
                    .HasConstraintName("FK_Well_StreamFlowZone_StreamFlowZoneID");
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
