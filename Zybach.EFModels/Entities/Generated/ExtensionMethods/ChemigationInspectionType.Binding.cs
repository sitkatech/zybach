//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationInspectionType]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class ChemigationInspectionType
    {
        public static readonly ChemigationInspectionTypeEQUIPMENTREPAIRORREPLACE EQUIPMENTREPAIRORREPLACE = Zybach.EFModels.Entities.ChemigationInspectionTypeEQUIPMENTREPAIRORREPLACE.Instance;
        public static readonly ChemigationInspectionTypeNEWINITIALORREACTIVATION NEWINITIALORREACTIVATION = Zybach.EFModels.Entities.ChemigationInspectionTypeNEWINITIALORREACTIVATION.Instance;
        public static readonly ChemigationInspectionTypeRENEWALROUTINEMONITORING RENEWALROUTINEMONITORING = Zybach.EFModels.Entities.ChemigationInspectionTypeRENEWALROUTINEMONITORING.Instance;

        public static readonly List<ChemigationInspectionType> All;
        public static readonly List<ChemigationInspectionTypeDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, ChemigationInspectionType> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, ChemigationInspectionTypeDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static ChemigationInspectionType()
        {
            All = new List<ChemigationInspectionType> { EQUIPMENTREPAIRORREPLACE, NEWINITIALORREACTIVATION, RENEWALROUTINEMONITORING };
            AllAsDto = new List<ChemigationInspectionTypeDto> { EQUIPMENTREPAIRORREPLACE.AsDto(), NEWINITIALORREACTIVATION.AsDto(), RENEWALROUTINEMONITORING.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, ChemigationInspectionType>(All.ToDictionary(x => x.ChemigationInspectionTypeID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, ChemigationInspectionTypeDto>(AllAsDto.ToDictionary(x => x.ChemigationInspectionTypeID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected ChemigationInspectionType(int chemigationInspectionTypeID, string chemigationInspectionTypeName, string chemigationInspectionTypeDisplayName)
        {
            ChemigationInspectionTypeID = chemigationInspectionTypeID;
            ChemigationInspectionTypeName = chemigationInspectionTypeName;
            ChemigationInspectionTypeDisplayName = chemigationInspectionTypeDisplayName;
        }

        [Key]
        public int ChemigationInspectionTypeID { get; private set; }
        public string ChemigationInspectionTypeName { get; private set; }
        public string ChemigationInspectionTypeDisplayName { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return ChemigationInspectionTypeID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(ChemigationInspectionType other)
        {
            if (other == null)
            {
                return false;
            }
            return other.ChemigationInspectionTypeID == ChemigationInspectionTypeID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as ChemigationInspectionType);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return ChemigationInspectionTypeID;
        }

        public static bool operator ==(ChemigationInspectionType left, ChemigationInspectionType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ChemigationInspectionType left, ChemigationInspectionType right)
        {
            return !Equals(left, right);
        }

        public ChemigationInspectionTypeEnum ToEnum => (ChemigationInspectionTypeEnum)GetHashCode();

        public static ChemigationInspectionType ToType(int enumValue)
        {
            return ToType((ChemigationInspectionTypeEnum)enumValue);
        }

        public static ChemigationInspectionType ToType(ChemigationInspectionTypeEnum enumValue)
        {
            switch (enumValue)
            {
                case ChemigationInspectionTypeEnum.EQUIPMENTREPAIRORREPLACE:
                    return EQUIPMENTREPAIRORREPLACE;
                case ChemigationInspectionTypeEnum.NEWINITIALORREACTIVATION:
                    return NEWINITIALORREACTIVATION;
                case ChemigationInspectionTypeEnum.RENEWALROUTINEMONITORING:
                    return RENEWALROUTINEMONITORING;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum ChemigationInspectionTypeEnum
    {
        EQUIPMENTREPAIRORREPLACE = 1,
        NEWINITIALORREACTIVATION = 2,
        RENEWALROUTINEMONITORING = 3
    }

    public partial class ChemigationInspectionTypeEQUIPMENTREPAIRORREPLACE : ChemigationInspectionType
    {
        private ChemigationInspectionTypeEQUIPMENTREPAIRORREPLACE(int chemigationInspectionTypeID, string chemigationInspectionTypeName, string chemigationInspectionTypeDisplayName) : base(chemigationInspectionTypeID, chemigationInspectionTypeName, chemigationInspectionTypeDisplayName) {}
        public static readonly ChemigationInspectionTypeEQUIPMENTREPAIRORREPLACE Instance = new ChemigationInspectionTypeEQUIPMENTREPAIRORREPLACE(1, @"EQUIPMENT REPAIR OR REPLACE", @"EQUIPMENT REPAIR OR REPLACE");
    }

    public partial class ChemigationInspectionTypeNEWINITIALORREACTIVATION : ChemigationInspectionType
    {
        private ChemigationInspectionTypeNEWINITIALORREACTIVATION(int chemigationInspectionTypeID, string chemigationInspectionTypeName, string chemigationInspectionTypeDisplayName) : base(chemigationInspectionTypeID, chemigationInspectionTypeName, chemigationInspectionTypeDisplayName) {}
        public static readonly ChemigationInspectionTypeNEWINITIALORREACTIVATION Instance = new ChemigationInspectionTypeNEWINITIALORREACTIVATION(2, @"NEW - INITIAL OR RE-ACTIVATION", @"NEW - INITIAL OR RE-ACTIVATION");
    }

    public partial class ChemigationInspectionTypeRENEWALROUTINEMONITORING : ChemigationInspectionType
    {
        private ChemigationInspectionTypeRENEWALROUTINEMONITORING(int chemigationInspectionTypeID, string chemigationInspectionTypeName, string chemigationInspectionTypeDisplayName) : base(chemigationInspectionTypeID, chemigationInspectionTypeName, chemigationInspectionTypeDisplayName) {}
        public static readonly ChemigationInspectionTypeRENEWALROUTINEMONITORING Instance = new ChemigationInspectionTypeRENEWALROUTINEMONITORING(3, @"RENEWAL - ROUTINE MONITORING", @"RENEWAL - ROUTINE MONITORING");
    }
}