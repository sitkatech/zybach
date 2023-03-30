//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ReportTemplateModel]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class ReportTemplateModel
    {
        public static readonly ReportTemplateModelChemigationPermit ChemigationPermit = Zybach.EFModels.Entities.ReportTemplateModelChemigationPermit.Instance;
        public static readonly ReportTemplateModelWellWaterQualityInspection WellWaterQualityInspection = Zybach.EFModels.Entities.ReportTemplateModelWellWaterQualityInspection.Instance;
        public static readonly ReportTemplateModelWellGroupWaterLevelInspection WellGroupWaterLevelInspection = Zybach.EFModels.Entities.ReportTemplateModelWellGroupWaterLevelInspection.Instance;

        public static readonly List<ReportTemplateModel> All;
        public static readonly List<ReportTemplateModelDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, ReportTemplateModel> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, ReportTemplateModelDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static ReportTemplateModel()
        {
            All = new List<ReportTemplateModel> { ChemigationPermit, WellWaterQualityInspection, WellGroupWaterLevelInspection };
            AllAsDto = new List<ReportTemplateModelDto> { ChemigationPermit.AsDto(), WellWaterQualityInspection.AsDto(), WellGroupWaterLevelInspection.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, ReportTemplateModel>(All.ToDictionary(x => x.ReportTemplateModelID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, ReportTemplateModelDto>(AllAsDto.ToDictionary(x => x.ReportTemplateModelID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected ReportTemplateModel(int reportTemplateModelID, string reportTemplateModelName, string reportTemplateModelDisplayName, string reportTemplateModelDescription)
        {
            ReportTemplateModelID = reportTemplateModelID;
            ReportTemplateModelName = reportTemplateModelName;
            ReportTemplateModelDisplayName = reportTemplateModelDisplayName;
            ReportTemplateModelDescription = reportTemplateModelDescription;
        }

        [Key]
        public int ReportTemplateModelID { get; private set; }
        public string ReportTemplateModelName { get; private set; }
        public string ReportTemplateModelDisplayName { get; private set; }
        public string ReportTemplateModelDescription { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return ReportTemplateModelID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(ReportTemplateModel other)
        {
            if (other == null)
            {
                return false;
            }
            return other.ReportTemplateModelID == ReportTemplateModelID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as ReportTemplateModel);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return ReportTemplateModelID;
        }

        public static bool operator ==(ReportTemplateModel left, ReportTemplateModel right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ReportTemplateModel left, ReportTemplateModel right)
        {
            return !Equals(left, right);
        }

        public ReportTemplateModelEnum ToEnum => (ReportTemplateModelEnum)GetHashCode();

        public static ReportTemplateModel ToType(int enumValue)
        {
            return ToType((ReportTemplateModelEnum)enumValue);
        }

        public static ReportTemplateModel ToType(ReportTemplateModelEnum enumValue)
        {
            switch (enumValue)
            {
                case ReportTemplateModelEnum.ChemigationPermit:
                    return ChemigationPermit;
                case ReportTemplateModelEnum.WellGroupWaterLevelInspection:
                    return WellGroupWaterLevelInspection;
                case ReportTemplateModelEnum.WellWaterQualityInspection:
                    return WellWaterQualityInspection;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum ReportTemplateModelEnum
    {
        ChemigationPermit = 1,
        WellWaterQualityInspection = 2,
        WellGroupWaterLevelInspection = 3
    }

    public partial class ReportTemplateModelChemigationPermit : ReportTemplateModel
    {
        private ReportTemplateModelChemigationPermit(int reportTemplateModelID, string reportTemplateModelName, string reportTemplateModelDisplayName, string reportTemplateModelDescription) : base(reportTemplateModelID, reportTemplateModelName, reportTemplateModelDisplayName, reportTemplateModelDescription) {}
        public static readonly ReportTemplateModelChemigationPermit Instance = new ReportTemplateModelChemigationPermit(1, @"ChemigationPermit", @"Chemigation Permit", @"Templates will be with the ""ChemigationPermit"" model.");
    }

    public partial class ReportTemplateModelWellWaterQualityInspection : ReportTemplateModel
    {
        private ReportTemplateModelWellWaterQualityInspection(int reportTemplateModelID, string reportTemplateModelName, string reportTemplateModelDisplayName, string reportTemplateModelDescription) : base(reportTemplateModelID, reportTemplateModelName, reportTemplateModelDisplayName, reportTemplateModelDescription) {}
        public static readonly ReportTemplateModelWellWaterQualityInspection Instance = new ReportTemplateModelWellWaterQualityInspection(2, @"WellWaterQualityInspection", @"Well Water Quality Inspection", @"Templates will be with the ""Well"" model.");
    }

    public partial class ReportTemplateModelWellGroupWaterLevelInspection : ReportTemplateModel
    {
        private ReportTemplateModelWellGroupWaterLevelInspection(int reportTemplateModelID, string reportTemplateModelName, string reportTemplateModelDisplayName, string reportTemplateModelDescription) : base(reportTemplateModelID, reportTemplateModelName, reportTemplateModelDisplayName, reportTemplateModelDescription) {}
        public static readonly ReportTemplateModelWellGroupWaterLevelInspection Instance = new ReportTemplateModelWellGroupWaterLevelInspection(3, @"WellGroupWaterLevelInspection", @"Well Water Level Inspection", @"Templates will be with the ""WellGroup"" model.");
    }
}