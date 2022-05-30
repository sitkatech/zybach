//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[FieldDefinitionType]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class FieldDefinitionType
    {
        public static readonly FieldDefinitionTypeName Name = Zybach.EFModels.Entities.FieldDefinitionTypeName.Instance;
        public static readonly FieldDefinitionTypeHasWaterLevelInspections HasWaterLevelInspections = Zybach.EFModels.Entities.FieldDefinitionTypeHasWaterLevelInspections.Instance;
        public static readonly FieldDefinitionTypeHasWaterQualityInspections HasWaterQualityInspections = Zybach.EFModels.Entities.FieldDefinitionTypeHasWaterQualityInspections.Instance;
        public static readonly FieldDefinitionTypeLatestWaterLevelInspectionDate LatestWaterLevelInspectionDate = Zybach.EFModels.Entities.FieldDefinitionTypeLatestWaterLevelInspectionDate.Instance;
        public static readonly FieldDefinitionTypeLatestWaterQualityInspectionDate LatestWaterQualityInspectionDate = Zybach.EFModels.Entities.FieldDefinitionTypeLatestWaterQualityInspectionDate.Instance;

        public static readonly List<FieldDefinitionType> All;
        public static readonly List<FieldDefinitionTypeDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, FieldDefinitionType> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, FieldDefinitionTypeDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static FieldDefinitionType()
        {
            All = new List<FieldDefinitionType> { Name, HasWaterLevelInspections, HasWaterQualityInspections, LatestWaterLevelInspectionDate, LatestWaterQualityInspectionDate };
            AllAsDto = new List<FieldDefinitionTypeDto> { Name.AsDto(), HasWaterLevelInspections.AsDto(), HasWaterQualityInspections.AsDto(), LatestWaterLevelInspectionDate.AsDto(), LatestWaterQualityInspectionDate.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, FieldDefinitionType>(All.ToDictionary(x => x.FieldDefinitionTypeID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, FieldDefinitionTypeDto>(AllAsDto.ToDictionary(x => x.FieldDefinitionTypeID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected FieldDefinitionType(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName)
        {
            FieldDefinitionTypeID = fieldDefinitionTypeID;
            FieldDefinitionTypeName = fieldDefinitionTypeName;
            FieldDefinitionTypeDisplayName = fieldDefinitionTypeDisplayName;
        }

        [Key]
        public int FieldDefinitionTypeID { get; private set; }
        public string FieldDefinitionTypeName { get; private set; }
        public string FieldDefinitionTypeDisplayName { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return FieldDefinitionTypeID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(FieldDefinitionType other)
        {
            if (other == null)
            {
                return false;
            }
            return other.FieldDefinitionTypeID == FieldDefinitionTypeID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as FieldDefinitionType);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return FieldDefinitionTypeID;
        }

        public static bool operator ==(FieldDefinitionType left, FieldDefinitionType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FieldDefinitionType left, FieldDefinitionType right)
        {
            return !Equals(left, right);
        }

        public FieldDefinitionTypeEnum ToEnum => (FieldDefinitionTypeEnum)GetHashCode();

        public static FieldDefinitionType ToType(int enumValue)
        {
            return ToType((FieldDefinitionTypeEnum)enumValue);
        }

        public static FieldDefinitionType ToType(FieldDefinitionTypeEnum enumValue)
        {
            switch (enumValue)
            {
                case FieldDefinitionTypeEnum.HasWaterLevelInspections:
                    return HasWaterLevelInspections;
                case FieldDefinitionTypeEnum.HasWaterQualityInspections:
                    return HasWaterQualityInspections;
                case FieldDefinitionTypeEnum.LatestWaterLevelInspectionDate:
                    return LatestWaterLevelInspectionDate;
                case FieldDefinitionTypeEnum.LatestWaterQualityInspectionDate:
                    return LatestWaterQualityInspectionDate;
                case FieldDefinitionTypeEnum.Name:
                    return Name;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum FieldDefinitionTypeEnum
    {
        Name = 1,
        HasWaterLevelInspections = 2,
        HasWaterQualityInspections = 3,
        LatestWaterLevelInspectionDate = 4,
        LatestWaterQualityInspectionDate = 5
    }

    public partial class FieldDefinitionTypeName : FieldDefinitionType
    {
        private FieldDefinitionTypeName(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeName Instance = new FieldDefinitionTypeName(1, @"Name", @"Name");
    }

    public partial class FieldDefinitionTypeHasWaterLevelInspections : FieldDefinitionType
    {
        private FieldDefinitionTypeHasWaterLevelInspections(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeHasWaterLevelInspections Instance = new FieldDefinitionTypeHasWaterLevelInspections(2, @"HasWaterLevelInspections", @"Has Water Level Inspections?");
    }

    public partial class FieldDefinitionTypeHasWaterQualityInspections : FieldDefinitionType
    {
        private FieldDefinitionTypeHasWaterQualityInspections(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeHasWaterQualityInspections Instance = new FieldDefinitionTypeHasWaterQualityInspections(3, @"HasWaterQualityInspections", @"Has Water Quality Inspections?");
    }

    public partial class FieldDefinitionTypeLatestWaterLevelInspectionDate : FieldDefinitionType
    {
        private FieldDefinitionTypeLatestWaterLevelInspectionDate(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeLatestWaterLevelInspectionDate Instance = new FieldDefinitionTypeLatestWaterLevelInspectionDate(4, @"LatestWaterLevelInspectionDate", @"Latest Water Level Inspection Date");
    }

    public partial class FieldDefinitionTypeLatestWaterQualityInspectionDate : FieldDefinitionType
    {
        private FieldDefinitionTypeLatestWaterQualityInspectionDate(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeLatestWaterQualityInspectionDate Instance = new FieldDefinitionTypeLatestWaterQualityInspectionDate(5, @"LatestWaterQualityInspectionDate", @"Latest Water Quality Inspection Date");
    }
}