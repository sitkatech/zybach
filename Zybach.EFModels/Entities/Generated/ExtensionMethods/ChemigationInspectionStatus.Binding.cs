//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationInspectionStatus]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class ChemigationInspectionStatus
    {
        public static readonly ChemigationInspectionStatusPending Pending = Zybach.EFModels.Entities.ChemigationInspectionStatusPending.Instance;
        public static readonly ChemigationInspectionStatusPass Pass = Zybach.EFModels.Entities.ChemigationInspectionStatusPass.Instance;
        public static readonly ChemigationInspectionStatusFail Fail = Zybach.EFModels.Entities.ChemigationInspectionStatusFail.Instance;

        public static readonly List<ChemigationInspectionStatus> All;
        public static readonly List<ChemigationInspectionStatusDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, ChemigationInspectionStatus> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, ChemigationInspectionStatusDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static ChemigationInspectionStatus()
        {
            All = new List<ChemigationInspectionStatus> { Pending, Pass, Fail };
            AllAsDto = new List<ChemigationInspectionStatusDto> { Pending.AsDto(), Pass.AsDto(), Fail.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, ChemigationInspectionStatus>(All.ToDictionary(x => x.ChemigationInspectionStatusID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, ChemigationInspectionStatusDto>(AllAsDto.ToDictionary(x => x.ChemigationInspectionStatusID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected ChemigationInspectionStatus(int chemigationInspectionStatusID, string chemigationInspectionStatusName, string chemigationInspectionStatusDisplayName)
        {
            ChemigationInspectionStatusID = chemigationInspectionStatusID;
            ChemigationInspectionStatusName = chemigationInspectionStatusName;
            ChemigationInspectionStatusDisplayName = chemigationInspectionStatusDisplayName;
        }

        [Key]
        public int ChemigationInspectionStatusID { get; private set; }
        public string ChemigationInspectionStatusName { get; private set; }
        public string ChemigationInspectionStatusDisplayName { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return ChemigationInspectionStatusID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(ChemigationInspectionStatus other)
        {
            if (other == null)
            {
                return false;
            }
            return other.ChemigationInspectionStatusID == ChemigationInspectionStatusID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as ChemigationInspectionStatus);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return ChemigationInspectionStatusID;
        }

        public static bool operator ==(ChemigationInspectionStatus left, ChemigationInspectionStatus right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ChemigationInspectionStatus left, ChemigationInspectionStatus right)
        {
            return !Equals(left, right);
        }

        public ChemigationInspectionStatusEnum ToEnum => (ChemigationInspectionStatusEnum)GetHashCode();

        public static ChemigationInspectionStatus ToType(int enumValue)
        {
            return ToType((ChemigationInspectionStatusEnum)enumValue);
        }

        public static ChemigationInspectionStatus ToType(ChemigationInspectionStatusEnum enumValue)
        {
            switch (enumValue)
            {
                case ChemigationInspectionStatusEnum.Fail:
                    return Fail;
                case ChemigationInspectionStatusEnum.Pass:
                    return Pass;
                case ChemigationInspectionStatusEnum.Pending:
                    return Pending;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum ChemigationInspectionStatusEnum
    {
        Pending = 1,
        Pass = 2,
        Fail = 3
    }

    public partial class ChemigationInspectionStatusPending : ChemigationInspectionStatus
    {
        private ChemigationInspectionStatusPending(int chemigationInspectionStatusID, string chemigationInspectionStatusName, string chemigationInspectionStatusDisplayName) : base(chemigationInspectionStatusID, chemigationInspectionStatusName, chemigationInspectionStatusDisplayName) {}
        public static readonly ChemigationInspectionStatusPending Instance = new ChemigationInspectionStatusPending(1, @"Pending", @"Pending");
    }

    public partial class ChemigationInspectionStatusPass : ChemigationInspectionStatus
    {
        private ChemigationInspectionStatusPass(int chemigationInspectionStatusID, string chemigationInspectionStatusName, string chemigationInspectionStatusDisplayName) : base(chemigationInspectionStatusID, chemigationInspectionStatusName, chemigationInspectionStatusDisplayName) {}
        public static readonly ChemigationInspectionStatusPass Instance = new ChemigationInspectionStatusPass(2, @"Pass", @"Pass");
    }

    public partial class ChemigationInspectionStatusFail : ChemigationInspectionStatus
    {
        private ChemigationInspectionStatusFail(int chemigationInspectionStatusID, string chemigationInspectionStatusName, string chemigationInspectionStatusDisplayName) : base(chemigationInspectionStatusID, chemigationInspectionStatusName, chemigationInspectionStatusDisplayName) {}
        public static readonly ChemigationInspectionStatusFail Instance = new ChemigationInspectionStatusFail(3, @"Fail", @"Fail");
    }
}