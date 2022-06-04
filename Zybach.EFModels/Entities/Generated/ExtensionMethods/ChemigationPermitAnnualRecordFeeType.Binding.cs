//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermitAnnualRecordFeeType]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class ChemigationPermitAnnualRecordFeeType
    {
        public static readonly ChemigationPermitAnnualRecordFeeTypeNew New = Zybach.EFModels.Entities.ChemigationPermitAnnualRecordFeeTypeNew.Instance;
        public static readonly ChemigationPermitAnnualRecordFeeTypeRenewal Renewal = Zybach.EFModels.Entities.ChemigationPermitAnnualRecordFeeTypeRenewal.Instance;
        public static readonly ChemigationPermitAnnualRecordFeeTypeEmergency Emergency = Zybach.EFModels.Entities.ChemigationPermitAnnualRecordFeeTypeEmergency.Instance;

        public static readonly List<ChemigationPermitAnnualRecordFeeType> All;
        public static readonly List<ChemigationPermitAnnualRecordFeeTypeDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, ChemigationPermitAnnualRecordFeeType> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, ChemigationPermitAnnualRecordFeeTypeDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static ChemigationPermitAnnualRecordFeeType()
        {
            All = new List<ChemigationPermitAnnualRecordFeeType> { New, Renewal, Emergency };
            AllAsDto = new List<ChemigationPermitAnnualRecordFeeTypeDto> { New.AsDto(), Renewal.AsDto(), Emergency.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, ChemigationPermitAnnualRecordFeeType>(All.ToDictionary(x => x.ChemigationPermitAnnualRecordFeeTypeID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, ChemigationPermitAnnualRecordFeeTypeDto>(AllAsDto.ToDictionary(x => x.ChemigationPermitAnnualRecordFeeTypeID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected ChemigationPermitAnnualRecordFeeType(int chemigationPermitAnnualRecordFeeTypeID, string chemigationPermitAnnualRecordFeeTypeName, string chemigationPermitAnnualRecordFeeTypeDisplayName, decimal feeAmount)
        {
            ChemigationPermitAnnualRecordFeeTypeID = chemigationPermitAnnualRecordFeeTypeID;
            ChemigationPermitAnnualRecordFeeTypeName = chemigationPermitAnnualRecordFeeTypeName;
            ChemigationPermitAnnualRecordFeeTypeDisplayName = chemigationPermitAnnualRecordFeeTypeDisplayName;
            FeeAmount = feeAmount;
        }

        [Key]
        public int ChemigationPermitAnnualRecordFeeTypeID { get; private set; }
        public string ChemigationPermitAnnualRecordFeeTypeName { get; private set; }
        public string ChemigationPermitAnnualRecordFeeTypeDisplayName { get; private set; }
        public decimal FeeAmount { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return ChemigationPermitAnnualRecordFeeTypeID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(ChemigationPermitAnnualRecordFeeType other)
        {
            if (other == null)
            {
                return false;
            }
            return other.ChemigationPermitAnnualRecordFeeTypeID == ChemigationPermitAnnualRecordFeeTypeID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as ChemigationPermitAnnualRecordFeeType);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return ChemigationPermitAnnualRecordFeeTypeID;
        }

        public static bool operator ==(ChemigationPermitAnnualRecordFeeType left, ChemigationPermitAnnualRecordFeeType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ChemigationPermitAnnualRecordFeeType left, ChemigationPermitAnnualRecordFeeType right)
        {
            return !Equals(left, right);
        }

        public ChemigationPermitAnnualRecordFeeTypeEnum ToEnum => (ChemigationPermitAnnualRecordFeeTypeEnum)GetHashCode();

        public static ChemigationPermitAnnualRecordFeeType ToType(int enumValue)
        {
            return ToType((ChemigationPermitAnnualRecordFeeTypeEnum)enumValue);
        }

        public static ChemigationPermitAnnualRecordFeeType ToType(ChemigationPermitAnnualRecordFeeTypeEnum enumValue)
        {
            switch (enumValue)
            {
                case ChemigationPermitAnnualRecordFeeTypeEnum.Emergency:
                    return Emergency;
                case ChemigationPermitAnnualRecordFeeTypeEnum.New:
                    return New;
                case ChemigationPermitAnnualRecordFeeTypeEnum.Renewal:
                    return Renewal;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum ChemigationPermitAnnualRecordFeeTypeEnum
    {
        New = 1,
        Renewal = 2,
        Emergency = 3
    }

    public partial class ChemigationPermitAnnualRecordFeeTypeNew : ChemigationPermitAnnualRecordFeeType
    {
        private ChemigationPermitAnnualRecordFeeTypeNew(int chemigationPermitAnnualRecordFeeTypeID, string chemigationPermitAnnualRecordFeeTypeName, string chemigationPermitAnnualRecordFeeTypeDisplayName, decimal feeAmount) : base(chemigationPermitAnnualRecordFeeTypeID, chemigationPermitAnnualRecordFeeTypeName, chemigationPermitAnnualRecordFeeTypeDisplayName, feeAmount) {}
        public static readonly ChemigationPermitAnnualRecordFeeTypeNew Instance = new ChemigationPermitAnnualRecordFeeTypeNew(1, @"New", @"New ($40)", 40.0000m);
    }

    public partial class ChemigationPermitAnnualRecordFeeTypeRenewal : ChemigationPermitAnnualRecordFeeType
    {
        private ChemigationPermitAnnualRecordFeeTypeRenewal(int chemigationPermitAnnualRecordFeeTypeID, string chemigationPermitAnnualRecordFeeTypeName, string chemigationPermitAnnualRecordFeeTypeDisplayName, decimal feeAmount) : base(chemigationPermitAnnualRecordFeeTypeID, chemigationPermitAnnualRecordFeeTypeName, chemigationPermitAnnualRecordFeeTypeDisplayName, feeAmount) {}
        public static readonly ChemigationPermitAnnualRecordFeeTypeRenewal Instance = new ChemigationPermitAnnualRecordFeeTypeRenewal(2, @"Renewal", @"Renewal ($20)", 20.0000m);
    }

    public partial class ChemigationPermitAnnualRecordFeeTypeEmergency : ChemigationPermitAnnualRecordFeeType
    {
        private ChemigationPermitAnnualRecordFeeTypeEmergency(int chemigationPermitAnnualRecordFeeTypeID, string chemigationPermitAnnualRecordFeeTypeName, string chemigationPermitAnnualRecordFeeTypeDisplayName, decimal feeAmount) : base(chemigationPermitAnnualRecordFeeTypeID, chemigationPermitAnnualRecordFeeTypeName, chemigationPermitAnnualRecordFeeTypeDisplayName, feeAmount) {}
        public static readonly ChemigationPermitAnnualRecordFeeTypeEmergency Instance = new ChemigationPermitAnnualRecordFeeTypeEmergency(3, @"Emergency", @"Emergency ($100)", 100.0000m);
    }
}