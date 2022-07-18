//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[SensorType]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class SensorType
    {
        public static readonly SensorTypeFlowMeter FlowMeter = Zybach.EFModels.Entities.SensorTypeFlowMeter.Instance;
        public static readonly SensorTypeContinuityMeter ContinuityMeter = Zybach.EFModels.Entities.SensorTypeContinuityMeter.Instance;
        public static readonly SensorTypeWellPressure WellPressure = Zybach.EFModels.Entities.SensorTypeWellPressure.Instance;
        public static readonly SensorTypeElectricalUsage ElectricalUsage = Zybach.EFModels.Entities.SensorTypeElectricalUsage.Instance;

        public static readonly List<SensorType> All;
        public static readonly List<SensorTypeDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, SensorType> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, SensorTypeDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static SensorType()
        {
            All = new List<SensorType> { FlowMeter, ContinuityMeter, WellPressure, ElectricalUsage };
            AllAsDto = new List<SensorTypeDto> { FlowMeter.AsDto(), ContinuityMeter.AsDto(), WellPressure.AsDto(), ElectricalUsage.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, SensorType>(All.ToDictionary(x => x.SensorTypeID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, SensorTypeDto>(AllAsDto.ToDictionary(x => x.SensorTypeID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected SensorType(int sensorTypeID, string sensorTypeName, string sensorTypeDisplayName, string chartColor, string anomalousChartColor, string yAxisTitle, bool reverseYAxisScale)
        {
            SensorTypeID = sensorTypeID;
            SensorTypeName = sensorTypeName;
            SensorTypeDisplayName = sensorTypeDisplayName;
            ChartColor = chartColor;
            AnomalousChartColor = anomalousChartColor;
            YAxisTitle = yAxisTitle;
            ReverseYAxisScale = reverseYAxisScale;
        }

        [Key]
        public int SensorTypeID { get; private set; }
        public string SensorTypeName { get; private set; }
        public string SensorTypeDisplayName { get; private set; }
        public string ChartColor { get; private set; }
        public string AnomalousChartColor { get; private set; }
        public string YAxisTitle { get; private set; }
        public bool ReverseYAxisScale { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return SensorTypeID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(SensorType other)
        {
            if (other == null)
            {
                return false;
            }
            return other.SensorTypeID == SensorTypeID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as SensorType);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return SensorTypeID;
        }

        public static bool operator ==(SensorType left, SensorType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SensorType left, SensorType right)
        {
            return !Equals(left, right);
        }

        public SensorTypeEnum ToEnum => (SensorTypeEnum)GetHashCode();

        public static SensorType ToType(int enumValue)
        {
            return ToType((SensorTypeEnum)enumValue);
        }

        public static SensorType ToType(SensorTypeEnum enumValue)
        {
            switch (enumValue)
            {
                case SensorTypeEnum.ContinuityMeter:
                    return ContinuityMeter;
                case SensorTypeEnum.ElectricalUsage:
                    return ElectricalUsage;
                case SensorTypeEnum.FlowMeter:
                    return FlowMeter;
                case SensorTypeEnum.WellPressure:
                    return WellPressure;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum SensorTypeEnum
    {
        FlowMeter = 1,
        ContinuityMeter = 2,
        WellPressure = 3,
        ElectricalUsage = 4
    }

    public partial class SensorTypeFlowMeter : SensorType
    {
        private SensorTypeFlowMeter(int sensorTypeID, string sensorTypeName, string sensorTypeDisplayName, string chartColor, string anomalousChartColor, string yAxisTitle, bool reverseYAxisScale) : base(sensorTypeID, sensorTypeName, sensorTypeDisplayName, chartColor, anomalousChartColor, yAxisTitle, reverseYAxisScale) {}
        public static readonly SensorTypeFlowMeter Instance = new SensorTypeFlowMeter(1, @"FlowMeter", @"Flow Meter", @"#42C3EE", @"#D55A6C", @"Gallons", false);
    }

    public partial class SensorTypeContinuityMeter : SensorType
    {
        private SensorTypeContinuityMeter(int sensorTypeID, string sensorTypeName, string sensorTypeDisplayName, string chartColor, string anomalousChartColor, string yAxisTitle, bool reverseYAxisScale) : base(sensorTypeID, sensorTypeName, sensorTypeDisplayName, chartColor, anomalousChartColor, yAxisTitle, reverseYAxisScale) {}
        public static readonly SensorTypeContinuityMeter Instance = new SensorTypeContinuityMeter(2, @"ContinuityMeter", @"Continuity Meter", @"#4AAA42", @"#D55A6C", @"Gallons", false);
    }

    public partial class SensorTypeWellPressure : SensorType
    {
        private SensorTypeWellPressure(int sensorTypeID, string sensorTypeName, string sensorTypeDisplayName, string chartColor, string anomalousChartColor, string yAxisTitle, bool reverseYAxisScale) : base(sensorTypeID, sensorTypeName, sensorTypeDisplayName, chartColor, anomalousChartColor, yAxisTitle, reverseYAxisScale) {}
        public static readonly SensorTypeWellPressure Instance = new SensorTypeWellPressure(3, @"WellPressure", @"Well Pressure", @"#42C3EE", @"#D55A6C", @"Depth to Groundwater (ft)", true);
    }

    public partial class SensorTypeElectricalUsage : SensorType
    {
        private SensorTypeElectricalUsage(int sensorTypeID, string sensorTypeName, string sensorTypeDisplayName, string chartColor, string anomalousChartColor, string yAxisTitle, bool reverseYAxisScale) : base(sensorTypeID, sensorTypeName, sensorTypeDisplayName, chartColor, anomalousChartColor, yAxisTitle, reverseYAxisScale) {}
        public static readonly SensorTypeElectricalUsage Instance = new SensorTypeElectricalUsage(4, @"ElectricalUsage", @"Electrical Usage", @"#0076C0", @"#D55A6C", @"Gallons", true);
    }
}