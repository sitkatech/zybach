//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubIrrigatedAcreCropType]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class AgHubIrrigatedAcreCropType : IHavePrimaryKey
    {
        public static readonly AgHubIrrigatedAcreCropTypeCorn Corn = Zybach.EFModels.Entities.AgHubIrrigatedAcreCropTypeCorn.Instance;
        public static readonly AgHubIrrigatedAcreCropTypePopcorn Popcorn = Zybach.EFModels.Entities.AgHubIrrigatedAcreCropTypePopcorn.Instance;
        public static readonly AgHubIrrigatedAcreCropTypeSoybeans Soybeans = Zybach.EFModels.Entities.AgHubIrrigatedAcreCropTypeSoybeans.Instance;
        public static readonly AgHubIrrigatedAcreCropTypeSorghum Sorghum = Zybach.EFModels.Entities.AgHubIrrigatedAcreCropTypeSorghum.Instance;
        public static readonly AgHubIrrigatedAcreCropTypeDryEdibleBeans DryEdibleBeans = Zybach.EFModels.Entities.AgHubIrrigatedAcreCropTypeDryEdibleBeans.Instance;
        public static readonly AgHubIrrigatedAcreCropTypeAlfalfa Alfalfa = Zybach.EFModels.Entities.AgHubIrrigatedAcreCropTypeAlfalfa.Instance;
        public static readonly AgHubIrrigatedAcreCropTypeSmallGrains SmallGrains = Zybach.EFModels.Entities.AgHubIrrigatedAcreCropTypeSmallGrains.Instance;
        public static readonly AgHubIrrigatedAcreCropTypeWinterWheat WinterWheat = Zybach.EFModels.Entities.AgHubIrrigatedAcreCropTypeWinterWheat.Instance;
        public static readonly AgHubIrrigatedAcreCropTypeFallowFields FallowFields = Zybach.EFModels.Entities.AgHubIrrigatedAcreCropTypeFallowFields.Instance;
        public static readonly AgHubIrrigatedAcreCropTypeSunflower Sunflower = Zybach.EFModels.Entities.AgHubIrrigatedAcreCropTypeSunflower.Instance;
        public static readonly AgHubIrrigatedAcreCropTypeSugarBeets SugarBeets = Zybach.EFModels.Entities.AgHubIrrigatedAcreCropTypeSugarBeets.Instance;
        public static readonly AgHubIrrigatedAcreCropTypePotatoes Potatoes = Zybach.EFModels.Entities.AgHubIrrigatedAcreCropTypePotatoes.Instance;
        public static readonly AgHubIrrigatedAcreCropTypeRangePastureGrassland RangePastureGrassland = Zybach.EFModels.Entities.AgHubIrrigatedAcreCropTypeRangePastureGrassland.Instance;
        public static readonly AgHubIrrigatedAcreCropTypeForage Forage = Zybach.EFModels.Entities.AgHubIrrigatedAcreCropTypeForage.Instance;
        public static readonly AgHubIrrigatedAcreCropTypeTurfGrass TurfGrass = Zybach.EFModels.Entities.AgHubIrrigatedAcreCropTypeTurfGrass.Instance;
        public static readonly AgHubIrrigatedAcreCropTypePasture Pasture = Zybach.EFModels.Entities.AgHubIrrigatedAcreCropTypePasture.Instance;
        public static readonly AgHubIrrigatedAcreCropTypeOther Other = Zybach.EFModels.Entities.AgHubIrrigatedAcreCropTypeOther.Instance;
        public static readonly AgHubIrrigatedAcreCropTypeNotReported NotReported = Zybach.EFModels.Entities.AgHubIrrigatedAcreCropTypeNotReported.Instance;

        public static readonly List<AgHubIrrigatedAcreCropType> All;
        public static readonly List<AgHubIrrigatedAcreCropTypeDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, AgHubIrrigatedAcreCropType> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, AgHubIrrigatedAcreCropTypeDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static AgHubIrrigatedAcreCropType()
        {
            All = new List<AgHubIrrigatedAcreCropType> { Corn, Popcorn, Soybeans, Sorghum, DryEdibleBeans, Alfalfa, SmallGrains, WinterWheat, FallowFields, Sunflower, SugarBeets, Potatoes, RangePastureGrassland, Forage, TurfGrass, Pasture, Other, NotReported };
            AllAsDto = new List<AgHubIrrigatedAcreCropTypeDto> { Corn.AsDto(), Popcorn.AsDto(), Soybeans.AsDto(), Sorghum.AsDto(), DryEdibleBeans.AsDto(), Alfalfa.AsDto(), SmallGrains.AsDto(), WinterWheat.AsDto(), FallowFields.AsDto(), Sunflower.AsDto(), SugarBeets.AsDto(), Potatoes.AsDto(), RangePastureGrassland.AsDto(), Forage.AsDto(), TurfGrass.AsDto(), Pasture.AsDto(), Other.AsDto(), NotReported.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, AgHubIrrigatedAcreCropType>(All.ToDictionary(x => x.AgHubIrrigatedAcreCropTypeID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, AgHubIrrigatedAcreCropTypeDto>(AllAsDto.ToDictionary(x => x.AgHubIrrigatedAcreCropTypeID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected AgHubIrrigatedAcreCropType(int agHubIrrigatedAcreCropTypeID, string agHubIrrigatedAcreCropTypeName, string mapColor)
        {
            AgHubIrrigatedAcreCropTypeID = agHubIrrigatedAcreCropTypeID;
            AgHubIrrigatedAcreCropTypeName = agHubIrrigatedAcreCropTypeName;
            MapColor = mapColor;
        }

        [Key]
        public int AgHubIrrigatedAcreCropTypeID { get; private set; }
        public string AgHubIrrigatedAcreCropTypeName { get; private set; }
        public string MapColor { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return AgHubIrrigatedAcreCropTypeID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(AgHubIrrigatedAcreCropType other)
        {
            if (other == null)
            {
                return false;
            }
            return other.AgHubIrrigatedAcreCropTypeID == AgHubIrrigatedAcreCropTypeID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as AgHubIrrigatedAcreCropType);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return AgHubIrrigatedAcreCropTypeID;
        }

        public static bool operator ==(AgHubIrrigatedAcreCropType left, AgHubIrrigatedAcreCropType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AgHubIrrigatedAcreCropType left, AgHubIrrigatedAcreCropType right)
        {
            return !Equals(left, right);
        }

        public AgHubIrrigatedAcreCropTypeEnum ToEnum => (AgHubIrrigatedAcreCropTypeEnum)GetHashCode();

        public static AgHubIrrigatedAcreCropType ToType(int enumValue)
        {
            return ToType((AgHubIrrigatedAcreCropTypeEnum)enumValue);
        }

        public static AgHubIrrigatedAcreCropType ToType(AgHubIrrigatedAcreCropTypeEnum enumValue)
        {
            switch (enumValue)
            {
                case AgHubIrrigatedAcreCropTypeEnum.Alfalfa:
                    return Alfalfa;
                case AgHubIrrigatedAcreCropTypeEnum.Corn:
                    return Corn;
                case AgHubIrrigatedAcreCropTypeEnum.DryEdibleBeans:
                    return DryEdibleBeans;
                case AgHubIrrigatedAcreCropTypeEnum.FallowFields:
                    return FallowFields;
                case AgHubIrrigatedAcreCropTypeEnum.Forage:
                    return Forage;
                case AgHubIrrigatedAcreCropTypeEnum.NotReported:
                    return NotReported;
                case AgHubIrrigatedAcreCropTypeEnum.Other:
                    return Other;
                case AgHubIrrigatedAcreCropTypeEnum.Pasture:
                    return Pasture;
                case AgHubIrrigatedAcreCropTypeEnum.Popcorn:
                    return Popcorn;
                case AgHubIrrigatedAcreCropTypeEnum.Potatoes:
                    return Potatoes;
                case AgHubIrrigatedAcreCropTypeEnum.RangePastureGrassland:
                    return RangePastureGrassland;
                case AgHubIrrigatedAcreCropTypeEnum.SmallGrains:
                    return SmallGrains;
                case AgHubIrrigatedAcreCropTypeEnum.Sorghum:
                    return Sorghum;
                case AgHubIrrigatedAcreCropTypeEnum.Soybeans:
                    return Soybeans;
                case AgHubIrrigatedAcreCropTypeEnum.SugarBeets:
                    return SugarBeets;
                case AgHubIrrigatedAcreCropTypeEnum.Sunflower:
                    return Sunflower;
                case AgHubIrrigatedAcreCropTypeEnum.TurfGrass:
                    return TurfGrass;
                case AgHubIrrigatedAcreCropTypeEnum.WinterWheat:
                    return WinterWheat;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum AgHubIrrigatedAcreCropTypeEnum
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
        Other = 99,
        NotReported = 100
    }

    public partial class AgHubIrrigatedAcreCropTypeCorn : AgHubIrrigatedAcreCropType
    {
        private AgHubIrrigatedAcreCropTypeCorn(int agHubIrrigatedAcreCropTypeID, string agHubIrrigatedAcreCropTypeName, string mapColor) : base(agHubIrrigatedAcreCropTypeID, agHubIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubIrrigatedAcreCropTypeCorn Instance = new AgHubIrrigatedAcreCropTypeCorn(1, @"Corn", @"#00b600");
    }

    public partial class AgHubIrrigatedAcreCropTypePopcorn : AgHubIrrigatedAcreCropType
    {
        private AgHubIrrigatedAcreCropTypePopcorn(int agHubIrrigatedAcreCropTypeID, string agHubIrrigatedAcreCropTypeName, string mapColor) : base(agHubIrrigatedAcreCropTypeID, agHubIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubIrrigatedAcreCropTypePopcorn Instance = new AgHubIrrigatedAcreCropTypePopcorn(2, @"Popcorn", @"#007b00");
    }

    public partial class AgHubIrrigatedAcreCropTypeSoybeans : AgHubIrrigatedAcreCropType
    {
        private AgHubIrrigatedAcreCropTypeSoybeans(int agHubIrrigatedAcreCropTypeID, string agHubIrrigatedAcreCropTypeName, string mapColor) : base(agHubIrrigatedAcreCropTypeID, agHubIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubIrrigatedAcreCropTypeSoybeans Instance = new AgHubIrrigatedAcreCropTypeSoybeans(3, @"Soybeans", @"#003e00");
    }

    public partial class AgHubIrrigatedAcreCropTypeSorghum : AgHubIrrigatedAcreCropType
    {
        private AgHubIrrigatedAcreCropTypeSorghum(int agHubIrrigatedAcreCropTypeID, string agHubIrrigatedAcreCropTypeName, string mapColor) : base(agHubIrrigatedAcreCropTypeID, agHubIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubIrrigatedAcreCropTypeSorghum Instance = new AgHubIrrigatedAcreCropTypeSorghum(4, @"Sorghum", @"#d9ae00");
    }

    public partial class AgHubIrrigatedAcreCropTypeDryEdibleBeans : AgHubIrrigatedAcreCropType
    {
        private AgHubIrrigatedAcreCropTypeDryEdibleBeans(int agHubIrrigatedAcreCropTypeID, string agHubIrrigatedAcreCropTypeName, string mapColor) : base(agHubIrrigatedAcreCropTypeID, agHubIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubIrrigatedAcreCropTypeDryEdibleBeans Instance = new AgHubIrrigatedAcreCropTypeDryEdibleBeans(5, @"Dry Edible Beans", @"#d57c00");
    }

    public partial class AgHubIrrigatedAcreCropTypeAlfalfa : AgHubIrrigatedAcreCropType
    {
        private AgHubIrrigatedAcreCropTypeAlfalfa(int agHubIrrigatedAcreCropTypeID, string agHubIrrigatedAcreCropTypeName, string mapColor) : base(agHubIrrigatedAcreCropTypeID, agHubIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubIrrigatedAcreCropTypeAlfalfa Instance = new AgHubIrrigatedAcreCropTypeAlfalfa(6, @"Alfalfa", @"#dade00");
    }

    public partial class AgHubIrrigatedAcreCropTypeSmallGrains : AgHubIrrigatedAcreCropType
    {
        private AgHubIrrigatedAcreCropTypeSmallGrains(int agHubIrrigatedAcreCropTypeID, string agHubIrrigatedAcreCropTypeName, string mapColor) : base(agHubIrrigatedAcreCropTypeID, agHubIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubIrrigatedAcreCropTypeSmallGrains Instance = new AgHubIrrigatedAcreCropTypeSmallGrains(7, @"Small Grains", @"#d500d9");
    }

    public partial class AgHubIrrigatedAcreCropTypeWinterWheat : AgHubIrrigatedAcreCropType
    {
        private AgHubIrrigatedAcreCropTypeWinterWheat(int agHubIrrigatedAcreCropTypeID, string agHubIrrigatedAcreCropTypeName, string mapColor) : base(agHubIrrigatedAcreCropTypeID, agHubIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubIrrigatedAcreCropTypeWinterWheat Instance = new AgHubIrrigatedAcreCropTypeWinterWheat(8, @"Winter Wheat", @"#b521b8");
    }

    public partial class AgHubIrrigatedAcreCropTypeFallowFields : AgHubIrrigatedAcreCropType
    {
        private AgHubIrrigatedAcreCropTypeFallowFields(int agHubIrrigatedAcreCropTypeID, string agHubIrrigatedAcreCropTypeName, string mapColor) : base(agHubIrrigatedAcreCropTypeID, agHubIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubIrrigatedAcreCropTypeFallowFields Instance = new AgHubIrrigatedAcreCropTypeFallowFields(9, @"Fallow Fields", @"#d9d9d9");
    }

    public partial class AgHubIrrigatedAcreCropTypeSunflower : AgHubIrrigatedAcreCropType
    {
        private AgHubIrrigatedAcreCropTypeSunflower(int agHubIrrigatedAcreCropTypeID, string agHubIrrigatedAcreCropTypeName, string mapColor) : base(agHubIrrigatedAcreCropTypeID, agHubIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubIrrigatedAcreCropTypeSunflower Instance = new AgHubIrrigatedAcreCropTypeSunflower(10, @"Sunflower", @"#d890a2");
    }

    public partial class AgHubIrrigatedAcreCropTypeSugarBeets : AgHubIrrigatedAcreCropType
    {
        private AgHubIrrigatedAcreCropTypeSugarBeets(int agHubIrrigatedAcreCropTypeID, string agHubIrrigatedAcreCropTypeName, string mapColor) : base(agHubIrrigatedAcreCropTypeID, agHubIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubIrrigatedAcreCropTypeSugarBeets Instance = new AgHubIrrigatedAcreCropTypeSugarBeets(11, @"Sugar Beets", @"#7000cb");
    }

    public partial class AgHubIrrigatedAcreCropTypePotatoes : AgHubIrrigatedAcreCropType
    {
        private AgHubIrrigatedAcreCropTypePotatoes(int agHubIrrigatedAcreCropTypeID, string agHubIrrigatedAcreCropTypeName, string mapColor) : base(agHubIrrigatedAcreCropTypeID, agHubIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubIrrigatedAcreCropTypePotatoes Instance = new AgHubIrrigatedAcreCropTypePotatoes(12, @"Potatoes", @"#780012");
    }

    public partial class AgHubIrrigatedAcreCropTypeRangePastureGrassland : AgHubIrrigatedAcreCropType
    {
        private AgHubIrrigatedAcreCropTypeRangePastureGrassland(int agHubIrrigatedAcreCropTypeID, string agHubIrrigatedAcreCropTypeName, string mapColor) : base(agHubIrrigatedAcreCropTypeID, agHubIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubIrrigatedAcreCropTypeRangePastureGrassland Instance = new AgHubIrrigatedAcreCropTypeRangePastureGrassland(13, @"Range, Pasture, Grassland", @"#a08c62");
    }

    public partial class AgHubIrrigatedAcreCropTypeForage : AgHubIrrigatedAcreCropType
    {
        private AgHubIrrigatedAcreCropTypeForage(int agHubIrrigatedAcreCropTypeID, string agHubIrrigatedAcreCropTypeName, string mapColor) : base(agHubIrrigatedAcreCropTypeID, agHubIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubIrrigatedAcreCropTypeForage Instance = new AgHubIrrigatedAcreCropTypeForage(14, @"Forage", @"#7c6c4b");
    }

    public partial class AgHubIrrigatedAcreCropTypeTurfGrass : AgHubIrrigatedAcreCropType
    {
        private AgHubIrrigatedAcreCropTypeTurfGrass(int agHubIrrigatedAcreCropTypeID, string agHubIrrigatedAcreCropTypeName, string mapColor) : base(agHubIrrigatedAcreCropTypeID, agHubIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubIrrigatedAcreCropTypeTurfGrass Instance = new AgHubIrrigatedAcreCropTypeTurfGrass(15, @"Turf Grass", @"#574c35");
    }

    public partial class AgHubIrrigatedAcreCropTypePasture : AgHubIrrigatedAcreCropType
    {
        private AgHubIrrigatedAcreCropTypePasture(int agHubIrrigatedAcreCropTypeID, string agHubIrrigatedAcreCropTypeName, string mapColor) : base(agHubIrrigatedAcreCropTypeID, agHubIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubIrrigatedAcreCropTypePasture Instance = new AgHubIrrigatedAcreCropTypePasture(16, @"Pasture", @"#a08c62");
    }

    public partial class AgHubIrrigatedAcreCropTypeOther : AgHubIrrigatedAcreCropType
    {
        private AgHubIrrigatedAcreCropTypeOther(int agHubIrrigatedAcreCropTypeID, string agHubIrrigatedAcreCropTypeName, string mapColor) : base(agHubIrrigatedAcreCropTypeID, agHubIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubIrrigatedAcreCropTypeOther Instance = new AgHubIrrigatedAcreCropTypeOther(99, @"Other", @"#00b6b6");
    }

    public partial class AgHubIrrigatedAcreCropTypeNotReported : AgHubIrrigatedAcreCropType
    {
        private AgHubIrrigatedAcreCropTypeNotReported(int agHubIrrigatedAcreCropTypeID, string agHubIrrigatedAcreCropTypeName, string mapColor) : base(agHubIrrigatedAcreCropTypeID, agHubIrrigatedAcreCropTypeName, mapColor) {}
        public static readonly AgHubIrrigatedAcreCropTypeNotReported Instance = new AgHubIrrigatedAcreCropTypeNotReported(100, @"Not Reported", @"#e22e1d");
    }
}