//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermitStatus]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class ChemigationPermitStatus
    {
        public static readonly ChemigationPermitStatusActive Active = Zybach.EFModels.Entities.ChemigationPermitStatusActive.Instance;
        public static readonly ChemigationPermitStatusInactive Inactive = Zybach.EFModels.Entities.ChemigationPermitStatusInactive.Instance;
        public static readonly ChemigationPermitStatusPermInactive PermInactive = Zybach.EFModels.Entities.ChemigationPermitStatusPermInactive.Instance;

        public static readonly List<ChemigationPermitStatus> All;
        public static readonly List<ChemigationPermitStatusDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, ChemigationPermitStatus> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, ChemigationPermitStatusDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static ChemigationPermitStatus()
        {
            All = new List<ChemigationPermitStatus> { Active, Inactive, PermInactive };
            AllAsDto = new List<ChemigationPermitStatusDto> { Active.AsDto(), Inactive.AsDto(), PermInactive.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, ChemigationPermitStatus>(All.ToDictionary(x => x.ChemigationPermitStatusID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, ChemigationPermitStatusDto>(AllAsDto.ToDictionary(x => x.ChemigationPermitStatusID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected ChemigationPermitStatus(int chemigationPermitStatusID, string chemigationPermitStatusName, string chemigationPermitStatusDisplayName)
        {
            ChemigationPermitStatusID = chemigationPermitStatusID;
            ChemigationPermitStatusName = chemigationPermitStatusName;
            ChemigationPermitStatusDisplayName = chemigationPermitStatusDisplayName;
        }

        [Key]
        public int ChemigationPermitStatusID { get; private set; }
        public string ChemigationPermitStatusName { get; private set; }
        public string ChemigationPermitStatusDisplayName { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return ChemigationPermitStatusID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(ChemigationPermitStatus other)
        {
            if (other == null)
            {
                return false;
            }
            return other.ChemigationPermitStatusID == ChemigationPermitStatusID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as ChemigationPermitStatus);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return ChemigationPermitStatusID;
        }

        public static bool operator ==(ChemigationPermitStatus left, ChemigationPermitStatus right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ChemigationPermitStatus left, ChemigationPermitStatus right)
        {
            return !Equals(left, right);
        }

        public ChemigationPermitStatusEnum ToEnum => (ChemigationPermitStatusEnum)GetHashCode();

        public static ChemigationPermitStatus ToType(int enumValue)
        {
            return ToType((ChemigationPermitStatusEnum)enumValue);
        }

        public static ChemigationPermitStatus ToType(ChemigationPermitStatusEnum enumValue)
        {
            switch (enumValue)
            {
                case ChemigationPermitStatusEnum.Active:
                    return Active;
                case ChemigationPermitStatusEnum.Inactive:
                    return Inactive;
                case ChemigationPermitStatusEnum.PermInactive:
                    return PermInactive;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum ChemigationPermitStatusEnum
    {
        Active = 1,
        Inactive = 2,
        PermInactive = 3
    }

    public partial class ChemigationPermitStatusActive : ChemigationPermitStatus
    {
        private ChemigationPermitStatusActive(int chemigationPermitStatusID, string chemigationPermitStatusName, string chemigationPermitStatusDisplayName) : base(chemigationPermitStatusID, chemigationPermitStatusName, chemigationPermitStatusDisplayName) {}
        public static readonly ChemigationPermitStatusActive Instance = new ChemigationPermitStatusActive(1, @"Active", @"Active");
    }

    public partial class ChemigationPermitStatusInactive : ChemigationPermitStatus
    {
        private ChemigationPermitStatusInactive(int chemigationPermitStatusID, string chemigationPermitStatusName, string chemigationPermitStatusDisplayName) : base(chemigationPermitStatusID, chemigationPermitStatusName, chemigationPermitStatusDisplayName) {}
        public static readonly ChemigationPermitStatusInactive Instance = new ChemigationPermitStatusInactive(2, @"Inactive", @"Inactive");
    }

    public partial class ChemigationPermitStatusPermInactive : ChemigationPermitStatus
    {
        private ChemigationPermitStatusPermInactive(int chemigationPermitStatusID, string chemigationPermitStatusName, string chemigationPermitStatusDisplayName) : base(chemigationPermitStatusID, chemigationPermitStatusName, chemigationPermitStatusDisplayName) {}
        public static readonly ChemigationPermitStatusPermInactive Instance = new ChemigationPermitStatusPermInactive(3, @"PermInactive", @"Permanently Inactive");
    }
}