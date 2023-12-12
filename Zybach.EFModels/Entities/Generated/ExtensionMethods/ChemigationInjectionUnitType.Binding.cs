//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationInjectionUnitType]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class ChemigationInjectionUnitType : IHavePrimaryKey
    {
        public static readonly ChemigationInjectionUnitTypePortable Portable = Zybach.EFModels.Entities.ChemigationInjectionUnitTypePortable.Instance;
        public static readonly ChemigationInjectionUnitTypeStationary Stationary = Zybach.EFModels.Entities.ChemigationInjectionUnitTypeStationary.Instance;

        public static readonly List<ChemigationInjectionUnitType> All;
        public static readonly List<ChemigationInjectionUnitTypeDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, ChemigationInjectionUnitType> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, ChemigationInjectionUnitTypeDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static ChemigationInjectionUnitType()
        {
            All = new List<ChemigationInjectionUnitType> { Portable, Stationary };
            AllAsDto = new List<ChemigationInjectionUnitTypeDto> { Portable.AsDto(), Stationary.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, ChemigationInjectionUnitType>(All.ToDictionary(x => x.ChemigationInjectionUnitTypeID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, ChemigationInjectionUnitTypeDto>(AllAsDto.ToDictionary(x => x.ChemigationInjectionUnitTypeID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected ChemigationInjectionUnitType(int chemigationInjectionUnitTypeID, string chemigationInjectionUnitTypeName, string chemigationInjectionUnitTypeDisplayName)
        {
            ChemigationInjectionUnitTypeID = chemigationInjectionUnitTypeID;
            ChemigationInjectionUnitTypeName = chemigationInjectionUnitTypeName;
            ChemigationInjectionUnitTypeDisplayName = chemigationInjectionUnitTypeDisplayName;
        }

        [Key]
        public int ChemigationInjectionUnitTypeID { get; private set; }
        public string ChemigationInjectionUnitTypeName { get; private set; }
        public string ChemigationInjectionUnitTypeDisplayName { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return ChemigationInjectionUnitTypeID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(ChemigationInjectionUnitType other)
        {
            if (other == null)
            {
                return false;
            }
            return other.ChemigationInjectionUnitTypeID == ChemigationInjectionUnitTypeID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as ChemigationInjectionUnitType);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return ChemigationInjectionUnitTypeID;
        }

        public static bool operator ==(ChemigationInjectionUnitType left, ChemigationInjectionUnitType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ChemigationInjectionUnitType left, ChemigationInjectionUnitType right)
        {
            return !Equals(left, right);
        }

        public ChemigationInjectionUnitTypeEnum ToEnum => (ChemigationInjectionUnitTypeEnum)GetHashCode();

        public static ChemigationInjectionUnitType ToType(int enumValue)
        {
            return ToType((ChemigationInjectionUnitTypeEnum)enumValue);
        }

        public static ChemigationInjectionUnitType ToType(ChemigationInjectionUnitTypeEnum enumValue)
        {
            switch (enumValue)
            {
                case ChemigationInjectionUnitTypeEnum.Portable:
                    return Portable;
                case ChemigationInjectionUnitTypeEnum.Stationary:
                    return Stationary;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum ChemigationInjectionUnitTypeEnum
    {
        Portable = 1,
        Stationary = 2
    }

    public partial class ChemigationInjectionUnitTypePortable : ChemigationInjectionUnitType
    {
        private ChemigationInjectionUnitTypePortable(int chemigationInjectionUnitTypeID, string chemigationInjectionUnitTypeName, string chemigationInjectionUnitTypeDisplayName) : base(chemigationInjectionUnitTypeID, chemigationInjectionUnitTypeName, chemigationInjectionUnitTypeDisplayName) {}
        public static readonly ChemigationInjectionUnitTypePortable Instance = new ChemigationInjectionUnitTypePortable(1, @"Portable", @"Portable");
    }

    public partial class ChemigationInjectionUnitTypeStationary : ChemigationInjectionUnitType
    {
        private ChemigationInjectionUnitTypeStationary(int chemigationInjectionUnitTypeID, string chemigationInjectionUnitTypeName, string chemigationInjectionUnitTypeDisplayName) : base(chemigationInjectionUnitTypeID, chemigationInjectionUnitTypeName, chemigationInjectionUnitTypeDisplayName) {}
        public static readonly ChemigationInjectionUnitTypeStationary Instance = new ChemigationInjectionUnitTypeStationary(2, @"Stationary", @"Stationary");
    }
}