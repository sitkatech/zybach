//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[FieldDefinitionType]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class FieldDefinitionType : IHavePrimaryKey
    {
        public static readonly FieldDefinitionTypeName Name = Zybach.EFModels.Entities.FieldDefinitionTypeName.Instance;
        public static readonly FieldDefinitionTypeHasWaterLevelInspections HasWaterLevelInspections = Zybach.EFModels.Entities.FieldDefinitionTypeHasWaterLevelInspections.Instance;
        public static readonly FieldDefinitionTypeHasWaterQualityInspections HasWaterQualityInspections = Zybach.EFModels.Entities.FieldDefinitionTypeHasWaterQualityInspections.Instance;
        public static readonly FieldDefinitionTypeLatestWaterLevelInspectionDate LatestWaterLevelInspectionDate = Zybach.EFModels.Entities.FieldDefinitionTypeLatestWaterLevelInspectionDate.Instance;
        public static readonly FieldDefinitionTypeLatestWaterQualityInspectionDate LatestWaterQualityInspectionDate = Zybach.EFModels.Entities.FieldDefinitionTypeLatestWaterQualityInspectionDate.Instance;
        public static readonly FieldDefinitionTypeWellRegistrationNumber WellRegistrationNumber = Zybach.EFModels.Entities.FieldDefinitionTypeWellRegistrationNumber.Instance;
        public static readonly FieldDefinitionTypeWellNickname WellNickname = Zybach.EFModels.Entities.FieldDefinitionTypeWellNickname.Instance;
        public static readonly FieldDefinitionTypeAgHubRegisteredUser AgHubRegisteredUser = Zybach.EFModels.Entities.FieldDefinitionTypeAgHubRegisteredUser.Instance;
        public static readonly FieldDefinitionTypeWellFieldName WellFieldName = Zybach.EFModels.Entities.FieldDefinitionTypeWellFieldName.Instance;
        public static readonly FieldDefinitionTypeIrrigationUnitID IrrigationUnitID = Zybach.EFModels.Entities.FieldDefinitionTypeIrrigationUnitID.Instance;
        public static readonly FieldDefinitionTypeWellIrrigatedAcres WellIrrigatedAcres = Zybach.EFModels.Entities.FieldDefinitionTypeWellIrrigatedAcres.Instance;
        public static readonly FieldDefinitionTypeWellChemigationInspectionParticipation WellChemigationInspectionParticipation = Zybach.EFModels.Entities.FieldDefinitionTypeWellChemigationInspectionParticipation.Instance;
        public static readonly FieldDefinitionTypeWellWaterLevelInspectionParticipation WellWaterLevelInspectionParticipation = Zybach.EFModels.Entities.FieldDefinitionTypeWellWaterLevelInspectionParticipation.Instance;
        public static readonly FieldDefinitionTypeWellWaterQualityInspectionParticipation WellWaterQualityInspectionParticipation = Zybach.EFModels.Entities.FieldDefinitionTypeWellWaterQualityInspectionParticipation.Instance;
        public static readonly FieldDefinitionTypeWellProgramParticipation WellProgramParticipation = Zybach.EFModels.Entities.FieldDefinitionTypeWellProgramParticipation.Instance;
        public static readonly FieldDefinitionTypeWellOwnerName WellOwnerName = Zybach.EFModels.Entities.FieldDefinitionTypeWellOwnerName.Instance;
        public static readonly FieldDefinitionTypeSensorLastMessageAgeHours SensorLastMessageAgeHours = Zybach.EFModels.Entities.FieldDefinitionTypeSensorLastMessageAgeHours.Instance;
        public static readonly FieldDefinitionTypeSensorLastVoltageReading SensorLastVoltageReading = Zybach.EFModels.Entities.FieldDefinitionTypeSensorLastVoltageReading.Instance;
        public static readonly FieldDefinitionTypeSensorFirstReadingDate SensorFirstReadingDate = Zybach.EFModels.Entities.FieldDefinitionTypeSensorFirstReadingDate.Instance;
        public static readonly FieldDefinitionTypeSensorLastReadingDate SensorLastReadingDate = Zybach.EFModels.Entities.FieldDefinitionTypeSensorLastReadingDate.Instance;
        public static readonly FieldDefinitionTypeSensorStatus SensorStatus = Zybach.EFModels.Entities.FieldDefinitionTypeSensorStatus.Instance;
        public static readonly FieldDefinitionTypeSensorType SensorType = Zybach.EFModels.Entities.FieldDefinitionTypeSensorType.Instance;
        public static readonly FieldDefinitionTypeIrrigationUnitAcres IrrigationUnitAcres = Zybach.EFModels.Entities.FieldDefinitionTypeIrrigationUnitAcres.Instance;
        public static readonly FieldDefinitionTypeSensorLastVoltageReadingDate SensorLastVoltageReadingDate = Zybach.EFModels.Entities.FieldDefinitionTypeSensorLastVoltageReadingDate.Instance;
        public static readonly FieldDefinitionTypeSensorRetirementDate SensorRetirementDate = Zybach.EFModels.Entities.FieldDefinitionTypeSensorRetirementDate.Instance;
        public static readonly FieldDefinitionTypeContinuityMeterStatus ContinuityMeterStatus = Zybach.EFModels.Entities.FieldDefinitionTypeContinuityMeterStatus.Instance;
        public static readonly FieldDefinitionTypeActiveSupportTicket ActiveSupportTicket = Zybach.EFModels.Entities.FieldDefinitionTypeActiveSupportTicket.Instance;

        public static readonly List<FieldDefinitionType> All;
        public static readonly List<FieldDefinitionTypeDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, FieldDefinitionType> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, FieldDefinitionTypeDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static FieldDefinitionType()
        {
            All = new List<FieldDefinitionType> { Name, HasWaterLevelInspections, HasWaterQualityInspections, LatestWaterLevelInspectionDate, LatestWaterQualityInspectionDate, WellRegistrationNumber, WellNickname, AgHubRegisteredUser, WellFieldName, IrrigationUnitID, WellIrrigatedAcres, WellChemigationInspectionParticipation, WellWaterLevelInspectionParticipation, WellWaterQualityInspectionParticipation, WellProgramParticipation, WellOwnerName, SensorLastMessageAgeHours, SensorLastVoltageReading, SensorFirstReadingDate, SensorLastReadingDate, SensorStatus, SensorType, IrrigationUnitAcres, SensorLastVoltageReadingDate, SensorRetirementDate, ContinuityMeterStatus, ActiveSupportTicket };
            AllAsDto = new List<FieldDefinitionTypeDto> { Name.AsDto(), HasWaterLevelInspections.AsDto(), HasWaterQualityInspections.AsDto(), LatestWaterLevelInspectionDate.AsDto(), LatestWaterQualityInspectionDate.AsDto(), WellRegistrationNumber.AsDto(), WellNickname.AsDto(), AgHubRegisteredUser.AsDto(), WellFieldName.AsDto(), IrrigationUnitID.AsDto(), WellIrrigatedAcres.AsDto(), WellChemigationInspectionParticipation.AsDto(), WellWaterLevelInspectionParticipation.AsDto(), WellWaterQualityInspectionParticipation.AsDto(), WellProgramParticipation.AsDto(), WellOwnerName.AsDto(), SensorLastMessageAgeHours.AsDto(), SensorLastVoltageReading.AsDto(), SensorFirstReadingDate.AsDto(), SensorLastReadingDate.AsDto(), SensorStatus.AsDto(), SensorType.AsDto(), IrrigationUnitAcres.AsDto(), SensorLastVoltageReadingDate.AsDto(), SensorRetirementDate.AsDto(), ContinuityMeterStatus.AsDto(), ActiveSupportTicket.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, FieldDefinitionType>(All.ToDictionary(x => x.FieldDefinitionTypeID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, FieldDefinitionTypeDto>(AllAsDto.ToDictionary(x => x.FieldDefinitionTypeID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected FieldDefinitionType(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName)
        {
            FieldDefinitionTypeID = fieldDefinitionTypeID;
            FieldDefinitionTypeName = fieldDefinitionTypeName;
            FieldDefinitionTypeDisplayName = fieldDefinitionTypeDisplayName;
        }

        [Key]
        public int FieldDefinitionTypeID { get; private set; }
        public string FieldDefinitionTypeName { get; private set; }
        public string FieldDefinitionTypeDisplayName { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return FieldDefinitionTypeID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(FieldDefinitionType other)
        {
            if (other == null)
            {
                return false;
            }
            return other.FieldDefinitionTypeID == FieldDefinitionTypeID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as FieldDefinitionType);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return FieldDefinitionTypeID;
        }

        public static bool operator ==(FieldDefinitionType left, FieldDefinitionType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FieldDefinitionType left, FieldDefinitionType right)
        {
            return !Equals(left, right);
        }

        public FieldDefinitionTypeEnum ToEnum => (FieldDefinitionTypeEnum)GetHashCode();

        public static FieldDefinitionType ToType(int enumValue)
        {
            return ToType((FieldDefinitionTypeEnum)enumValue);
        }

        public static FieldDefinitionType ToType(FieldDefinitionTypeEnum enumValue)
        {
            switch (enumValue)
            {
                case FieldDefinitionTypeEnum.ActiveSupportTicket:
                    return ActiveSupportTicket;
                case FieldDefinitionTypeEnum.AgHubRegisteredUser:
                    return AgHubRegisteredUser;
                case FieldDefinitionTypeEnum.ContinuityMeterStatus:
                    return ContinuityMeterStatus;
                case FieldDefinitionTypeEnum.HasWaterLevelInspections:
                    return HasWaterLevelInspections;
                case FieldDefinitionTypeEnum.HasWaterQualityInspections:
                    return HasWaterQualityInspections;
                case FieldDefinitionTypeEnum.IrrigationUnitAcres:
                    return IrrigationUnitAcres;
                case FieldDefinitionTypeEnum.IrrigationUnitID:
                    return IrrigationUnitID;
                case FieldDefinitionTypeEnum.LatestWaterLevelInspectionDate:
                    return LatestWaterLevelInspectionDate;
                case FieldDefinitionTypeEnum.LatestWaterQualityInspectionDate:
                    return LatestWaterQualityInspectionDate;
                case FieldDefinitionTypeEnum.Name:
                    return Name;
                case FieldDefinitionTypeEnum.SensorFirstReadingDate:
                    return SensorFirstReadingDate;
                case FieldDefinitionTypeEnum.SensorLastMessageAgeHours:
                    return SensorLastMessageAgeHours;
                case FieldDefinitionTypeEnum.SensorLastReadingDate:
                    return SensorLastReadingDate;
                case FieldDefinitionTypeEnum.SensorLastVoltageReading:
                    return SensorLastVoltageReading;
                case FieldDefinitionTypeEnum.SensorLastVoltageReadingDate:
                    return SensorLastVoltageReadingDate;
                case FieldDefinitionTypeEnum.SensorRetirementDate:
                    return SensorRetirementDate;
                case FieldDefinitionTypeEnum.SensorStatus:
                    return SensorStatus;
                case FieldDefinitionTypeEnum.SensorType:
                    return SensorType;
                case FieldDefinitionTypeEnum.WellChemigationInspectionParticipation:
                    return WellChemigationInspectionParticipation;
                case FieldDefinitionTypeEnum.WellFieldName:
                    return WellFieldName;
                case FieldDefinitionTypeEnum.WellIrrigatedAcres:
                    return WellIrrigatedAcres;
                case FieldDefinitionTypeEnum.WellNickname:
                    return WellNickname;
                case FieldDefinitionTypeEnum.WellOwnerName:
                    return WellOwnerName;
                case FieldDefinitionTypeEnum.WellProgramParticipation:
                    return WellProgramParticipation;
                case FieldDefinitionTypeEnum.WellRegistrationNumber:
                    return WellRegistrationNumber;
                case FieldDefinitionTypeEnum.WellWaterLevelInspectionParticipation:
                    return WellWaterLevelInspectionParticipation;
                case FieldDefinitionTypeEnum.WellWaterQualityInspectionParticipation:
                    return WellWaterQualityInspectionParticipation;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum FieldDefinitionTypeEnum
    {
        Name = 1,
        HasWaterLevelInspections = 2,
        HasWaterQualityInspections = 3,
        LatestWaterLevelInspectionDate = 4,
        LatestWaterQualityInspectionDate = 5,
        WellRegistrationNumber = 6,
        WellNickname = 7,
        AgHubRegisteredUser = 8,
        WellFieldName = 9,
        IrrigationUnitID = 10,
        WellIrrigatedAcres = 11,
        WellChemigationInspectionParticipation = 12,
        WellWaterLevelInspectionParticipation = 13,
        WellWaterQualityInspectionParticipation = 14,
        WellProgramParticipation = 15,
        WellOwnerName = 16,
        SensorLastMessageAgeHours = 17,
        SensorLastVoltageReading = 18,
        SensorFirstReadingDate = 19,
        SensorLastReadingDate = 20,
        SensorStatus = 21,
        SensorType = 22,
        IrrigationUnitAcres = 23,
        SensorLastVoltageReadingDate = 24,
        SensorRetirementDate = 25,
        ContinuityMeterStatus = 26,
        ActiveSupportTicket = 27
    }

    public partial class FieldDefinitionTypeName : FieldDefinitionType
    {
        private FieldDefinitionTypeName(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeName Instance = new FieldDefinitionTypeName(1, @"Name", @"Name");
    }

    public partial class FieldDefinitionTypeHasWaterLevelInspections : FieldDefinitionType
    {
        private FieldDefinitionTypeHasWaterLevelInspections(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeHasWaterLevelInspections Instance = new FieldDefinitionTypeHasWaterLevelInspections(2, @"HasWaterLevelInspections", @"Has Water Level Inspections?");
    }

    public partial class FieldDefinitionTypeHasWaterQualityInspections : FieldDefinitionType
    {
        private FieldDefinitionTypeHasWaterQualityInspections(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeHasWaterQualityInspections Instance = new FieldDefinitionTypeHasWaterQualityInspections(3, @"HasWaterQualityInspections", @"Has Water Quality Inspections?");
    }

    public partial class FieldDefinitionTypeLatestWaterLevelInspectionDate : FieldDefinitionType
    {
        private FieldDefinitionTypeLatestWaterLevelInspectionDate(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeLatestWaterLevelInspectionDate Instance = new FieldDefinitionTypeLatestWaterLevelInspectionDate(4, @"LatestWaterLevelInspectionDate", @"Latest Water Level Inspection Date");
    }

    public partial class FieldDefinitionTypeLatestWaterQualityInspectionDate : FieldDefinitionType
    {
        private FieldDefinitionTypeLatestWaterQualityInspectionDate(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeLatestWaterQualityInspectionDate Instance = new FieldDefinitionTypeLatestWaterQualityInspectionDate(5, @"LatestWaterQualityInspectionDate", @"Latest Water Quality Inspection Date");
    }

    public partial class FieldDefinitionTypeWellRegistrationNumber : FieldDefinitionType
    {
        private FieldDefinitionTypeWellRegistrationNumber(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeWellRegistrationNumber Instance = new FieldDefinitionTypeWellRegistrationNumber(6, @"WellRegistrationNumber", @"Well Registration #");
    }

    public partial class FieldDefinitionTypeWellNickname : FieldDefinitionType
    {
        private FieldDefinitionTypeWellNickname(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeWellNickname Instance = new FieldDefinitionTypeWellNickname(7, @"WellNickname", @"Well Nickname");
    }

    public partial class FieldDefinitionTypeAgHubRegisteredUser : FieldDefinitionType
    {
        private FieldDefinitionTypeAgHubRegisteredUser(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeAgHubRegisteredUser Instance = new FieldDefinitionTypeAgHubRegisteredUser(8, @"AgHubRegisteredUser", @"AgHub Registered User");
    }

    public partial class FieldDefinitionTypeWellFieldName : FieldDefinitionType
    {
        private FieldDefinitionTypeWellFieldName(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeWellFieldName Instance = new FieldDefinitionTypeWellFieldName(9, @"WellFieldName", @"Field Name");
    }

    public partial class FieldDefinitionTypeIrrigationUnitID : FieldDefinitionType
    {
        private FieldDefinitionTypeIrrigationUnitID(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeIrrigationUnitID Instance = new FieldDefinitionTypeIrrigationUnitID(10, @"IrrigationUnitID", @"Irrigation Unit ID");
    }

    public partial class FieldDefinitionTypeWellIrrigatedAcres : FieldDefinitionType
    {
        private FieldDefinitionTypeWellIrrigatedAcres(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeWellIrrigatedAcres Instance = new FieldDefinitionTypeWellIrrigatedAcres(11, @"WellIrrigatedAcres", @"Irrigated Acres");
    }

    public partial class FieldDefinitionTypeWellChemigationInspectionParticipation : FieldDefinitionType
    {
        private FieldDefinitionTypeWellChemigationInspectionParticipation(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeWellChemigationInspectionParticipation Instance = new FieldDefinitionTypeWellChemigationInspectionParticipation(12, @"WellChemigationInspectionParticipation", @"Requires Chemigation Inspections?");
    }

    public partial class FieldDefinitionTypeWellWaterLevelInspectionParticipation : FieldDefinitionType
    {
        private FieldDefinitionTypeWellWaterLevelInspectionParticipation(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeWellWaterLevelInspectionParticipation Instance = new FieldDefinitionTypeWellWaterLevelInspectionParticipation(13, @"WellWaterLevelInspectionParticipation", @"Requires Water Level Inspections?");
    }

    public partial class FieldDefinitionTypeWellWaterQualityInspectionParticipation : FieldDefinitionType
    {
        private FieldDefinitionTypeWellWaterQualityInspectionParticipation(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeWellWaterQualityInspectionParticipation Instance = new FieldDefinitionTypeWellWaterQualityInspectionParticipation(14, @"WellWaterQualityInspectionParticipation", @"Water Quality Inspection Type");
    }

    public partial class FieldDefinitionTypeWellProgramParticipation : FieldDefinitionType
    {
        private FieldDefinitionTypeWellProgramParticipation(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeWellProgramParticipation Instance = new FieldDefinitionTypeWellProgramParticipation(15, @"WellProgramParticipation", @"Well Participation");
    }

    public partial class FieldDefinitionTypeWellOwnerName : FieldDefinitionType
    {
        private FieldDefinitionTypeWellOwnerName(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeWellOwnerName Instance = new FieldDefinitionTypeWellOwnerName(16, @"WellOwnerName", @"Owner");
    }

    public partial class FieldDefinitionTypeSensorLastMessageAgeHours : FieldDefinitionType
    {
        private FieldDefinitionTypeSensorLastMessageAgeHours(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeSensorLastMessageAgeHours Instance = new FieldDefinitionTypeSensorLastMessageAgeHours(17, @"SensorLastMessageAgeHours", @"Last Message Age (Hours)");
    }

    public partial class FieldDefinitionTypeSensorLastVoltageReading : FieldDefinitionType
    {
        private FieldDefinitionTypeSensorLastVoltageReading(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeSensorLastVoltageReading Instance = new FieldDefinitionTypeSensorLastVoltageReading(18, @"SensorLastVoltageReading", @"Last Voltage Reading (mV)");
    }

    public partial class FieldDefinitionTypeSensorFirstReadingDate : FieldDefinitionType
    {
        private FieldDefinitionTypeSensorFirstReadingDate(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeSensorFirstReadingDate Instance = new FieldDefinitionTypeSensorFirstReadingDate(19, @"SensorFirstReadingDate", @"First Measurement Date");
    }

    public partial class FieldDefinitionTypeSensorLastReadingDate : FieldDefinitionType
    {
        private FieldDefinitionTypeSensorLastReadingDate(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeSensorLastReadingDate Instance = new FieldDefinitionTypeSensorLastReadingDate(20, @"SensorLastReadingDate", @"Last Measurement Date");
    }

    public partial class FieldDefinitionTypeSensorStatus : FieldDefinitionType
    {
        private FieldDefinitionTypeSensorStatus(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeSensorStatus Instance = new FieldDefinitionTypeSensorStatus(21, @"SensorStatus", @"Status");
    }

    public partial class FieldDefinitionTypeSensorType : FieldDefinitionType
    {
        private FieldDefinitionTypeSensorType(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeSensorType Instance = new FieldDefinitionTypeSensorType(22, @"SensorType", @"Sensor Type");
    }

    public partial class FieldDefinitionTypeIrrigationUnitAcres : FieldDefinitionType
    {
        private FieldDefinitionTypeIrrigationUnitAcres(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeIrrigationUnitAcres Instance = new FieldDefinitionTypeIrrigationUnitAcres(23, @"IrrigationUnitAcres", @"Irrigation Unit Area (ac)");
    }

    public partial class FieldDefinitionTypeSensorLastVoltageReadingDate : FieldDefinitionType
    {
        private FieldDefinitionTypeSensorLastVoltageReadingDate(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeSensorLastVoltageReadingDate Instance = new FieldDefinitionTypeSensorLastVoltageReadingDate(24, @"SensorLastVoltageReadingDate", @"Last Voltage Reading Date");
    }

    public partial class FieldDefinitionTypeSensorRetirementDate : FieldDefinitionType
    {
        private FieldDefinitionTypeSensorRetirementDate(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeSensorRetirementDate Instance = new FieldDefinitionTypeSensorRetirementDate(25, @"SensorRetirementDate", @"Sensor Retirement Date");
    }

    public partial class FieldDefinitionTypeContinuityMeterStatus : FieldDefinitionType
    {
        private FieldDefinitionTypeContinuityMeterStatus(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeContinuityMeterStatus Instance = new FieldDefinitionTypeContinuityMeterStatus(26, @"ContinuityMeterStatus", @"Continuity Meter Always On/Off");
    }

    public partial class FieldDefinitionTypeActiveSupportTicket : FieldDefinitionType
    {
        private FieldDefinitionTypeActiveSupportTicket(int fieldDefinitionTypeID, string fieldDefinitionTypeName, string fieldDefinitionTypeDisplayName) : base(fieldDefinitionTypeID, fieldDefinitionTypeName, fieldDefinitionTypeDisplayName) {}
        public static readonly FieldDefinitionTypeActiveSupportTicket Instance = new FieldDefinitionTypeActiveSupportTicket(27, @"ActiveSupportTicket", @"Active Support Ticket");
    }
}