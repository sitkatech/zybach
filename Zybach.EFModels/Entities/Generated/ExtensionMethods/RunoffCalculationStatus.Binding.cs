//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[RunoffCalculationStatus]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class RunoffCalculationStatus : IHavePrimaryKey
    {
        public static readonly RunoffCalculationStatusCalculationNotStarted CalculationNotStarted = Zybach.EFModels.Entities.RunoffCalculationStatusCalculationNotStarted.Instance;
        public static readonly RunoffCalculationStatusInProgress InProgress = Zybach.EFModels.Entities.RunoffCalculationStatusInProgress.Instance;
        public static readonly RunoffCalculationStatusSucceeded Succeeded = Zybach.EFModels.Entities.RunoffCalculationStatusSucceeded.Instance;
        public static readonly RunoffCalculationStatusFailed Failed = Zybach.EFModels.Entities.RunoffCalculationStatusFailed.Instance;

        public static readonly List<RunoffCalculationStatus> All;
        public static readonly List<RunoffCalculationStatusDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, RunoffCalculationStatus> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, RunoffCalculationStatusDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static RunoffCalculationStatus()
        {
            All = new List<RunoffCalculationStatus> { CalculationNotStarted, InProgress, Succeeded, Failed };
            AllAsDto = new List<RunoffCalculationStatusDto> { CalculationNotStarted.AsDto(), InProgress.AsDto(), Succeeded.AsDto(), Failed.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, RunoffCalculationStatus>(All.ToDictionary(x => x.RunoffCalculationStatusID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, RunoffCalculationStatusDto>(AllAsDto.ToDictionary(x => x.RunoffCalculationStatusID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected RunoffCalculationStatus(int runoffCalculationStatusID, string runoffCalculationStatusName, string runoffCalculationStatusDisplayName)
        {
            RunoffCalculationStatusID = runoffCalculationStatusID;
            RunoffCalculationStatusName = runoffCalculationStatusName;
            RunoffCalculationStatusDisplayName = runoffCalculationStatusDisplayName;
        }

        [Key]
        public int RunoffCalculationStatusID { get; private set; }
        public string RunoffCalculationStatusName { get; private set; }
        public string RunoffCalculationStatusDisplayName { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return RunoffCalculationStatusID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(RunoffCalculationStatus other)
        {
            if (other == null)
            {
                return false;
            }
            return other.RunoffCalculationStatusID == RunoffCalculationStatusID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as RunoffCalculationStatus);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return RunoffCalculationStatusID;
        }

        public static bool operator ==(RunoffCalculationStatus left, RunoffCalculationStatus right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RunoffCalculationStatus left, RunoffCalculationStatus right)
        {
            return !Equals(left, right);
        }

        public RunoffCalculationStatusEnum ToEnum => (RunoffCalculationStatusEnum)GetHashCode();

        public static RunoffCalculationStatus ToType(int enumValue)
        {
            return ToType((RunoffCalculationStatusEnum)enumValue);
        }

        public static RunoffCalculationStatus ToType(RunoffCalculationStatusEnum enumValue)
        {
            switch (enumValue)
            {
                case RunoffCalculationStatusEnum.CalculationNotStarted:
                    return CalculationNotStarted;
                case RunoffCalculationStatusEnum.Failed:
                    return Failed;
                case RunoffCalculationStatusEnum.InProgress:
                    return InProgress;
                case RunoffCalculationStatusEnum.Succeeded:
                    return Succeeded;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum RunoffCalculationStatusEnum
    {
        CalculationNotStarted = 1,
        InProgress = 2,
        Succeeded = 3,
        Failed = 4
    }

    public partial class RunoffCalculationStatusCalculationNotStarted : RunoffCalculationStatus
    {
        private RunoffCalculationStatusCalculationNotStarted(int runoffCalculationStatusID, string runoffCalculationStatusName, string runoffCalculationStatusDisplayName) : base(runoffCalculationStatusID, runoffCalculationStatusName, runoffCalculationStatusDisplayName) {}
        public static readonly RunoffCalculationStatusCalculationNotStarted Instance = new RunoffCalculationStatusCalculationNotStarted(1, @"CalculationNotStarted", @"Calculation Not Started");
    }

    public partial class RunoffCalculationStatusInProgress : RunoffCalculationStatus
    {
        private RunoffCalculationStatusInProgress(int runoffCalculationStatusID, string runoffCalculationStatusName, string runoffCalculationStatusDisplayName) : base(runoffCalculationStatusID, runoffCalculationStatusName, runoffCalculationStatusDisplayName) {}
        public static readonly RunoffCalculationStatusInProgress Instance = new RunoffCalculationStatusInProgress(2, @"InProgress", @"In Progress");
    }

    public partial class RunoffCalculationStatusSucceeded : RunoffCalculationStatus
    {
        private RunoffCalculationStatusSucceeded(int runoffCalculationStatusID, string runoffCalculationStatusName, string runoffCalculationStatusDisplayName) : base(runoffCalculationStatusID, runoffCalculationStatusName, runoffCalculationStatusDisplayName) {}
        public static readonly RunoffCalculationStatusSucceeded Instance = new RunoffCalculationStatusSucceeded(3, @"Succeeded", @"Succeeded");
    }

    public partial class RunoffCalculationStatusFailed : RunoffCalculationStatus
    {
        private RunoffCalculationStatusFailed(int runoffCalculationStatusID, string runoffCalculationStatusName, string runoffCalculationStatusDisplayName) : base(runoffCalculationStatusID, runoffCalculationStatusName, runoffCalculationStatusDisplayName) {}
        public static readonly RunoffCalculationStatusFailed Instance = new RunoffCalculationStatusFailed(4, @"Failed", @"Failed");
    }
}