//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermitAnnualRecordStatus]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class ChemigationPermitAnnualRecordStatus : IHavePrimaryKey
    {
        public static readonly ChemigationPermitAnnualRecordStatusPendingPayment PendingPayment = Zybach.EFModels.Entities.ChemigationPermitAnnualRecordStatusPendingPayment.Instance;
        public static readonly ChemigationPermitAnnualRecordStatusReadyForReview ReadyForReview = Zybach.EFModels.Entities.ChemigationPermitAnnualRecordStatusReadyForReview.Instance;
        public static readonly ChemigationPermitAnnualRecordStatusPendingInspection PendingInspection = Zybach.EFModels.Entities.ChemigationPermitAnnualRecordStatusPendingInspection.Instance;
        public static readonly ChemigationPermitAnnualRecordStatusApproved Approved = Zybach.EFModels.Entities.ChemigationPermitAnnualRecordStatusApproved.Instance;
        public static readonly ChemigationPermitAnnualRecordStatusCanceled Canceled = Zybach.EFModels.Entities.ChemigationPermitAnnualRecordStatusCanceled.Instance;

        public static readonly List<ChemigationPermitAnnualRecordStatus> All;
        public static readonly List<ChemigationPermitAnnualRecordStatusDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, ChemigationPermitAnnualRecordStatus> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, ChemigationPermitAnnualRecordStatusDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static ChemigationPermitAnnualRecordStatus()
        {
            All = new List<ChemigationPermitAnnualRecordStatus> { PendingPayment, ReadyForReview, PendingInspection, Approved, Canceled };
            AllAsDto = new List<ChemigationPermitAnnualRecordStatusDto> { PendingPayment.AsDto(), ReadyForReview.AsDto(), PendingInspection.AsDto(), Approved.AsDto(), Canceled.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, ChemigationPermitAnnualRecordStatus>(All.ToDictionary(x => x.ChemigationPermitAnnualRecordStatusID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, ChemigationPermitAnnualRecordStatusDto>(AllAsDto.ToDictionary(x => x.ChemigationPermitAnnualRecordStatusID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected ChemigationPermitAnnualRecordStatus(int chemigationPermitAnnualRecordStatusID, string chemigationPermitAnnualRecordStatusName, string chemigationPermitAnnualRecordStatusDisplayName)
        {
            ChemigationPermitAnnualRecordStatusID = chemigationPermitAnnualRecordStatusID;
            ChemigationPermitAnnualRecordStatusName = chemigationPermitAnnualRecordStatusName;
            ChemigationPermitAnnualRecordStatusDisplayName = chemigationPermitAnnualRecordStatusDisplayName;
        }

        [Key]
        public int ChemigationPermitAnnualRecordStatusID { get; private set; }
        public string ChemigationPermitAnnualRecordStatusName { get; private set; }
        public string ChemigationPermitAnnualRecordStatusDisplayName { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return ChemigationPermitAnnualRecordStatusID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(ChemigationPermitAnnualRecordStatus other)
        {
            if (other == null)
            {
                return false;
            }
            return other.ChemigationPermitAnnualRecordStatusID == ChemigationPermitAnnualRecordStatusID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as ChemigationPermitAnnualRecordStatus);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return ChemigationPermitAnnualRecordStatusID;
        }

        public static bool operator ==(ChemigationPermitAnnualRecordStatus left, ChemigationPermitAnnualRecordStatus right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ChemigationPermitAnnualRecordStatus left, ChemigationPermitAnnualRecordStatus right)
        {
            return !Equals(left, right);
        }

        public ChemigationPermitAnnualRecordStatusEnum ToEnum => (ChemigationPermitAnnualRecordStatusEnum)GetHashCode();

        public static ChemigationPermitAnnualRecordStatus ToType(int enumValue)
        {
            return ToType((ChemigationPermitAnnualRecordStatusEnum)enumValue);
        }

        public static ChemigationPermitAnnualRecordStatus ToType(ChemigationPermitAnnualRecordStatusEnum enumValue)
        {
            switch (enumValue)
            {
                case ChemigationPermitAnnualRecordStatusEnum.Approved:
                    return Approved;
                case ChemigationPermitAnnualRecordStatusEnum.Canceled:
                    return Canceled;
                case ChemigationPermitAnnualRecordStatusEnum.PendingInspection:
                    return PendingInspection;
                case ChemigationPermitAnnualRecordStatusEnum.PendingPayment:
                    return PendingPayment;
                case ChemigationPermitAnnualRecordStatusEnum.ReadyForReview:
                    return ReadyForReview;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum ChemigationPermitAnnualRecordStatusEnum
    {
        PendingPayment = 1,
        ReadyForReview = 2,
        PendingInspection = 3,
        Approved = 4,
        Canceled = 5
    }

    public partial class ChemigationPermitAnnualRecordStatusPendingPayment : ChemigationPermitAnnualRecordStatus
    {
        private ChemigationPermitAnnualRecordStatusPendingPayment(int chemigationPermitAnnualRecordStatusID, string chemigationPermitAnnualRecordStatusName, string chemigationPermitAnnualRecordStatusDisplayName) : base(chemigationPermitAnnualRecordStatusID, chemigationPermitAnnualRecordStatusName, chemigationPermitAnnualRecordStatusDisplayName) {}
        public static readonly ChemigationPermitAnnualRecordStatusPendingPayment Instance = new ChemigationPermitAnnualRecordStatusPendingPayment(1, @"PendingPayment", @"Pending Payment");
    }

    public partial class ChemigationPermitAnnualRecordStatusReadyForReview : ChemigationPermitAnnualRecordStatus
    {
        private ChemigationPermitAnnualRecordStatusReadyForReview(int chemigationPermitAnnualRecordStatusID, string chemigationPermitAnnualRecordStatusName, string chemigationPermitAnnualRecordStatusDisplayName) : base(chemigationPermitAnnualRecordStatusID, chemigationPermitAnnualRecordStatusName, chemigationPermitAnnualRecordStatusDisplayName) {}
        public static readonly ChemigationPermitAnnualRecordStatusReadyForReview Instance = new ChemigationPermitAnnualRecordStatusReadyForReview(2, @"ReadyForReview", @"Ready For Review");
    }

    public partial class ChemigationPermitAnnualRecordStatusPendingInspection : ChemigationPermitAnnualRecordStatus
    {
        private ChemigationPermitAnnualRecordStatusPendingInspection(int chemigationPermitAnnualRecordStatusID, string chemigationPermitAnnualRecordStatusName, string chemigationPermitAnnualRecordStatusDisplayName) : base(chemigationPermitAnnualRecordStatusID, chemigationPermitAnnualRecordStatusName, chemigationPermitAnnualRecordStatusDisplayName) {}
        public static readonly ChemigationPermitAnnualRecordStatusPendingInspection Instance = new ChemigationPermitAnnualRecordStatusPendingInspection(3, @"PendingInspection", @"Pending Inspection");
    }

    public partial class ChemigationPermitAnnualRecordStatusApproved : ChemigationPermitAnnualRecordStatus
    {
        private ChemigationPermitAnnualRecordStatusApproved(int chemigationPermitAnnualRecordStatusID, string chemigationPermitAnnualRecordStatusName, string chemigationPermitAnnualRecordStatusDisplayName) : base(chemigationPermitAnnualRecordStatusID, chemigationPermitAnnualRecordStatusName, chemigationPermitAnnualRecordStatusDisplayName) {}
        public static readonly ChemigationPermitAnnualRecordStatusApproved Instance = new ChemigationPermitAnnualRecordStatusApproved(4, @"Approved", @"Approved");
    }

    public partial class ChemigationPermitAnnualRecordStatusCanceled : ChemigationPermitAnnualRecordStatus
    {
        private ChemigationPermitAnnualRecordStatusCanceled(int chemigationPermitAnnualRecordStatusID, string chemigationPermitAnnualRecordStatusName, string chemigationPermitAnnualRecordStatusDisplayName) : base(chemigationPermitAnnualRecordStatusID, chemigationPermitAnnualRecordStatusName, chemigationPermitAnnualRecordStatusDisplayName) {}
        public static readonly ChemigationPermitAnnualRecordStatusCanceled Instance = new ChemigationPermitAnnualRecordStatusCanceled(5, @"Canceled", @"Canceled");
    }
}