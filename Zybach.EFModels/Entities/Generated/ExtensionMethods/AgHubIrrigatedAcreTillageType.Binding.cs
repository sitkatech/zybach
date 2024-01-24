//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubIrrigatedAcreTillageType]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class AgHubIrrigatedAcreTillageType : IHavePrimaryKey
    {
        public static readonly AgHubIrrigatedAcreTillageTypeNTill NTill = Zybach.EFModels.Entities.AgHubIrrigatedAcreTillageTypeNTill.Instance;
        public static readonly AgHubIrrigatedAcreTillageTypeMTill MTill = Zybach.EFModels.Entities.AgHubIrrigatedAcreTillageTypeMTill.Instance;
        public static readonly AgHubIrrigatedAcreTillageTypeCTill CTill = Zybach.EFModels.Entities.AgHubIrrigatedAcreTillageTypeCTill.Instance;
        public static readonly AgHubIrrigatedAcreTillageTypeSTill STill = Zybach.EFModels.Entities.AgHubIrrigatedAcreTillageTypeSTill.Instance;
        public static readonly AgHubIrrigatedAcreTillageTypeOther Other = Zybach.EFModels.Entities.AgHubIrrigatedAcreTillageTypeOther.Instance;
        public static readonly AgHubIrrigatedAcreTillageTypeNotReported NotReported = Zybach.EFModels.Entities.AgHubIrrigatedAcreTillageTypeNotReported.Instance;

        public static readonly List<AgHubIrrigatedAcreTillageType> All;
        public static readonly List<AgHubIrrigatedAcreTillageTypeDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, AgHubIrrigatedAcreTillageType> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, AgHubIrrigatedAcreTillageTypeDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static AgHubIrrigatedAcreTillageType()
        {
            All = new List<AgHubIrrigatedAcreTillageType> { NTill, MTill, CTill, STill, Other, NotReported };
            AllAsDto = new List<AgHubIrrigatedAcreTillageTypeDto> { NTill.AsDto(), MTill.AsDto(), CTill.AsDto(), STill.AsDto(), Other.AsDto(), NotReported.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, AgHubIrrigatedAcreTillageType>(All.ToDictionary(x => x.AgHubIrrigatedAcreTillageTypeID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, AgHubIrrigatedAcreTillageTypeDto>(AllAsDto.ToDictionary(x => x.AgHubIrrigatedAcreTillageTypeID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected AgHubIrrigatedAcreTillageType(int agHubIrrigatedAcreTillageTypeID, string agHubIrrigatedAcreTillageTypeName, string agHubIrrigatedAcreTillageTypeDisplayName, string mapColor, int sortOrder)
        {
            AgHubIrrigatedAcreTillageTypeID = agHubIrrigatedAcreTillageTypeID;
            AgHubIrrigatedAcreTillageTypeName = agHubIrrigatedAcreTillageTypeName;
            AgHubIrrigatedAcreTillageTypeDisplayName = agHubIrrigatedAcreTillageTypeDisplayName;
            MapColor = mapColor;
            SortOrder = sortOrder;
        }

        [Key]
        public int AgHubIrrigatedAcreTillageTypeID { get; private set; }
        public string AgHubIrrigatedAcreTillageTypeName { get; private set; }
        public string AgHubIrrigatedAcreTillageTypeDisplayName { get; private set; }
        public string MapColor { get; private set; }
        public int SortOrder { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return AgHubIrrigatedAcreTillageTypeID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(AgHubIrrigatedAcreTillageType other)
        {
            if (other == null)
            {
                return false;
            }
            return other.AgHubIrrigatedAcreTillageTypeID == AgHubIrrigatedAcreTillageTypeID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as AgHubIrrigatedAcreTillageType);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return AgHubIrrigatedAcreTillageTypeID;
        }

        public static bool operator ==(AgHubIrrigatedAcreTillageType left, AgHubIrrigatedAcreTillageType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AgHubIrrigatedAcreTillageType left, AgHubIrrigatedAcreTillageType right)
        {
            return !Equals(left, right);
        }

        public AgHubIrrigatedAcreTillageTypeEnum ToEnum => (AgHubIrrigatedAcreTillageTypeEnum)GetHashCode();

        public static AgHubIrrigatedAcreTillageType ToType(int enumValue)
        {
            return ToType((AgHubIrrigatedAcreTillageTypeEnum)enumValue);
        }

        public static AgHubIrrigatedAcreTillageType ToType(AgHubIrrigatedAcreTillageTypeEnum enumValue)
        {
            switch (enumValue)
            {
                case AgHubIrrigatedAcreTillageTypeEnum.CTill:
                    return CTill;
                case AgHubIrrigatedAcreTillageTypeEnum.MTill:
                    return MTill;
                case AgHubIrrigatedAcreTillageTypeEnum.NotReported:
                    return NotReported;
                case AgHubIrrigatedAcreTillageTypeEnum.NTill:
                    return NTill;
                case AgHubIrrigatedAcreTillageTypeEnum.Other:
                    return Other;
                case AgHubIrrigatedAcreTillageTypeEnum.STill:
                    return STill;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum AgHubIrrigatedAcreTillageTypeEnum
    {
        NTill = 1,
        MTill = 2,
        CTill = 3,
        STill = 4,
        Other = 99,
        NotReported = 100
    }

    public partial class AgHubIrrigatedAcreTillageTypeNTill : AgHubIrrigatedAcreTillageType
    {
        private AgHubIrrigatedAcreTillageTypeNTill(int agHubIrrigatedAcreTillageTypeID, string agHubIrrigatedAcreTillageTypeName, string agHubIrrigatedAcreTillageTypeDisplayName, string mapColor, int sortOrder) : base(agHubIrrigatedAcreTillageTypeID, agHubIrrigatedAcreTillageTypeName, agHubIrrigatedAcreTillageTypeDisplayName, mapColor, sortOrder) {}
        public static readonly AgHubIrrigatedAcreTillageTypeNTill Instance = new AgHubIrrigatedAcreTillageTypeNTill(1, @"NTill", @"N Till (No Till)", @"#430154", 10);
    }

    public partial class AgHubIrrigatedAcreTillageTypeMTill : AgHubIrrigatedAcreTillageType
    {
        private AgHubIrrigatedAcreTillageTypeMTill(int agHubIrrigatedAcreTillageTypeID, string agHubIrrigatedAcreTillageTypeName, string agHubIrrigatedAcreTillageTypeDisplayName, string mapColor, int sortOrder) : base(agHubIrrigatedAcreTillageTypeID, agHubIrrigatedAcreTillageTypeName, agHubIrrigatedAcreTillageTypeDisplayName, mapColor, sortOrder) {}
        public static readonly AgHubIrrigatedAcreTillageTypeMTill Instance = new AgHubIrrigatedAcreTillageTypeMTill(2, @"MTill", @"M Till (Minimum Till)", @"#453781", 20);
    }

    public partial class AgHubIrrigatedAcreTillageTypeCTill : AgHubIrrigatedAcreTillageType
    {
        private AgHubIrrigatedAcreTillageTypeCTill(int agHubIrrigatedAcreTillageTypeID, string agHubIrrigatedAcreTillageTypeName, string agHubIrrigatedAcreTillageTypeDisplayName, string mapColor, int sortOrder) : base(agHubIrrigatedAcreTillageTypeID, agHubIrrigatedAcreTillageTypeName, agHubIrrigatedAcreTillageTypeDisplayName, mapColor, sortOrder) {}
        public static readonly AgHubIrrigatedAcreTillageTypeCTill Instance = new AgHubIrrigatedAcreTillageTypeCTill(3, @"CTill", @"C Till (Conventional Till)", @"#33638d", 30);
    }

    public partial class AgHubIrrigatedAcreTillageTypeSTill : AgHubIrrigatedAcreTillageType
    {
        private AgHubIrrigatedAcreTillageTypeSTill(int agHubIrrigatedAcreTillageTypeID, string agHubIrrigatedAcreTillageTypeName, string agHubIrrigatedAcreTillageTypeDisplayName, string mapColor, int sortOrder) : base(agHubIrrigatedAcreTillageTypeID, agHubIrrigatedAcreTillageTypeName, agHubIrrigatedAcreTillageTypeDisplayName, mapColor, sortOrder) {}
        public static readonly AgHubIrrigatedAcreTillageTypeSTill Instance = new AgHubIrrigatedAcreTillageTypeSTill(4, @"STill", @"S Till (Strip Till)", @"#238a8d", 40);
    }

    public partial class AgHubIrrigatedAcreTillageTypeOther : AgHubIrrigatedAcreTillageType
    {
        private AgHubIrrigatedAcreTillageTypeOther(int agHubIrrigatedAcreTillageTypeID, string agHubIrrigatedAcreTillageTypeName, string agHubIrrigatedAcreTillageTypeDisplayName, string mapColor, int sortOrder) : base(agHubIrrigatedAcreTillageTypeID, agHubIrrigatedAcreTillageTypeName, agHubIrrigatedAcreTillageTypeDisplayName, mapColor, sortOrder) {}
        public static readonly AgHubIrrigatedAcreTillageTypeOther Instance = new AgHubIrrigatedAcreTillageTypeOther(99, @"Other", @"Other", @"#00b6b6", 999);
    }

    public partial class AgHubIrrigatedAcreTillageTypeNotReported : AgHubIrrigatedAcreTillageType
    {
        private AgHubIrrigatedAcreTillageTypeNotReported(int agHubIrrigatedAcreTillageTypeID, string agHubIrrigatedAcreTillageTypeName, string agHubIrrigatedAcreTillageTypeDisplayName, string mapColor, int sortOrder) : base(agHubIrrigatedAcreTillageTypeID, agHubIrrigatedAcreTillageTypeName, agHubIrrigatedAcreTillageTypeDisplayName, mapColor, sortOrder) {}
        public static readonly AgHubIrrigatedAcreTillageTypeNotReported Instance = new AgHubIrrigatedAcreTillageTypeNotReported(100, @"NotReported", @"Not Reported", @"#e22e1d", 1000);
    }
}