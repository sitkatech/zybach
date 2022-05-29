//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WaterQualityInspectionType]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class WaterQualityInspectionType
    {
        public static readonly WaterQualityInspectionTypeFullPanel FullPanel = Zybach.EFModels.Entities.WaterQualityInspectionTypeFullPanel.Instance;
        public static readonly WaterQualityInspectionTypeNitratesOnly NitratesOnly = Zybach.EFModels.Entities.WaterQualityInspectionTypeNitratesOnly.Instance;

        public static readonly List<WaterQualityInspectionType> All;
        public static readonly List<WaterQualityInspectionTypeDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, WaterQualityInspectionType> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, WaterQualityInspectionTypeDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static WaterQualityInspectionType()
        {
            All = new List<WaterQualityInspectionType> { FullPanel, NitratesOnly };
            AllAsDto = new List<WaterQualityInspectionTypeDto> { FullPanel.AsDto(), NitratesOnly.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, WaterQualityInspectionType>(All.ToDictionary(x => x.WaterQualityInspectionTypeID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, WaterQualityInspectionTypeDto>(AllAsDto.ToDictionary(x => x.WaterQualityInspectionTypeID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected WaterQualityInspectionType(int waterQualityInspectionTypeID, string waterQualityInspectionTypeName, string waterQualityInspectionTypeDisplayName)
        {
            WaterQualityInspectionTypeID = waterQualityInspectionTypeID;
            WaterQualityInspectionTypeName = waterQualityInspectionTypeName;
            WaterQualityInspectionTypeDisplayName = waterQualityInspectionTypeDisplayName;
        }

        [Key]
        public int WaterQualityInspectionTypeID { get; private set; }
        public string WaterQualityInspectionTypeName { get; private set; }
        public string WaterQualityInspectionTypeDisplayName { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return WaterQualityInspectionTypeID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(WaterQualityInspectionType other)
        {
            if (other == null)
            {
                return false;
            }
            return other.WaterQualityInspectionTypeID == WaterQualityInspectionTypeID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as WaterQualityInspectionType);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return WaterQualityInspectionTypeID;
        }

        public static bool operator ==(WaterQualityInspectionType left, WaterQualityInspectionType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(WaterQualityInspectionType left, WaterQualityInspectionType right)
        {
            return !Equals(left, right);
        }

        public WaterQualityInspectionTypeEnum ToEnum => (WaterQualityInspectionTypeEnum)GetHashCode();

        public static WaterQualityInspectionType ToType(int enumValue)
        {
            return ToType((WaterQualityInspectionTypeEnum)enumValue);
        }

        public static WaterQualityInspectionType ToType(WaterQualityInspectionTypeEnum enumValue)
        {
            switch (enumValue)
            {
                case WaterQualityInspectionTypeEnum.FullPanel:
                    return FullPanel;
                case WaterQualityInspectionTypeEnum.NitratesOnly:
                    return NitratesOnly;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum WaterQualityInspectionTypeEnum
    {
        FullPanel = 1,
        NitratesOnly = 2
    }

    public partial class WaterQualityInspectionTypeFullPanel : WaterQualityInspectionType
    {
        private WaterQualityInspectionTypeFullPanel(int waterQualityInspectionTypeID, string waterQualityInspectionTypeName, string waterQualityInspectionTypeDisplayName) : base(waterQualityInspectionTypeID, waterQualityInspectionTypeName, waterQualityInspectionTypeDisplayName) {}
        public static readonly WaterQualityInspectionTypeFullPanel Instance = new WaterQualityInspectionTypeFullPanel(1, @"FullPanel", @"Full Panel");
    }

    public partial class WaterQualityInspectionTypeNitratesOnly : WaterQualityInspectionType
    {
        private WaterQualityInspectionTypeNitratesOnly(int waterQualityInspectionTypeID, string waterQualityInspectionTypeName, string waterQualityInspectionTypeDisplayName) : base(waterQualityInspectionTypeID, waterQualityInspectionTypeName, waterQualityInspectionTypeDisplayName) {}
        public static readonly WaterQualityInspectionTypeNitratesOnly Instance = new WaterQualityInspectionTypeNitratesOnly(2, @"NitratesOnly", @"Nitrates Only");
    }
}