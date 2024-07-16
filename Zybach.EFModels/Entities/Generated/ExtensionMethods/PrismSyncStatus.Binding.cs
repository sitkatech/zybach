//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[PrismSyncStatus]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class PrismSyncStatus : IHavePrimaryKey
    {
        public static readonly PrismSyncStatusSynchronizationNotStarted SynchronizationNotStarted = Zybach.EFModels.Entities.PrismSyncStatusSynchronizationNotStarted.Instance;
        public static readonly PrismSyncStatusInProgress InProgress = Zybach.EFModels.Entities.PrismSyncStatusInProgress.Instance;
        public static readonly PrismSyncStatusSucceeded Succeeded = Zybach.EFModels.Entities.PrismSyncStatusSucceeded.Instance;
        public static readonly PrismSyncStatusFailed Failed = Zybach.EFModels.Entities.PrismSyncStatusFailed.Instance;

        public static readonly List<PrismSyncStatus> All;
        public static readonly List<PrismSyncStatusDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, PrismSyncStatus> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, PrismSyncStatusDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static PrismSyncStatus()
        {
            All = new List<PrismSyncStatus> { SynchronizationNotStarted, InProgress, Succeeded, Failed };
            AllAsDto = new List<PrismSyncStatusDto> { SynchronizationNotStarted.AsDto(), InProgress.AsDto(), Succeeded.AsDto(), Failed.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, PrismSyncStatus>(All.ToDictionary(x => x.PrismSyncStatusID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, PrismSyncStatusDto>(AllAsDto.ToDictionary(x => x.PrismSyncStatusID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected PrismSyncStatus(int prismSyncStatusID, string prismSyncStatusName, string prismSyncStatusDisplayName)
        {
            PrismSyncStatusID = prismSyncStatusID;
            PrismSyncStatusName = prismSyncStatusName;
            PrismSyncStatusDisplayName = prismSyncStatusDisplayName;
        }

        [Key]
        public int PrismSyncStatusID { get; private set; }
        public string PrismSyncStatusName { get; private set; }
        public string PrismSyncStatusDisplayName { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return PrismSyncStatusID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(PrismSyncStatus other)
        {
            if (other == null)
            {
                return false;
            }
            return other.PrismSyncStatusID == PrismSyncStatusID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as PrismSyncStatus);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return PrismSyncStatusID;
        }

        public static bool operator ==(PrismSyncStatus left, PrismSyncStatus right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PrismSyncStatus left, PrismSyncStatus right)
        {
            return !Equals(left, right);
        }

        public PrismSyncStatusEnum ToEnum => (PrismSyncStatusEnum)GetHashCode();

        public static PrismSyncStatus ToType(int enumValue)
        {
            return ToType((PrismSyncStatusEnum)enumValue);
        }

        public static PrismSyncStatus ToType(PrismSyncStatusEnum enumValue)
        {
            switch (enumValue)
            {
                case PrismSyncStatusEnum.Failed:
                    return Failed;
                case PrismSyncStatusEnum.InProgress:
                    return InProgress;
                case PrismSyncStatusEnum.Succeeded:
                    return Succeeded;
                case PrismSyncStatusEnum.SynchronizationNotStarted:
                    return SynchronizationNotStarted;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum PrismSyncStatusEnum
    {
        SynchronizationNotStarted = 1,
        InProgress = 2,
        Succeeded = 3,
        Failed = 4
    }

    public partial class PrismSyncStatusSynchronizationNotStarted : PrismSyncStatus
    {
        private PrismSyncStatusSynchronizationNotStarted(int prismSyncStatusID, string prismSyncStatusName, string prismSyncStatusDisplayName) : base(prismSyncStatusID, prismSyncStatusName, prismSyncStatusDisplayName) {}
        public static readonly PrismSyncStatusSynchronizationNotStarted Instance = new PrismSyncStatusSynchronizationNotStarted(1, @"SynchronizationNotStarted", @"Synchronization Not Started");
    }

    public partial class PrismSyncStatusInProgress : PrismSyncStatus
    {
        private PrismSyncStatusInProgress(int prismSyncStatusID, string prismSyncStatusName, string prismSyncStatusDisplayName) : base(prismSyncStatusID, prismSyncStatusName, prismSyncStatusDisplayName) {}
        public static readonly PrismSyncStatusInProgress Instance = new PrismSyncStatusInProgress(2, @"InProgress", @"In Progress");
    }

    public partial class PrismSyncStatusSucceeded : PrismSyncStatus
    {
        private PrismSyncStatusSucceeded(int prismSyncStatusID, string prismSyncStatusName, string prismSyncStatusDisplayName) : base(prismSyncStatusID, prismSyncStatusName, prismSyncStatusDisplayName) {}
        public static readonly PrismSyncStatusSucceeded Instance = new PrismSyncStatusSucceeded(3, @"Succeeded", @"Succeeded");
    }

    public partial class PrismSyncStatusFailed : PrismSyncStatus
    {
        private PrismSyncStatusFailed(int prismSyncStatusID, string prismSyncStatusName, string prismSyncStatusDisplayName) : base(prismSyncStatusID, prismSyncStatusName, prismSyncStatusDisplayName) {}
        public static readonly PrismSyncStatusFailed Instance = new PrismSyncStatusFailed(4, @"Failed", @"Failed");
    }
}