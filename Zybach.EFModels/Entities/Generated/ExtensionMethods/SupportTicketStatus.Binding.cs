//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[SupportTicketStatus]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class SupportTicketStatus : IHavePrimaryKey
    {
        public static readonly SupportTicketStatusOpen Open = Zybach.EFModels.Entities.SupportTicketStatusOpen.Instance;
        public static readonly SupportTicketStatusInProgress InProgress = Zybach.EFModels.Entities.SupportTicketStatusInProgress.Instance;
        public static readonly SupportTicketStatusResolved Resolved = Zybach.EFModels.Entities.SupportTicketStatusResolved.Instance;
        public static readonly SupportTicketStatusPendingAnomalyReview PendingAnomalyReview = Zybach.EFModels.Entities.SupportTicketStatusPendingAnomalyReview.Instance;

        public static readonly List<SupportTicketStatus> All;
        public static readonly List<SupportTicketStatusDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, SupportTicketStatus> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, SupportTicketStatusDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static SupportTicketStatus()
        {
            All = new List<SupportTicketStatus> { Open, InProgress, Resolved, PendingAnomalyReview };
            AllAsDto = new List<SupportTicketStatusDto> { Open.AsDto(), InProgress.AsDto(), Resolved.AsDto(), PendingAnomalyReview.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, SupportTicketStatus>(All.ToDictionary(x => x.SupportTicketStatusID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, SupportTicketStatusDto>(AllAsDto.ToDictionary(x => x.SupportTicketStatusID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected SupportTicketStatus(int supportTicketStatusID, string supportTicketStatusName, string supportTicketStatusDisplayName, int sortOrder)
        {
            SupportTicketStatusID = supportTicketStatusID;
            SupportTicketStatusName = supportTicketStatusName;
            SupportTicketStatusDisplayName = supportTicketStatusDisplayName;
            SortOrder = sortOrder;
        }

        [Key]
        public int SupportTicketStatusID { get; private set; }
        public string SupportTicketStatusName { get; private set; }
        public string SupportTicketStatusDisplayName { get; private set; }
        public int SortOrder { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return SupportTicketStatusID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(SupportTicketStatus other)
        {
            if (other == null)
            {
                return false;
            }
            return other.SupportTicketStatusID == SupportTicketStatusID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as SupportTicketStatus);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return SupportTicketStatusID;
        }

        public static bool operator ==(SupportTicketStatus left, SupportTicketStatus right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SupportTicketStatus left, SupportTicketStatus right)
        {
            return !Equals(left, right);
        }

        public SupportTicketStatusEnum ToEnum => (SupportTicketStatusEnum)GetHashCode();

        public static SupportTicketStatus ToType(int enumValue)
        {
            return ToType((SupportTicketStatusEnum)enumValue);
        }

        public static SupportTicketStatus ToType(SupportTicketStatusEnum enumValue)
        {
            switch (enumValue)
            {
                case SupportTicketStatusEnum.InProgress:
                    return InProgress;
                case SupportTicketStatusEnum.Open:
                    return Open;
                case SupportTicketStatusEnum.PendingAnomalyReview:
                    return PendingAnomalyReview;
                case SupportTicketStatusEnum.Resolved:
                    return Resolved;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum SupportTicketStatusEnum
    {
        Open = 1,
        InProgress = 2,
        Resolved = 3,
        PendingAnomalyReview = 4
    }

    public partial class SupportTicketStatusOpen : SupportTicketStatus
    {
        private SupportTicketStatusOpen(int supportTicketStatusID, string supportTicketStatusName, string supportTicketStatusDisplayName, int sortOrder) : base(supportTicketStatusID, supportTicketStatusName, supportTicketStatusDisplayName, sortOrder) {}
        public static readonly SupportTicketStatusOpen Instance = new SupportTicketStatusOpen(1, @"Open", @"Open", 10);
    }

    public partial class SupportTicketStatusInProgress : SupportTicketStatus
    {
        private SupportTicketStatusInProgress(int supportTicketStatusID, string supportTicketStatusName, string supportTicketStatusDisplayName, int sortOrder) : base(supportTicketStatusID, supportTicketStatusName, supportTicketStatusDisplayName, sortOrder) {}
        public static readonly SupportTicketStatusInProgress Instance = new SupportTicketStatusInProgress(2, @"InProgress", @"In Progress", 20);
    }

    public partial class SupportTicketStatusResolved : SupportTicketStatus
    {
        private SupportTicketStatusResolved(int supportTicketStatusID, string supportTicketStatusName, string supportTicketStatusDisplayName, int sortOrder) : base(supportTicketStatusID, supportTicketStatusName, supportTicketStatusDisplayName, sortOrder) {}
        public static readonly SupportTicketStatusResolved Instance = new SupportTicketStatusResolved(3, @"Resolved", @"Resolved", 30);
    }

    public partial class SupportTicketStatusPendingAnomalyReview : SupportTicketStatus
    {
        private SupportTicketStatusPendingAnomalyReview(int supportTicketStatusID, string supportTicketStatusName, string supportTicketStatusDisplayName, int sortOrder) : base(supportTicketStatusID, supportTicketStatusName, supportTicketStatusDisplayName, sortOrder) {}
        public static readonly SupportTicketStatusPendingAnomalyReview Instance = new SupportTicketStatusPendingAnomalyReview(4, @"PendingAnomalyReview", @"Pending Anomaly Review", 25);
    }
}