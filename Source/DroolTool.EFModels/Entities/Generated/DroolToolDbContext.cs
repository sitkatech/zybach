using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DroolTool.EFModels.Entities
{
    public partial class DroolToolDbContext : DbContext
    {
        public DroolToolDbContext()
        {
        }

        public DroolToolDbContext(DbContextOptions<DroolToolDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BackboneSegment> BackboneSegment { get; set; }
        public virtual DbSet<BackboneSegmentType> BackboneSegmentType { get; set; }
        public virtual DbSet<CustomRichText> CustomRichText { get; set; }
        public virtual DbSet<CustomRichTextType> CustomRichTextType { get; set; }
        public virtual DbSet<DatabaseMigration> DatabaseMigration { get; set; }
        public virtual DbSet<FileResource> FileResource { get; set; }
        public virtual DbSet<FileResourceMimeType> FileResourceMimeType { get; set; }
        public virtual DbSet<Neighborhood> Neighborhood { get; set; }
        public virtual DbSet<RawDroolMetric> RawDroolMetric { get; set; }
        public virtual DbSet<RegionalSubbasin> RegionalSubbasin { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Watershed> Watershed { get; set; }
        public virtual DbSet<vGeoServerBackbone> vGeoServerBackbone { get; set; }
        public virtual DbSet<vGeoServerNeighborhood> vGeoServerNeighborhood { get; set; }
        public virtual DbSet<vGeoServerWatershed> vGeoServerWatershed { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            {

                
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BackboneSegment>(entity =>
            {
                entity.Property(e => e.StreamName).IsUnicode(false);

                entity.HasOne(d => d.BackboneSegmentType)
                    .WithMany(p => p.BackboneSegment)
                    .HasForeignKey(d => d.BackboneSegmentTypeID)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DownstreamBackboneSegment)
                    .WithMany(p => p.InverseDownstreamBackboneSegment)
                    .HasForeignKey(d => d.DownstreamBackboneSegmentID)
                    .HasConstraintName("FK_BackboneSegment_BackboneSegment_DownstreamBackboneSegmentID_BackboneSegmentID");
            });

            modelBuilder.Entity<BackboneSegmentType>(entity =>
            {
                entity.HasIndex(e => e.BackboneSegmentTypeDisplayName)
                    .HasName("AK_BackboneSegmentType_BackboneSegmentTypeDisplayName")
                    .IsUnique();

                entity.HasIndex(e => e.BackboneSegmentTypeName)
                    .HasName("AK_BackboneSegmentType_BackboneSegmentTypeName")
                    .IsUnique();

                entity.Property(e => e.BackboneSegmentTypeID).ValueGeneratedNever();

                entity.Property(e => e.BackboneSegmentTypeDisplayName).IsUnicode(false);

                entity.Property(e => e.BackboneSegmentTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<CustomRichText>(entity =>
            {
                entity.Property(e => e.CustomRichTextContent).IsUnicode(false);

                entity.HasOne(d => d.CustomRichTextType)
                    .WithMany(p => p.CustomRichText)
                    .HasForeignKey(d => d.CustomRichTextTypeID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<CustomRichTextType>(entity =>
            {
                entity.HasIndex(e => e.CustomRichTextTypeDisplayName)
                    .HasName("AK_CustomRichTextType_CustomRichTextTypeDisplayName")
                    .IsUnique();

                entity.HasIndex(e => e.CustomRichTextTypeName)
                    .HasName("AK_CustomRichTextType_CustomRichTextTypeName")
                    .IsUnique();

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

            modelBuilder.Entity<FileResource>(entity =>
            {
                entity.HasIndex(e => e.FileResourceGUID)
                    .HasName("AK_FileResource_FileResourceGUID")
                    .IsUnique();

                entity.Property(e => e.OriginalBaseFilename).IsUnicode(false);

                entity.Property(e => e.OriginalFileExtension).IsUnicode(false);

                entity.HasOne(d => d.CreateUser)
                    .WithMany(p => p.FileResource)
                    .HasForeignKey(d => d.CreateUserID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FileResource_User_CreateUserID_UserID");

                entity.HasOne(d => d.FileResourceMimeType)
                    .WithMany(p => p.FileResource)
                    .HasForeignKey(d => d.FileResourceMimeTypeID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<FileResourceMimeType>(entity =>
            {
                entity.HasIndex(e => e.FileResourceMimeTypeDisplayName)
                    .HasName("AK_FileResourceMimeType_FileResourceMimeTypeDisplayName")
                    .IsUnique();

                entity.HasIndex(e => e.FileResourceMimeTypeName)
                    .HasName("AK_FileResourceMimeType_FileResourceMimeTypeName")
                    .IsUnique();

                entity.Property(e => e.FileResourceMimeTypeID).ValueGeneratedNever();

                entity.Property(e => e.FileResourceMimeTypeContentTypeName).IsUnicode(false);

                entity.Property(e => e.FileResourceMimeTypeDisplayName).IsUnicode(false);

                entity.Property(e => e.FileResourceMimeTypeIconNormalFilename).IsUnicode(false);

                entity.Property(e => e.FileResourceMimeTypeIconSmallFilename).IsUnicode(false);

                entity.Property(e => e.FileResourceMimeTypeName).IsUnicode(false);
            });

            modelBuilder.Entity<Neighborhood>(entity =>
            {
                entity.HasIndex(e => e.OCSurveyNeighborhoodID)
                    .HasName("AK_Neighborhood_OCSurveyNeighborhoodID")
                    .IsUnique();

                entity.Property(e => e.DrainID).IsUnicode(false);

                entity.Property(e => e.Watershed).IsUnicode(false);

                entity.HasOne(d => d.OCSurveyDownstreamNeighborhood)
                    .WithMany(p => p.InverseOCSurveyDownstreamNeighborhood)
                    .HasPrincipalKey(p => p.OCSurveyNeighborhoodID)
                    .HasForeignKey(d => d.OCSurveyDownstreamNeighborhoodID)
                    .HasConstraintName("FK_Neighborhood_Neighborhood_OCSurveyDownstreamNeighborhoodID_OCSurveyNeighborhoodID");
            });

            modelBuilder.Entity<RawDroolMetric>(entity =>
            {
                entity.Property(e => e.RawDroolMetricID).ValueGeneratedNever();

                entity.HasOne(d => d.MetricCatchIDNNavigation)
                    .WithMany(p => p.RawDroolMetric)
                    .HasPrincipalKey(p => p.OCSurveyNeighborhoodID)
                    .HasForeignKey(d => d.MetricCatchIDN)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RawDroolMetric_Neighborhood_CatchIDN_OCSurveyNeighborhoodID");
            });

            modelBuilder.Entity<RegionalSubbasin>(entity =>
            {
                entity.HasIndex(e => e.OCSurveyCatchmentID)
                    .HasName("AK_RegionalSubbasin_OCSurveyCatchmentID")
                    .IsUnique();

                entity.Property(e => e.DrainID).IsUnicode(false);

                entity.Property(e => e.Watershed).IsUnicode(false);

                entity.HasOne(d => d.OCSurveyDownstreamCatchment)
                    .WithMany(p => p.InverseOCSurveyDownstreamCatchment)
                    .HasPrincipalKey(p => p.OCSurveyCatchmentID)
                    .HasForeignKey(d => d.OCSurveyDownstreamCatchmentID)
                    .HasConstraintName("FK_RegionalSubbasin_RegionalSubbasin_OCSurveyDownstreamCatchmentID_OCSurveyCatchmentID");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.RoleDisplayName)
                    .HasName("AK_Role_RoleDisplayName")
                    .IsUnique();

                entity.HasIndex(e => e.RoleName)
                    .HasName("AK_Role_RoleName")
                    .IsUnique();

                entity.Property(e => e.RoleID).ValueGeneratedNever();

                entity.Property(e => e.RoleDescription).IsUnicode(false);

                entity.Property(e => e.RoleDisplayName).IsUnicode(false);

                entity.Property(e => e.RoleName).IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .HasName("AK_User_Email")
                    .IsUnique();

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.LoginName).IsUnicode(false);

                entity.Property(e => e.Phone).IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RoleID)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Watershed>(entity =>
            {
                entity.Property(e => e.WatershedName).IsUnicode(false);
            });

            modelBuilder.Entity<vGeoServerBackbone>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vGeoServerBackbone");

                entity.Property(e => e.BackboneSegmentType).IsUnicode(false);

                entity.Property(e => e.StreamName).IsUnicode(false);
            });

            modelBuilder.Entity<vGeoServerNeighborhood>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vGeoServerNeighborhood");

                entity.Property(e => e.DrainID).IsUnicode(false);

                entity.Property(e => e.NeighborhoodID).ValueGeneratedOnAdd();

                entity.Property(e => e.Watershed).IsUnicode(false);
            });

            modelBuilder.Entity<vGeoServerWatershed>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vGeoServerWatershed");

                entity.Property(e => e.WatershedID).ValueGeneratedOnAdd();

                entity.Property(e => e.WatershedName).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
