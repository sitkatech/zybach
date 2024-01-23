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

        public static readonly List<AgHubIrrigatedAcreTillageType> All;
        public static readonly List<AgHubIrrigatedAcreTillageTypeDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, AgHubIrrigatedAcreTillageType> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, AgHubIrrigatedAcreTillageTypeDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static AgHubIrrigatedAcreTillageType()
        {
            All = new List<AgHubIrrigatedAcreTillageType> { NTill, MTill, CTill, STill };
            AllAsDto = new List<AgHubIrrigatedAcreTillageTypeDto> { NTill.AsDto(), MTill.AsDto(), CTill.AsDto(), STill.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, AgHubIrrigatedAcreTillageType>(All.ToDictionary(x => x.AgHubIrrigatedAcreTillageTypeID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, AgHubIrrigatedAcreTillageTypeDto>(AllAsDto.ToDictionary(x => x.AgHubIrrigatedAcreTillageTypeID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected AgHubIrrigatedAcreTillageType(int agHubIrrigatedAcreTillageTypeID, string agHubIrrigatedAcreTillageTypeName, string agHubIrrigatedAcreTillageTypeDisplayName, string mapColor)
        {
            AgHubIrrigatedAcreTillageTypeID = agHubIrrigatedAcreTillageTypeID;
            AgHubIrrigatedAcreTillageTypeName = agHubIrrigatedAcreTillageTypeName;
            AgHubIrrigatedAcreTillageTypeDisplayName = agHubIrrigatedAcreTillageTypeDisplayName;
            MapColor = mapColor;
        }

        [Key]
        public int AgHubIrrigatedAcreTillageTypeID { get; private set; }
        public string AgHubIrrigatedAcreTillageTypeName { get; private set; }
        public string AgHubIrrigatedAcreTillageTypeDisplayName { get; private set; }
        public string MapColor { get; private set; }
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
                case AgHubIrrigatedAcreTillageTypeEnum.NTill:
                    return NTill;
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
        STill = 4
    }

    public partial class AgHubIrrigatedAcreTillageTypeNTill : AgHubIrrigatedAcreTillageType
    {
        private AgHubIrrigatedAcreTillageTypeNTill(int agHubIrrigatedAcreTillageTypeID, string agHubIrrigatedAcreTillageTypeName, string agHubIrrigatedAcreTillageTypeDisplayName, string mapColor) : base(agHubIrrigatedAcreTillageTypeID, agHubIrrigatedAcreTillageTypeName, agHubIrrigatedAcreTillageTypeDisplayName, mapColor) {}
        public static readonly AgHubIrrigatedAcreTillageTypeNTill Instance = new AgHubIrrigatedAcreTillageTypeNTill(1, @"N Till", @"No Till", @"#430154");
    }

    public partial class AgHubIrrigatedAcreTillageTypeMTill : AgHubIrrigatedAcreTillageType
    {
        private AgHubIrrigatedAcreTillageTypeMTill(int agHubIrrigatedAcreTillageTypeID, string agHubIrrigatedAcreTillageTypeName, string agHubIrrigatedAcreTillageTypeDisplayName, string mapColor) : base(agHubIrrigatedAcreTillageTypeID, agHubIrrigatedAcreTillageTypeName, agHubIrrigatedAcreTillageTypeDisplayName, mapColor) {}
        public static readonly AgHubIrrigatedAcreTillageTypeMTill Instance = new AgHubIrrigatedAcreTillageTypeMTill(2, @"M Till", @"Minimum Till", @"#453781");
    }

    public partial class AgHubIrrigatedAcreTillageTypeCTill : AgHubIrrigatedAcreTillageType
    {
        private AgHubIrrigatedAcreTillageTypeCTill(int agHubIrrigatedAcreTillageTypeID, string agHubIrrigatedAcreTillageTypeName, string agHubIrrigatedAcreTillageTypeDisplayName, string mapColor) : base(agHubIrrigatedAcreTillageTypeID, agHubIrrigatedAcreTillageTypeName, agHubIrrigatedAcreTillageTypeDisplayName, mapColor) {}
        public static readonly AgHubIrrigatedAcreTillageTypeCTill Instance = new AgHubIrrigatedAcreTillageTypeCTill(3, @"C Till", @"Conventional Till", @"#33638d");
    }

    public partial class AgHubIrrigatedAcreTillageTypeSTill : AgHubIrrigatedAcreTillageType
    {
        private AgHubIrrigatedAcreTillageTypeSTill(int agHubIrrigatedAcreTillageTypeID, string agHubIrrigatedAcreTillageTypeName, string agHubIrrigatedAcreTillageTypeDisplayName, string mapColor) : base(agHubIrrigatedAcreTillageTypeID, agHubIrrigatedAcreTillageTypeName, agHubIrrigatedAcreTillageTypeDisplayName, mapColor) {}
        public static readonly AgHubIrrigatedAcreTillageTypeSTill Instance = new AgHubIrrigatedAcreTillageTypeSTill(4, @"S Till", @"Strip Till", @"#238a8d");
    }
}