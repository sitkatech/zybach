//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubWellIrrigatedAcreCropType]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class AgHubWellIrrigatedAcreCropType : IHavePrimaryKey
    {
        public static readonly AgHubWellIrrigatedAcreCropTypeCorn Corn = Zybach.EFModels.Entities.AgHubWellIrrigatedAcreCropTypeCorn.Instance;
        public static readonly AgHubWellIrrigatedAcreCropTypePopcorn Popcorn = Zybach.EFModels.Entities.AgHubWellIrrigatedAcreCropTypePopcorn.Instance;
        public static readonly AgHubWellIrrigatedAcreCropTypeSoybeans Soybeans = Zybach.EFModels.Entities.AgHubWellIrrigatedAcreCropTypeSoybeans.Instance;
        public static readonly AgHubWellIrrigatedAcreCropTypeSorghum Sorghum = Zybach.EFModels.Entities.AgHubWellIrrigatedAcreCropTypeSorghum.Instance;
        public static readonly AgHubWellIrrigatedAcreCropTypeDryEdibleBeans DryEdibleBeans = Zybach.EFModels.Entities.AgHubWellIrrigatedAcreCropTypeDryEdibleBeans.Instance;
        public static readonly AgHubWellIrrigatedAcreCropTypeAlfalfa Alfalfa = Zybach.EFModels.Entities.AgHubWellIrrigatedAcreCropTypeAlfalfa.Instance;
        public static readonly AgHubWellIrrigatedAcreCropTypeSmallGrains SmallGrains = Zybach.EFModels.Entities.AgHubWellIrrigatedAcreCropTypeSmallGrains.Instance;
        public static readonly AgHubWellIrrigatedAcreCropTypeWinterWheat WinterWheat = Zybach.EFModels.Entities.AgHubWellIrrigatedAcreCropTypeWinterWheat.Instance;
        public static readonly AgHubWellIrrigatedAcreCropTypeFallowFields FallowFields = Zybach.EFModels.Entities.AgHubWellIrrigatedAcreCropTypeFallowFields.Instance;
        public static readonly AgHubWellIrrigatedAcreCropTypeSunflower Sunflower = Zybach.EFModels.Entities.AgHubWellIrrigatedAcreCropTypeSunflower.Instance;
        public static readonly AgHubWellIrrigatedAcreCropTypeSugarBeets SugarBeets = Zybach.EFModels.Entities.AgHubWellIrrigatedAcreCropTypeSugarBeets.Instance;
        public static readonly AgHubWellIrrigatedAcreCropTypePotatoes Potatoes = Zybach.EFModels.Entities.AgHubWellIrrigatedAcreCropTypePotatoes.Instance;
        public static readonly AgHubWellIrrigatedAcreCropTypeRangePastureGrassland RangePastureGrassland = Zybach.EFModels.Entities.AgHubWellIrrigatedAcreCropTypeRangePastureGrassland.Instance;
        public static readonly AgHubWellIrrigatedAcreCropTypeForage Forage = Zybach.EFModels.Entities.AgHubWellIrrigatedAcreCropTypeForage.Instance;
        public static readonly AgHubWellIrrigatedAcreCropTypeTurfGrass TurfGrass = Zybach.EFModels.Entities.AgHubWellIrrigatedAcreCropTypeTurfGrass.Instance;
        public static readonly AgHubWellIrrigatedAcreCropTypePasture Pasture = Zybach.EFModels.Entities.AgHubWellIrrigatedAcreCropTypePasture.Instance;
        public static readonly AgHubWellIrrigatedAcreCropTypeNotReported NotReported = Zybach.EFModels.Entities.AgHubWellIrrigatedAcreCropTypeNotReported.Instance;
        public static readonly AgHubWellIrrigatedAcreCropTypeOther Other = Zybach.EFModels.Entities.AgHubWellIrrigatedAcreCropTypeOther.Instance;

        public static readonly List<AgHubWellIrrigatedAcreCropType> All;
        public static readonly List<AgHubWellIrrigatedAcreCropTypeDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, AgHubWellIrrigatedAcreCropType> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, AgHubWellIrrigatedAcreCropTypeDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static AgHubWellIrrigatedAcreCropType()
        {
            All = new List<AgHubWellIrrigatedAcreCropType> { Corn, Popcorn, Soybeans, Sorghum, DryEdibleBeans, Alfalfa, SmallGrains, WinterWheat, FallowFields, Sunflower, SugarBeets, Potatoes, RangePastureGrassland, Forage, TurfGrass, Pasture, NotReported, Other };
            AllAsDto = new List<AgHubWellIrrigatedAcreCropTypeDto> { Corn.AsDto(), Popcorn.AsDto(), Soybeans.AsDto(), Sorghum.AsDto(), DryEdibleBeans.AsDto(), Alfalfa.AsDto(), SmallGrains.AsDto(), WinterWheat.AsDto(), FallowFields.AsDto(), Sunflower.AsDto(), SugarBeets.AsDto(), Potatoes.AsDto(), RangePastureGrassland.AsDto(), Forage.AsDto(), TurfGrass.AsDto(), Pasture.AsDto(), NotReported.AsDto(), Other.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, AgHubWellIrrigatedAcreCropType>(All.ToDictionary(x => x.AgHubWellIrrigatedAcreCropTypeID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, AgHubWellIrrigatedAcreCropTypeDto>(AllAsDto.ToDictionary(x => x.AgHubWellIrrigatedAcreCropTypeID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected AgHubWellIrrigatedAcreCropType(int agHubWellIrrigatedAcreCropTypeID, string agHubWellIrrigatedAcreCropTypeName, string mapColor)
        {
            AgHubWellIrrigatedAcreCropTypeID = agHubWellIrrigatedAcreCropTypeID;
            AgHubWellIrrigatedAcreCropTypeName = agHubWellIrrigatedAcreCropTypeName;
            MapColor = mapColor;
        }

        [Key]
        public int AgHubWellIrrigatedAcreCropTypeID { get; private set; }
        public string AgHubWellIrrigatedAcreCropTypeName { get; private set; }
        public string MapColor { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return AgHubWellIrrigatedAcreCropTypeID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(AgHubWellIrrigatedAcreCropType other)
        {
            if (other == null)
            {
                return false;
            }
            return other.AgHubWellIrrigatedAcreCropTypeID == AgHubWellIrrigatedAcreCropTypeID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as AgHubWellIrrigatedAcreCropType);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return AgHubWellIrrigatedAcreCropTypeID;
        }

        public static bool operator ==(AgHubWellIrrigatedAcreCropType left, AgHubWellIrrigatedAcreCropType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AgHubWellIrrigatedAcreCropType left, AgHubWellIrrigatedAcreCropType right)
        {
            return !Equals(left, right);
        }

        public AgHubWellIrrigatedAcreCropTypeEnum ToEnum => (AgHubWellIrrigatedAcreCropTypeEnum)GetHashCode();

        public static AgHubWellIrrigatedAcreCropType ToType(int enumValue)
        {
            return ToType((AgHubWellIrrigatedAcreCropTypeEnum)enumValue);
        }

        public static AgHubWellIrrigatedAcreCropType ToType(AgHubWellIrrigatedAcreCropTypeEnum enumValue)
        {
            switch (enumValue)
            {
                case AgHubWellIrrigatedAcreCropTypeEnum.Alfalfa:
                    return Alfalfa;
                case AgHubWellIrrigatedAcreCropTypeEnum.Corn:
                    return Corn;
                case AgHubWellIrrigatedAcreCropTypeEnum.DryEdibleBeans:
                    return DryEdibleBeans;
                case AgHubWellIrrigatedAcreCropTypeEnum.FallowFields:
                    return FallowFields;
                case AgHubWellIrrigatedAcreCropTypeEnum.Forage:
                    return Forage;
                case AgHubWellIrrigatedAcreCropTypeEnum.NotReported:
                    return NotReported;
                case AgHubWellIrrigatedAcreCropTypeEnum.Other:
                    return Other;
                case AgHubWellIrrigatedAcreCropTypeEnum.Pasture:
                    return Pasture;
                case AgHubWellIrrigatedAcreCropTypeEnum.Popcorn:
                    return Popcorn;
                case AgHubWellIrrigatedAcreCropTypeEnum.Potatoes:
                    return Potatoes;
                case AgHubWellIrrigatedAcreCropTypeEnum.RangePastureGrassland:
                    return RangePastureGrassland;
                case AgHubWellIrrigatedAcreCropTypeEnum.SmallGrains:
                    return SmallGrains;
                case AgHubWellIrrigatedAcreCropTypeEnum.Sorghum:
                    return Sorghum;
                case AgHubWellIrrigatedAcreCropTypeEnum.Soybeans:
                    return Soybeans;
                case AgHubWellIrrigatedAcreCropTypeEnum.SugarBeets:
                    return SugarBeets;
                case AgHubWellIrrigatedAcreCropTypeEnum.Sunflower:
                    return Sunflower;
                case AgHubWellIrrigatedAcreCropTypeEnum.TurfGrass:
                    return TurfGrass;
                case AgHubWellIrrigatedAcreCropTypeEnum.WinterWheat:
                    return WinterWheat;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum AgHubWellIrrigatedAcreCropTypeEnum
    {
        Corn = 1,
        Popcorn = 2,
        Soybeans = 3,
        Sorghum = 4,
        DryEdibleBeans = 5,
        Alfalfa = 6,
        SmallGrains = 7,
        WinterWheat = 8,
        FallowFields = 9,
        Sunflower = 10,
        SugarBeets = 11,
        Potatoes = 12,
        RangePastureGrassland = 13,
        Forage = 14,
        TurfGrass = 15,
        Pasture = 16,
        NotReported = 17,
        Other = 18
    }

    public partial class AgHubWellIrrigatedAcreCropTypeCorn : AgHubWellIrrigatedAcreCropType
    {
        private AgHubWellIrrigatedAcreCropTypeCorn(int agHubWellIrrigatedAcreCropTypeID, string agHubWellIrrigatedAcreCropTypeName, string mapColor) : base(agHubWellIrrigatedAcreCropTypeID, agHubWellIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubWellIrrigatedAcreCropTypeCorn Instance = new AgHubWellIrrigatedAcreCropTypeCorn(1, @"Corn", @"#00b600");
    }

    public partial class AgHubWellIrrigatedAcreCropTypePopcorn : AgHubWellIrrigatedAcreCropType
    {
        private AgHubWellIrrigatedAcreCropTypePopcorn(int agHubWellIrrigatedAcreCropTypeID, string agHubWellIrrigatedAcreCropTypeName, string mapColor) : base(agHubWellIrrigatedAcreCropTypeID, agHubWellIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubWellIrrigatedAcreCropTypePopcorn Instance = new AgHubWellIrrigatedAcreCropTypePopcorn(2, @"Popcorn", @"#007b00");
    }

    public partial class AgHubWellIrrigatedAcreCropTypeSoybeans : AgHubWellIrrigatedAcreCropType
    {
        private AgHubWellIrrigatedAcreCropTypeSoybeans(int agHubWellIrrigatedAcreCropTypeID, string agHubWellIrrigatedAcreCropTypeName, string mapColor) : base(agHubWellIrrigatedAcreCropTypeID, agHubWellIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubWellIrrigatedAcreCropTypeSoybeans Instance = new AgHubWellIrrigatedAcreCropTypeSoybeans(3, @"Soybeans", @"#003e00");
    }

    public partial class AgHubWellIrrigatedAcreCropTypeSorghum : AgHubWellIrrigatedAcreCropType
    {
        private AgHubWellIrrigatedAcreCropTypeSorghum(int agHubWellIrrigatedAcreCropTypeID, string agHubWellIrrigatedAcreCropTypeName, string mapColor) : base(agHubWellIrrigatedAcreCropTypeID, agHubWellIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubWellIrrigatedAcreCropTypeSorghum Instance = new AgHubWellIrrigatedAcreCropTypeSorghum(4, @"Sorghum", @"#d9ae00");
    }

    public partial class AgHubWellIrrigatedAcreCropTypeDryEdibleBeans : AgHubWellIrrigatedAcreCropType
    {
        private AgHubWellIrrigatedAcreCropTypeDryEdibleBeans(int agHubWellIrrigatedAcreCropTypeID, string agHubWellIrrigatedAcreCropTypeName, string mapColor) : base(agHubWellIrrigatedAcreCropTypeID, agHubWellIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubWellIrrigatedAcreCropTypeDryEdibleBeans Instance = new AgHubWellIrrigatedAcreCropTypeDryEdibleBeans(5, @"Dry Edible Beans", @"#d57c00");
    }

    public partial class AgHubWellIrrigatedAcreCropTypeAlfalfa : AgHubWellIrrigatedAcreCropType
    {
        private AgHubWellIrrigatedAcreCropTypeAlfalfa(int agHubWellIrrigatedAcreCropTypeID, string agHubWellIrrigatedAcreCropTypeName, string mapColor) : base(agHubWellIrrigatedAcreCropTypeID, agHubWellIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubWellIrrigatedAcreCropTypeAlfalfa Instance = new AgHubWellIrrigatedAcreCropTypeAlfalfa(6, @"Alfalfa", @"#dade00");
    }

    public partial class AgHubWellIrrigatedAcreCropTypeSmallGrains : AgHubWellIrrigatedAcreCropType
    {
        private AgHubWellIrrigatedAcreCropTypeSmallGrains(int agHubWellIrrigatedAcreCropTypeID, string agHubWellIrrigatedAcreCropTypeName, string mapColor) : base(agHubWellIrrigatedAcreCropTypeID, agHubWellIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubWellIrrigatedAcreCropTypeSmallGrains Instance = new AgHubWellIrrigatedAcreCropTypeSmallGrains(7, @"Small Grains", @"#d500d9");
    }

    public partial class AgHubWellIrrigatedAcreCropTypeWinterWheat : AgHubWellIrrigatedAcreCropType
    {
        private AgHubWellIrrigatedAcreCropTypeWinterWheat(int agHubWellIrrigatedAcreCropTypeID, string agHubWellIrrigatedAcreCropTypeName, string mapColor) : base(agHubWellIrrigatedAcreCropTypeID, agHubWellIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubWellIrrigatedAcreCropTypeWinterWheat Instance = new AgHubWellIrrigatedAcreCropTypeWinterWheat(8, @"Winter Wheat", @"#b521b8");
    }

    public partial class AgHubWellIrrigatedAcreCropTypeFallowFields : AgHubWellIrrigatedAcreCropType
    {
        private AgHubWellIrrigatedAcreCropTypeFallowFields(int agHubWellIrrigatedAcreCropTypeID, string agHubWellIrrigatedAcreCropTypeName, string mapColor) : base(agHubWellIrrigatedAcreCropTypeID, agHubWellIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubWellIrrigatedAcreCropTypeFallowFields Instance = new AgHubWellIrrigatedAcreCropTypeFallowFields(9, @"Fallow Fields", @"#d9d9d9");
    }

    public partial class AgHubWellIrrigatedAcreCropTypeSunflower : AgHubWellIrrigatedAcreCropType
    {
        private AgHubWellIrrigatedAcreCropTypeSunflower(int agHubWellIrrigatedAcreCropTypeID, string agHubWellIrrigatedAcreCropTypeName, string mapColor) : base(agHubWellIrrigatedAcreCropTypeID, agHubWellIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubWellIrrigatedAcreCropTypeSunflower Instance = new AgHubWellIrrigatedAcreCropTypeSunflower(10, @"Sunflower", @"#d890a2");
    }

    public partial class AgHubWellIrrigatedAcreCropTypeSugarBeets : AgHubWellIrrigatedAcreCropType
    {
        private AgHubWellIrrigatedAcreCropTypeSugarBeets(int agHubWellIrrigatedAcreCropTypeID, string agHubWellIrrigatedAcreCropTypeName, string mapColor) : base(agHubWellIrrigatedAcreCropTypeID, agHubWellIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubWellIrrigatedAcreCropTypeSugarBeets Instance = new AgHubWellIrrigatedAcreCropTypeSugarBeets(11, @"Sugar Beets", @"#7000cb");
    }

    public partial class AgHubWellIrrigatedAcreCropTypePotatoes : AgHubWellIrrigatedAcreCropType
    {
        private AgHubWellIrrigatedAcreCropTypePotatoes(int agHubWellIrrigatedAcreCropTypeID, string agHubWellIrrigatedAcreCropTypeName, string mapColor) : base(agHubWellIrrigatedAcreCropTypeID, agHubWellIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubWellIrrigatedAcreCropTypePotatoes Instance = new AgHubWellIrrigatedAcreCropTypePotatoes(12, @"Potatoes", @"#780012");
    }

    public partial class AgHubWellIrrigatedAcreCropTypeRangePastureGrassland : AgHubWellIrrigatedAcreCropType
    {
        private AgHubWellIrrigatedAcreCropTypeRangePastureGrassland(int agHubWellIrrigatedAcreCropTypeID, string agHubWellIrrigatedAcreCropTypeName, string mapColor) : base(agHubWellIrrigatedAcreCropTypeID, agHubWellIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubWellIrrigatedAcreCropTypeRangePastureGrassland Instance = new AgHubWellIrrigatedAcreCropTypeRangePastureGrassland(13, @"Range, Pasture, Grassland", @"#a08c62");
    }

    public partial class AgHubWellIrrigatedAcreCropTypeForage : AgHubWellIrrigatedAcreCropType
    {
        private AgHubWellIrrigatedAcreCropTypeForage(int agHubWellIrrigatedAcreCropTypeID, string agHubWellIrrigatedAcreCropTypeName, string mapColor) : base(agHubWellIrrigatedAcreCropTypeID, agHubWellIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubWellIrrigatedAcreCropTypeForage Instance = new AgHubWellIrrigatedAcreCropTypeForage(14, @"Forage", @"#7c6c4b");
    }

    public partial class AgHubWellIrrigatedAcreCropTypeTurfGrass : AgHubWellIrrigatedAcreCropType
    {
        private AgHubWellIrrigatedAcreCropTypeTurfGrass(int agHubWellIrrigatedAcreCropTypeID, string agHubWellIrrigatedAcreCropTypeName, string mapColor) : base(agHubWellIrrigatedAcreCropTypeID, agHubWellIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubWellIrrigatedAcreCropTypeTurfGrass Instance = new AgHubWellIrrigatedAcreCropTypeTurfGrass(15, @"Turf Grass", @"#574c35");
    }

    public partial class AgHubWellIrrigatedAcreCropTypePasture : AgHubWellIrrigatedAcreCropType
    {
        private AgHubWellIrrigatedAcreCropTypePasture(int agHubWellIrrigatedAcreCropTypeID, string agHubWellIrrigatedAcreCropTypeName, string mapColor) : base(agHubWellIrrigatedAcreCropTypeID, agHubWellIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubWellIrrigatedAcreCropTypePasture Instance = new AgHubWellIrrigatedAcreCropTypePasture(16, @"Pasture", @"#a08c62");
    }

    public partial class AgHubWellIrrigatedAcreCropTypeNotReported : AgHubWellIrrigatedAcreCropType
    {
        private AgHubWellIrrigatedAcreCropTypeNotReported(int agHubWellIrrigatedAcreCropTypeID, string agHubWellIrrigatedAcreCropTypeName, string mapColor) : base(agHubWellIrrigatedAcreCropTypeID, agHubWellIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubWellIrrigatedAcreCropTypeNotReported Instance = new AgHubWellIrrigatedAcreCropTypeNotReported(17, @"Not Reported", @"#e22e1d");
    }

    public partial class AgHubWellIrrigatedAcreCropTypeOther : AgHubWellIrrigatedAcreCropType
    {
        private AgHubWellIrrigatedAcreCropTypeOther(int agHubWellIrrigatedAcreCropTypeID, string agHubWellIrrigatedAcreCropTypeName, string mapColor) : base(agHubWellIrrigatedAcreCropTypeID, agHubWellIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubWellIrrigatedAcreCropTypeOther Instance = new AgHubWellIrrigatedAcreCropTypeOther(18, @"Other", @"#00b6b6");
    }
}