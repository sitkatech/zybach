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
        public static readonly ChemigationInspectionTypeEquipmentRepairOrReplace EquipmentRepairOrReplace = Zybach.EFModels.Entities.ChemigationInspectionTypeEquipmentRepairOrReplace.Instance;
        public static readonly ChemigationInspectionTypeNewInitialOrReactivation NewInitialOrReactivation = Zybach.EFModels.Entities.ChemigationInspectionTypeNewInitialOrReactivation.Instance;
        public static readonly ChemigationInspectionTypeRenewalRoutineMonitoring RenewalRoutineMonitoring = Zybach.EFModels.Entities.ChemigationInspectionTypeRenewalRoutineMonitoring.Instance;

        public static readonly List<ChemigationInspectionType> All;
        public static readonly List<ChemigationInspectionTypeDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, ChemigationInspectionType> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, ChemigationInspectionTypeDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static ChemigationInspectionType()
        {
            All = new List<ChemigationInspectionType> { EquipmentRepairOrReplace, NewInitialOrReactivation, RenewalRoutineMonitoring };
            AllAsDto = new List<ChemigationInspectionTypeDto> { EquipmentRepairOrReplace.AsDto(), NewInitialOrReactivation.AsDto(), RenewalRoutineMonitoring.AsDto() };
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
                case ChemigationInspectionTypeEnum.EquipmentRepairOrReplace:
                    return EquipmentRepairOrReplace;
                case ChemigationInspectionTypeEnum.NewInitialOrReactivation:
                    return NewInitialOrReactivation;
                case ChemigationInspectionTypeEnum.RenewalRoutineMonitoring:
                    return RenewalRoutineMonitoring;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum ChemigationInspectionTypeEnum
    {
        EquipmentRepairOrReplace = 1,
        NewInitialOrReactivation = 2,
        RenewalRoutineMonitoring = 3
    }

    public partial class ChemigationInspectionTypeEquipmentRepairOrReplace : ChemigationInspectionType
    {
        private ChemigationInspectionTypeEquipmentRepairOrReplace(int chemigationInspectionTypeID, string chemigationInspectionTypeName, string chemigationInspectionTypeDisplayName) : base(chemigationInspectionTypeID, chemigationInspectionTypeName, chemigationInspectionTypeDisplayName) {}
        public static readonly ChemigationInspectionTypeEquipmentRepairOrReplace Instance = new ChemigationInspectionTypeEquipmentRepairOrReplace(1, @"EquipmentRepairOrReplace", @"Equipment Repair or Replace");
    }

    public partial class ChemigationInspectionTypeNewInitialOrReactivation : ChemigationInspectionType
    {
        private ChemigationInspectionTypeNewInitialOrReactivation(int chemigationInspectionTypeID, string chemigationInspectionTypeName, string chemigationInspectionTypeDisplayName) : base(chemigationInspectionTypeID, chemigationInspectionTypeName, chemigationInspectionTypeDisplayName) {}
        public static readonly ChemigationInspectionTypeNewInitialOrReactivation Instance = new ChemigationInspectionTypeNewInitialOrReactivation(2, @"NewInitialOrReactivation", @"New - Initial or Re-activation");
    }

    public partial class ChemigationInspectionTypeRenewalRoutineMonitoring : ChemigationInspectionType
    {
        private ChemigationInspectionTypeRenewalRoutineMonitoring(int chemigationInspectionTypeID, string chemigationInspectionTypeName, string chemigationInspectionTypeDisplayName) : base(chemigationInspectionTypeID, chemigationInspectionTypeName, chemigationInspectionTypeDisplayName) {}
        public static readonly ChemigationInspectionTypeRenewalRoutineMonitoring Instance = new ChemigationInspectionTypeRenewalRoutineMonitoring(3, @"RenewalRoutineMonitoring", @"Renewal - Routine Monitoring");
    }
}