//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ContinuityMeterStatus]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class ContinuityMeterStatus
    {
        public static readonly ContinuityMeterStatusReportingNormally ReportingNormally = Zybach.EFModels.Entities.ContinuityMeterStatusReportingNormally.Instance;
        public static readonly ContinuityMeterStatusAlwaysOn AlwaysOn = Zybach.EFModels.Entities.ContinuityMeterStatusAlwaysOn.Instance;
        public static readonly ContinuityMeterStatusAlwaysOff AlwaysOff = Zybach.EFModels.Entities.ContinuityMeterStatusAlwaysOff.Instance;

        public static readonly List<ContinuityMeterStatus> All;
        public static readonly List<ContinuityMeterStatusDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, ContinuityMeterStatus> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, ContinuityMeterStatusDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static ContinuityMeterStatus()
        {
            All = new List<ContinuityMeterStatus> { ReportingNormally, AlwaysOn, AlwaysOff };
            AllAsDto = new List<ContinuityMeterStatusDto> { ReportingNormally.AsDto(), AlwaysOn.AsDto(), AlwaysOff.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, ContinuityMeterStatus>(All.ToDictionary(x => x.ContinuityMeterStatusID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, ContinuityMeterStatusDto>(AllAsDto.ToDictionary(x => x.ContinuityMeterStatusID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected ContinuityMeterStatus(int continuityMeterStatusID, string continuityMeterStatusName, string continuityMeterStatusDisplayName)
        {
            ContinuityMeterStatusID = continuityMeterStatusID;
            ContinuityMeterStatusName = continuityMeterStatusName;
            ContinuityMeterStatusDisplayName = continuityMeterStatusDisplayName;
        }

        [Key]
        public int ContinuityMeterStatusID { get; private set; }
        public string ContinuityMeterStatusName { get; private set; }
        public string ContinuityMeterStatusDisplayName { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return ContinuityMeterStatusID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(ContinuityMeterStatus other)
        {
            if (other == null)
            {
                return false;
            }
            return other.ContinuityMeterStatusID == ContinuityMeterStatusID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as ContinuityMeterStatus);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return ContinuityMeterStatusID;
        }

        public static bool operator ==(ContinuityMeterStatus left, ContinuityMeterStatus right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ContinuityMeterStatus left, ContinuityMeterStatus right)
        {
            return !Equals(left, right);
        }

        public ContinuityMeterStatusEnum ToEnum => (ContinuityMeterStatusEnum)GetHashCode();

        public static ContinuityMeterStatus ToType(int enumValue)
        {
            return ToType((ContinuityMeterStatusEnum)enumValue);
        }

        public static ContinuityMeterStatus ToType(ContinuityMeterStatusEnum enumValue)
        {
            switch (enumValue)
            {
                case ContinuityMeterStatusEnum.AlwaysOff:
                    return AlwaysOff;
                case ContinuityMeterStatusEnum.AlwaysOn:
                    return AlwaysOn;
                case ContinuityMeterStatusEnum.ReportingNormally:
                    return ReportingNormally;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum ContinuityMeterStatusEnum
    {
        ReportingNormally = 1,
        AlwaysOn = 2,
        AlwaysOff = 3
    }

    public partial class ContinuityMeterStatusReportingNormally : ContinuityMeterStatus
    {
        private ContinuityMeterStatusReportingNormally(int continuityMeterStatusID, string continuityMeterStatusName, string continuityMeterStatusDisplayName) : base(continuityMeterStatusID, continuityMeterStatusName, continuityMeterStatusDisplayName) {}
        public static readonly ContinuityMeterStatusReportingNormally Instance = new ContinuityMeterStatusReportingNormally(1, @"ReportingNormally", @"Reporting Normally");
    }

    public partial class ContinuityMeterStatusAlwaysOn : ContinuityMeterStatus
    {
        private ContinuityMeterStatusAlwaysOn(int continuityMeterStatusID, string continuityMeterStatusName, string continuityMeterStatusDisplayName) : base(continuityMeterStatusID, continuityMeterStatusName, continuityMeterStatusDisplayName) {}
        public static readonly ContinuityMeterStatusAlwaysOn Instance = new ContinuityMeterStatusAlwaysOn(2, @"AlwaysOn", @"Always On");
    }

    public partial class ContinuityMeterStatusAlwaysOff : ContinuityMeterStatus
    {
        private ContinuityMeterStatusAlwaysOff(int continuityMeterStatusID, string continuityMeterStatusName, string continuityMeterStatusDisplayName) : base(continuityMeterStatusID, continuityMeterStatusName, continuityMeterStatusDisplayName) {}
        public static readonly ContinuityMeterStatusAlwaysOff Instance = new ContinuityMeterStatusAlwaysOff(3, @"AlwaysOff", @"Always Off");
    }
}