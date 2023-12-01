//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[CustomRichTextType]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class CustomRichTextType
    {
        public static readonly CustomRichTextTypePlatformOverview PlatformOverview = Zybach.EFModels.Entities.CustomRichTextTypePlatformOverview.Instance;
        public static readonly CustomRichTextTypeDisclaimer Disclaimer = Zybach.EFModels.Entities.CustomRichTextTypeDisclaimer.Instance;
        public static readonly CustomRichTextTypeHomepage Homepage = Zybach.EFModels.Entities.CustomRichTextTypeHomepage.Instance;
        public static readonly CustomRichTextTypeHelp Help = Zybach.EFModels.Entities.CustomRichTextTypeHelp.Instance;
        public static readonly CustomRichTextTypeLabelsAndDefinitionsList LabelsAndDefinitionsList = Zybach.EFModels.Entities.CustomRichTextTypeLabelsAndDefinitionsList.Instance;
        public static readonly CustomRichTextTypeTraining Training = Zybach.EFModels.Entities.CustomRichTextTypeTraining.Instance;
        public static readonly CustomRichTextTypeRobustReviewScenario RobustReviewScenario = Zybach.EFModels.Entities.CustomRichTextTypeRobustReviewScenario.Instance;
        public static readonly CustomRichTextTypeReportsList ReportsList = Zybach.EFModels.Entities.CustomRichTextTypeReportsList.Instance;
        public static readonly CustomRichTextTypeChemigation Chemigation = Zybach.EFModels.Entities.CustomRichTextTypeChemigation.Instance;
        public static readonly CustomRichTextTypeNDEEChemicalsReport NDEEChemicalsReport = Zybach.EFModels.Entities.CustomRichTextTypeNDEEChemicalsReport.Instance;
        public static readonly CustomRichTextTypeChemigationPermitReport ChemigationPermitReport = Zybach.EFModels.Entities.CustomRichTextTypeChemigationPermitReport.Instance;
        public static readonly CustomRichTextTypeChemigationInspections ChemigationInspections = Zybach.EFModels.Entities.CustomRichTextTypeChemigationInspections.Instance;
        public static readonly CustomRichTextTypeWaterQualityInspections WaterQualityInspections = Zybach.EFModels.Entities.CustomRichTextTypeWaterQualityInspections.Instance;
        public static readonly CustomRichTextTypeWaterLevelInspections WaterLevelInspections = Zybach.EFModels.Entities.CustomRichTextTypeWaterLevelInspections.Instance;
        public static readonly CustomRichTextTypeWellRegistrationIDChangeHelpText WellRegistrationIDChangeHelpText = Zybach.EFModels.Entities.CustomRichTextTypeWellRegistrationIDChangeHelpText.Instance;
        public static readonly CustomRichTextTypeClearinghouseReport ClearinghouseReport = Zybach.EFModels.Entities.CustomRichTextTypeClearinghouseReport.Instance;
        public static readonly CustomRichTextTypeSensorList SensorList = Zybach.EFModels.Entities.CustomRichTextTypeSensorList.Instance;
        public static readonly CustomRichTextTypeWaterQualityReport WaterQualityReport = Zybach.EFModels.Entities.CustomRichTextTypeWaterQualityReport.Instance;
        public static readonly CustomRichTextTypeAnomalyReportList AnomalyReportList = Zybach.EFModels.Entities.CustomRichTextTypeAnomalyReportList.Instance;
        public static readonly CustomRichTextTypeWaterLevelExplorerMap WaterLevelExplorerMap = Zybach.EFModels.Entities.CustomRichTextTypeWaterLevelExplorerMap.Instance;
        public static readonly CustomRichTextTypeWaterLevelExplorerMapDisclaimer WaterLevelExplorerMapDisclaimer = Zybach.EFModels.Entities.CustomRichTextTypeWaterLevelExplorerMapDisclaimer.Instance;
        public static readonly CustomRichTextTypeIrrigationUnitIndex IrrigationUnitIndex = Zybach.EFModels.Entities.CustomRichTextTypeIrrigationUnitIndex.Instance;
        public static readonly CustomRichTextTypeOpenETIntegration OpenETIntegration = Zybach.EFModels.Entities.CustomRichTextTypeOpenETIntegration.Instance;
        public static readonly CustomRichTextTypeSupportTicketIndex SupportTicketIndex = Zybach.EFModels.Entities.CustomRichTextTypeSupportTicketIndex.Instance;
        public static readonly CustomRichTextTypeWellPumpingSummary WellPumpingSummary = Zybach.EFModels.Entities.CustomRichTextTypeWellPumpingSummary.Instance;
        public static readonly CustomRichTextTypeWellGroupList WellGroupList = Zybach.EFModels.Entities.CustomRichTextTypeWellGroupList.Instance;
        public static readonly CustomRichTextTypeWellGroupEdit WellGroupEdit = Zybach.EFModels.Entities.CustomRichTextTypeWellGroupEdit.Instance;
        public static readonly CustomRichTextTypeWaterLevelsReport WaterLevelsReport = Zybach.EFModels.Entities.CustomRichTextTypeWaterLevelsReport.Instance;
        public static readonly CustomRichTextTypeSensorHealthCheck SensorHealthCheck = Zybach.EFModels.Entities.CustomRichTextTypeSensorHealthCheck.Instance;
        public static readonly CustomRichTextTypeSensorStatusMap SensorStatusMap = Zybach.EFModels.Entities.CustomRichTextTypeSensorStatusMap.Instance;

        public static readonly List<CustomRichTextType> All;
        public static readonly List<CustomRichTextTypeDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, CustomRichTextType> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, CustomRichTextTypeDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static CustomRichTextType()
        {
            All = new List<CustomRichTextType> { PlatformOverview, Disclaimer, Homepage, Help, LabelsAndDefinitionsList, Training, RobustReviewScenario, ReportsList, Chemigation, NDEEChemicalsReport, ChemigationPermitReport, ChemigationInspections, WaterQualityInspections, WaterLevelInspections, WellRegistrationIDChangeHelpText, ClearinghouseReport, SensorList, WaterQualityReport, AnomalyReportList, WaterLevelExplorerMap, WaterLevelExplorerMapDisclaimer, IrrigationUnitIndex, OpenETIntegration, SupportTicketIndex, WellPumpingSummary, WellGroupList, WellGroupEdit, WaterLevelsReport, SensorHealthCheck, SensorStatusMap };
            AllAsDto = new List<CustomRichTextTypeDto> { PlatformOverview.AsDto(), Disclaimer.AsDto(), Homepage.AsDto(), Help.AsDto(), LabelsAndDefinitionsList.AsDto(), Training.AsDto(), RobustReviewScenario.AsDto(), ReportsList.AsDto(), Chemigation.AsDto(), NDEEChemicalsReport.AsDto(), ChemigationPermitReport.AsDto(), ChemigationInspections.AsDto(), WaterQualityInspections.AsDto(), WaterLevelInspections.AsDto(), WellRegistrationIDChangeHelpText.AsDto(), ClearinghouseReport.AsDto(), SensorList.AsDto(), WaterQualityReport.AsDto(), AnomalyReportList.AsDto(), WaterLevelExplorerMap.AsDto(), WaterLevelExplorerMapDisclaimer.AsDto(), IrrigationUnitIndex.AsDto(), OpenETIntegration.AsDto(), SupportTicketIndex.AsDto(), WellPumpingSummary.AsDto(), WellGroupList.AsDto(), WellGroupEdit.AsDto(), WaterLevelsReport.AsDto(), SensorHealthCheck.AsDto(), SensorStatusMap.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, CustomRichTextType>(All.ToDictionary(x => x.CustomRichTextTypeID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, CustomRichTextTypeDto>(AllAsDto.ToDictionary(x => x.CustomRichTextTypeID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected CustomRichTextType(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName)
        {
            CustomRichTextTypeID = customRichTextTypeID;
            CustomRichTextTypeName = customRichTextTypeName;
            CustomRichTextTypeDisplayName = customRichTextTypeDisplayName;
        }

        [Key]
        public int CustomRichTextTypeID { get; private set; }
        public string CustomRichTextTypeName { get; private set; }
        public string CustomRichTextTypeDisplayName { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return CustomRichTextTypeID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(CustomRichTextType other)
        {
            if (other == null)
            {
                return false;
            }
            return other.CustomRichTextTypeID == CustomRichTextTypeID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as CustomRichTextType);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return CustomRichTextTypeID;
        }

        public static bool operator ==(CustomRichTextType left, CustomRichTextType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CustomRichTextType left, CustomRichTextType right)
        {
            return !Equals(left, right);
        }

        public CustomRichTextTypeEnum ToEnum => (CustomRichTextTypeEnum)GetHashCode();

        public static CustomRichTextType ToType(int enumValue)
        {
            return ToType((CustomRichTextTypeEnum)enumValue);
        }

        public static CustomRichTextType ToType(CustomRichTextTypeEnum enumValue)
        {
            switch (enumValue)
            {
                case CustomRichTextTypeEnum.AnomalyReportList:
                    return AnomalyReportList;
                case CustomRichTextTypeEnum.Chemigation:
                    return Chemigation;
                case CustomRichTextTypeEnum.ChemigationInspections:
                    return ChemigationInspections;
                case CustomRichTextTypeEnum.ChemigationPermitReport:
                    return ChemigationPermitReport;
                case CustomRichTextTypeEnum.ClearinghouseReport:
                    return ClearinghouseReport;
                case CustomRichTextTypeEnum.Disclaimer:
                    return Disclaimer;
                case CustomRichTextTypeEnum.Help:
                    return Help;
                case CustomRichTextTypeEnum.Homepage:
                    return Homepage;
                case CustomRichTextTypeEnum.IrrigationUnitIndex:
                    return IrrigationUnitIndex;
                case CustomRichTextTypeEnum.LabelsAndDefinitionsList:
                    return LabelsAndDefinitionsList;
                case CustomRichTextTypeEnum.NDEEChemicalsReport:
                    return NDEEChemicalsReport;
                case CustomRichTextTypeEnum.OpenETIntegration:
                    return OpenETIntegration;
                case CustomRichTextTypeEnum.PlatformOverview:
                    return PlatformOverview;
                case CustomRichTextTypeEnum.ReportsList:
                    return ReportsList;
                case CustomRichTextTypeEnum.RobustReviewScenario:
                    return RobustReviewScenario;
                case CustomRichTextTypeEnum.SensorHealthCheck:
                    return SensorHealthCheck;
                case CustomRichTextTypeEnum.SensorList:
                    return SensorList;
                case CustomRichTextTypeEnum.SensorStatusMap:
                    return SensorStatusMap;
                case CustomRichTextTypeEnum.SupportTicketIndex:
                    return SupportTicketIndex;
                case CustomRichTextTypeEnum.Training:
                    return Training;
                case CustomRichTextTypeEnum.WaterLevelExplorerMap:
                    return WaterLevelExplorerMap;
                case CustomRichTextTypeEnum.WaterLevelExplorerMapDisclaimer:
                    return WaterLevelExplorerMapDisclaimer;
                case CustomRichTextTypeEnum.WaterLevelInspections:
                    return WaterLevelInspections;
                case CustomRichTextTypeEnum.WaterLevelsReport:
                    return WaterLevelsReport;
                case CustomRichTextTypeEnum.WaterQualityInspections:
                    return WaterQualityInspections;
                case CustomRichTextTypeEnum.WaterQualityReport:
                    return WaterQualityReport;
                case CustomRichTextTypeEnum.WellGroupEdit:
                    return WellGroupEdit;
                case CustomRichTextTypeEnum.WellGroupList:
                    return WellGroupList;
                case CustomRichTextTypeEnum.WellPumpingSummary:
                    return WellPumpingSummary;
                case CustomRichTextTypeEnum.WellRegistrationIDChangeHelpText:
                    return WellRegistrationIDChangeHelpText;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum CustomRichTextTypeEnum
    {
        PlatformOverview = 1,
        Disclaimer = 2,
        Homepage = 3,
        Help = 4,
        LabelsAndDefinitionsList = 5,
        Training = 6,
        RobustReviewScenario = 7,
        ReportsList = 8,
        Chemigation = 9,
        NDEEChemicalsReport = 10,
        ChemigationPermitReport = 11,
        ChemigationInspections = 12,
        WaterQualityInspections = 13,
        WaterLevelInspections = 14,
        WellRegistrationIDChangeHelpText = 15,
        ClearinghouseReport = 16,
        SensorList = 17,
        WaterQualityReport = 18,
        AnomalyReportList = 19,
        WaterLevelExplorerMap = 20,
        WaterLevelExplorerMapDisclaimer = 21,
        IrrigationUnitIndex = 22,
        OpenETIntegration = 23,
        SupportTicketIndex = 24,
        WellPumpingSummary = 25,
        WellGroupList = 26,
        WellGroupEdit = 27,
        WaterLevelsReport = 28,
        SensorHealthCheck = 29,
        SensorStatusMap = 30
    }

    public partial class CustomRichTextTypePlatformOverview : CustomRichTextType
    {
        private CustomRichTextTypePlatformOverview(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypePlatformOverview Instance = new CustomRichTextTypePlatformOverview(1, @"Platform Overview", @"Platform Overview");
    }

    public partial class CustomRichTextTypeDisclaimer : CustomRichTextType
    {
        private CustomRichTextTypeDisclaimer(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeDisclaimer Instance = new CustomRichTextTypeDisclaimer(2, @"Disclaimer", @"Disclaimer");
    }

    public partial class CustomRichTextTypeHomepage : CustomRichTextType
    {
        private CustomRichTextTypeHomepage(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeHomepage Instance = new CustomRichTextTypeHomepage(3, @"Home page", @"Home page");
    }

    public partial class CustomRichTextTypeHelp : CustomRichTextType
    {
        private CustomRichTextTypeHelp(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeHelp Instance = new CustomRichTextTypeHelp(4, @"Help", @"Help");
    }

    public partial class CustomRichTextTypeLabelsAndDefinitionsList : CustomRichTextType
    {
        private CustomRichTextTypeLabelsAndDefinitionsList(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeLabelsAndDefinitionsList Instance = new CustomRichTextTypeLabelsAndDefinitionsList(5, @"LabelsAndDefinitionsList", @"Labels and Definitions List");
    }

    public partial class CustomRichTextTypeTraining : CustomRichTextType
    {
        private CustomRichTextTypeTraining(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeTraining Instance = new CustomRichTextTypeTraining(6, @"Training", @"Training");
    }

    public partial class CustomRichTextTypeRobustReviewScenario : CustomRichTextType
    {
        private CustomRichTextTypeRobustReviewScenario(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeRobustReviewScenario Instance = new CustomRichTextTypeRobustReviewScenario(7, @"RobustReviewScenario", @"Robust Review Scenario");
    }

    public partial class CustomRichTextTypeReportsList : CustomRichTextType
    {
        private CustomRichTextTypeReportsList(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeReportsList Instance = new CustomRichTextTypeReportsList(8, @"ReportsList", @"Reports List");
    }

    public partial class CustomRichTextTypeChemigation : CustomRichTextType
    {
        private CustomRichTextTypeChemigation(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeChemigation Instance = new CustomRichTextTypeChemigation(9, @"Chemigation", @"Chemigation");
    }

    public partial class CustomRichTextTypeNDEEChemicalsReport : CustomRichTextType
    {
        private CustomRichTextTypeNDEEChemicalsReport(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeNDEEChemicalsReport Instance = new CustomRichTextTypeNDEEChemicalsReport(10, @"NDEEChemicalsReport", @"NDEE Chemicals Report");
    }

    public partial class CustomRichTextTypeChemigationPermitReport : CustomRichTextType
    {
        private CustomRichTextTypeChemigationPermitReport(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeChemigationPermitReport Instance = new CustomRichTextTypeChemigationPermitReport(11, @"ChemigationPermitReport", @"Chemigation Permit Report");
    }

    public partial class CustomRichTextTypeChemigationInspections : CustomRichTextType
    {
        private CustomRichTextTypeChemigationInspections(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeChemigationInspections Instance = new CustomRichTextTypeChemigationInspections(12, @"ChemigationInspections", @"Chemigation Inspections");
    }

    public partial class CustomRichTextTypeWaterQualityInspections : CustomRichTextType
    {
        private CustomRichTextTypeWaterQualityInspections(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeWaterQualityInspections Instance = new CustomRichTextTypeWaterQualityInspections(13, @"WaterQualityInspections", @"Water Quality Inspections");
    }

    public partial class CustomRichTextTypeWaterLevelInspections : CustomRichTextType
    {
        private CustomRichTextTypeWaterLevelInspections(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeWaterLevelInspections Instance = new CustomRichTextTypeWaterLevelInspections(14, @"WaterLevelInspections", @"Water Level Inspections");
    }

    public partial class CustomRichTextTypeWellRegistrationIDChangeHelpText : CustomRichTextType
    {
        private CustomRichTextTypeWellRegistrationIDChangeHelpText(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeWellRegistrationIDChangeHelpText Instance = new CustomRichTextTypeWellRegistrationIDChangeHelpText(15, @"WellRegistrationIDChangeHelpText", @"Well Registration ID Change Help Text");
    }

    public partial class CustomRichTextTypeClearinghouseReport : CustomRichTextType
    {
        private CustomRichTextTypeClearinghouseReport(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeClearinghouseReport Instance = new CustomRichTextTypeClearinghouseReport(16, @"ClearinghouseReport", @"Clearinghouse Report");
    }

    public partial class CustomRichTextTypeSensorList : CustomRichTextType
    {
        private CustomRichTextTypeSensorList(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeSensorList Instance = new CustomRichTextTypeSensorList(17, @"SensorList", @"Sensor List");
    }

    public partial class CustomRichTextTypeWaterQualityReport : CustomRichTextType
    {
        private CustomRichTextTypeWaterQualityReport(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeWaterQualityReport Instance = new CustomRichTextTypeWaterQualityReport(18, @"WaterQualityReport", @"Water Quality Report");
    }

    public partial class CustomRichTextTypeAnomalyReportList : CustomRichTextType
    {
        private CustomRichTextTypeAnomalyReportList(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeAnomalyReportList Instance = new CustomRichTextTypeAnomalyReportList(19, @"AnomalyReportList", @"Anomaly Report List");
    }

    public partial class CustomRichTextTypeWaterLevelExplorerMap : CustomRichTextType
    {
        private CustomRichTextTypeWaterLevelExplorerMap(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeWaterLevelExplorerMap Instance = new CustomRichTextTypeWaterLevelExplorerMap(20, @"WaterLevelExplorerMap", @"Water Level Explorer Map");
    }

    public partial class CustomRichTextTypeWaterLevelExplorerMapDisclaimer : CustomRichTextType
    {
        private CustomRichTextTypeWaterLevelExplorerMapDisclaimer(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeWaterLevelExplorerMapDisclaimer Instance = new CustomRichTextTypeWaterLevelExplorerMapDisclaimer(21, @"WaterLevelExplorerMapDisclaimer", @"Water Level Explorer Map Disclaimer");
    }

    public partial class CustomRichTextTypeIrrigationUnitIndex : CustomRichTextType
    {
        private CustomRichTextTypeIrrigationUnitIndex(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeIrrigationUnitIndex Instance = new CustomRichTextTypeIrrigationUnitIndex(22, @"IrrigationUnitIndex", @"Irrigation Unit Index");
    }

    public partial class CustomRichTextTypeOpenETIntegration : CustomRichTextType
    {
        private CustomRichTextTypeOpenETIntegration(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeOpenETIntegration Instance = new CustomRichTextTypeOpenETIntegration(23, @"OpenETIntegration", @"OpenET Integration");
    }

    public partial class CustomRichTextTypeSupportTicketIndex : CustomRichTextType
    {
        private CustomRichTextTypeSupportTicketIndex(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeSupportTicketIndex Instance = new CustomRichTextTypeSupportTicketIndex(24, @"SupportTicketIndex", @"Support Ticket Index");
    }

    public partial class CustomRichTextTypeWellPumpingSummary : CustomRichTextType
    {
        private CustomRichTextTypeWellPumpingSummary(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeWellPumpingSummary Instance = new CustomRichTextTypeWellPumpingSummary(25, @"WellPumpingSummary", @"Well Pumping Summary");
    }

    public partial class CustomRichTextTypeWellGroupList : CustomRichTextType
    {
        private CustomRichTextTypeWellGroupList(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeWellGroupList Instance = new CustomRichTextTypeWellGroupList(26, @"WellGroupList", @"Well Group List");
    }

    public partial class CustomRichTextTypeWellGroupEdit : CustomRichTextType
    {
        private CustomRichTextTypeWellGroupEdit(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeWellGroupEdit Instance = new CustomRichTextTypeWellGroupEdit(27, @"WellGroupEdit", @"Well Group Edit");
    }

    public partial class CustomRichTextTypeWaterLevelsReport : CustomRichTextType
    {
        private CustomRichTextTypeWaterLevelsReport(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeWaterLevelsReport Instance = new CustomRichTextTypeWaterLevelsReport(28, @"WaterLevelsReport", @"Water Levels Report");
    }

    public partial class CustomRichTextTypeSensorHealthCheck : CustomRichTextType
    {
        private CustomRichTextTypeSensorHealthCheck(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeSensorHealthCheck Instance = new CustomRichTextTypeSensorHealthCheck(29, @"SensorHealthCheck", @"Sensor Health Check");
    }

    public partial class CustomRichTextTypeSensorStatusMap : CustomRichTextType
    {
        private CustomRichTextTypeSensorStatusMap(int customRichTextTypeID, string customRichTextTypeName, string customRichTextTypeDisplayName) : base(customRichTextTypeID, customRichTextTypeName, customRichTextTypeDisplayName) {}
        public static readonly CustomRichTextTypeSensorStatusMap Instance = new CustomRichTextTypeSensorStatusMap(30, @"SensorStatusMap", @"Sensor Status Map");
    }
}