//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationInterlockType]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class ChemigationInterlockType : IHavePrimaryKey
    {
        public static readonly ChemigationInterlockTypeMechanical Mechanical = Zybach.EFModels.Entities.ChemigationInterlockTypeMechanical.Instance;
        public static readonly ChemigationInterlockTypeElectrical Electrical = Zybach.EFModels.Entities.ChemigationInterlockTypeElectrical.Instance;

        public static readonly List<ChemigationInterlockType> All;
        public static readonly List<ChemigationInterlockTypeDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, ChemigationInterlockType> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, ChemigationInterlockTypeDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static ChemigationInterlockType()
        {
            All = new List<ChemigationInterlockType> { Mechanical, Electrical };
            AllAsDto = new List<ChemigationInterlockTypeDto> { Mechanical.AsDto(), Electrical.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, ChemigationInterlockType>(All.ToDictionary(x => x.ChemigationInterlockTypeID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, ChemigationInterlockTypeDto>(AllAsDto.ToDictionary(x => x.ChemigationInterlockTypeID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected ChemigationInterlockType(int chemigationInterlockTypeID, string chemigationInterlockTypeName, string chemigationInterlockTypeDisplayName)
        {
            ChemigationInterlockTypeID = chemigationInterlockTypeID;
            ChemigationInterlockTypeName = chemigationInterlockTypeName;
            ChemigationInterlockTypeDisplayName = chemigationInterlockTypeDisplayName;
        }

        [Key]
        public int ChemigationInterlockTypeID { get; private set; }
        public string ChemigationInterlockTypeName { get; private set; }
        public string ChemigationInterlockTypeDisplayName { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return ChemigationInterlockTypeID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(ChemigationInterlockType other)
        {
            if (other == null)
            {
                return false;
            }
            return other.ChemigationInterlockTypeID == ChemigationInterlockTypeID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as ChemigationInterlockType);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return ChemigationInterlockTypeID;
        }

        public static bool operator ==(ChemigationInterlockType left, ChemigationInterlockType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ChemigationInterlockType left, ChemigationInterlockType right)
        {
            return !Equals(left, right);
        }

        public ChemigationInterlockTypeEnum ToEnum => (ChemigationInterlockTypeEnum)GetHashCode();

        public static ChemigationInterlockType ToType(int enumValue)
        {
            return ToType((ChemigationInterlockTypeEnum)enumValue);
        }

        public static ChemigationInterlockType ToType(ChemigationInterlockTypeEnum enumValue)
        {
            switch (enumValue)
            {
                case ChemigationInterlockTypeEnum.Electrical:
                    return Electrical;
                case ChemigationInterlockTypeEnum.Mechanical:
                    return Mechanical;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum ChemigationInterlockTypeEnum
    {
        Mechanical = 1,
        Electrical = 2
    }

    public partial class ChemigationInterlockTypeMechanical : ChemigationInterlockType
    {
        private ChemigationInterlockTypeMechanical(int chemigationInterlockTypeID, string chemigationInterlockTypeName, string chemigationInterlockTypeDisplayName) : base(chemigationInterlockTypeID, chemigationInterlockTypeName, chemigationInterlockTypeDisplayName) {}
        public static readonly ChemigationInterlockTypeMechanical Instance = new ChemigationInterlockTypeMechanical(1, @"Mechanical", @"Mechanical");
    }

    public partial class ChemigationInterlockTypeElectrical : ChemigationInterlockType
    {
        private ChemigationInterlockTypeElectrical(int chemigationInterlockTypeID, string chemigationInterlockTypeName, string chemigationInterlockTypeDisplayName) : base(chemigationInterlockTypeID, chemigationInterlockTypeName, chemigationInterlockTypeDisplayName) {}
        public static readonly ChemigationInterlockTypeElectrical Instance = new ChemigationInterlockTypeElectrical(2, @"Electrical", @"Electrical");
    }
}