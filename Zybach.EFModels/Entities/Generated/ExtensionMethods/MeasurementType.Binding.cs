//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[MeasurementType]
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zybach.Models.DataTransferObjects;


namespace Zybach.EFModels.Entities
{
    public abstract partial class MeasurementType
    {
        public static readonly MeasurementTypeFlowMeter FlowMeter = Zybach.EFModels.Entities.MeasurementTypeFlowMeter.Instance;
        public static readonly MeasurementTypeContinuityMeter ContinuityMeter = Zybach.EFModels.Entities.MeasurementTypeContinuityMeter.Instance;
        public static readonly MeasurementTypeElectricalUsage ElectricalUsage = Zybach.EFModels.Entities.MeasurementTypeElectricalUsage.Instance;
        public static readonly MeasurementTypeWellPressure WellPressure = Zybach.EFModels.Entities.MeasurementTypeWellPressure.Instance;
        public static readonly MeasurementTypeBatteryVoltage BatteryVoltage = Zybach.EFModels.Entities.MeasurementTypeBatteryVoltage.Instance;

        public static readonly List<MeasurementType> All;
        public static readonly List<MeasurementTypeDto> AllAsDto;
        public static readonly ReadOnlyDictionary<int, MeasurementType> AllLookupDictionary;
        public static readonly ReadOnlyDictionary<int, MeasurementTypeDto> AllAsDtoLookupDictionary;

        /// <summary>
        /// Static type constructor to coordinate static initialization order
        /// </summary>
        static MeasurementType()
        {
            All = new List<MeasurementType> { FlowMeter, ContinuityMeter, ElectricalUsage, WellPressure, BatteryVoltage };
            AllAsDto = new List<MeasurementTypeDto> { FlowMeter.AsDto(), ContinuityMeter.AsDto(), ElectricalUsage.AsDto(), WellPressure.AsDto(), BatteryVoltage.AsDto() };
            AllLookupDictionary = new ReadOnlyDictionary<int, MeasurementType>(All.ToDictionary(x => x.MeasurementTypeID));
            AllAsDtoLookupDictionary = new ReadOnlyDictionary<int, MeasurementTypeDto>(AllAsDto.ToDictionary(x => x.MeasurementTypeID));
        }

        /// <summary>
        /// Protected constructor only for use in instantiating the set of static lookup values that match database
        /// </summary>
        protected MeasurementType(int measurementTypeID, string measurementTypeName, string measurementTypeDisplayName, string influxMeasurementName, string influxFieldName, string unitsDisplay, string unitsDisplayPlural)
        {
            MeasurementTypeID = measurementTypeID;
            MeasurementTypeName = measurementTypeName;
            MeasurementTypeDisplayName = measurementTypeDisplayName;
            InfluxMeasurementName = influxMeasurementName;
            InfluxFieldName = influxFieldName;
            UnitsDisplay = unitsDisplay;
            UnitsDisplayPlural = unitsDisplayPlural;
        }

        [Key]
        public int MeasurementTypeID { get; private set; }
        public string MeasurementTypeName { get; private set; }
        public string MeasurementTypeDisplayName { get; private set; }
        public string InfluxMeasurementName { get; private set; }
        public string InfluxFieldName { get; private set; }
        public string UnitsDisplay { get; private set; }
        public string UnitsDisplayPlural { get; private set; }
        [NotMapped]
        public int PrimaryKey { get { return MeasurementTypeID; } }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public bool Equals(MeasurementType other)
        {
            if (other == null)
            {
                return false;
            }
            return other.MeasurementTypeID == MeasurementTypeID;
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as MeasurementType);
        }

        /// <summary>
        /// Enum types are equal by primary key
        /// </summary>
        public override int GetHashCode()
        {
            return MeasurementTypeID;
        }

        public static bool operator ==(MeasurementType left, MeasurementType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MeasurementType left, MeasurementType right)
        {
            return !Equals(left, right);
        }

        public MeasurementTypeEnum ToEnum => (MeasurementTypeEnum)GetHashCode();

        public static MeasurementType ToType(int enumValue)
        {
            return ToType((MeasurementTypeEnum)enumValue);
        }

        public static MeasurementType ToType(MeasurementTypeEnum enumValue)
        {
            switch (enumValue)
            {
                case MeasurementTypeEnum.BatteryVoltage:
                    return BatteryVoltage;
                case MeasurementTypeEnum.ContinuityMeter:
                    return ContinuityMeter;
                case MeasurementTypeEnum.ElectricalUsage:
                    return ElectricalUsage;
                case MeasurementTypeEnum.FlowMeter:
                    return FlowMeter;
                case MeasurementTypeEnum.WellPressure:
                    return WellPressure;
                default:
                    throw new ArgumentException("Unable to map Enum: {enumValue}");
            }
        }
    }

    public enum MeasurementTypeEnum
    {
        FlowMeter = 1,
        ContinuityMeter = 2,
        ElectricalUsage = 3,
        WellPressure = 4,
        BatteryVoltage = 5
    }

    public partial class MeasurementTypeFlowMeter : MeasurementType
    {
        private MeasurementTypeFlowMeter(int measurementTypeID, string measurementTypeName, string measurementTypeDisplayName, string influxMeasurementName, string influxFieldName, string unitsDisplay, string unitsDisplayPlural) : base(measurementTypeID, measurementTypeName, measurementTypeDisplayName, influxMeasurementName, influxFieldName, unitsDisplay, unitsDisplayPlural) {}
        public static readonly MeasurementTypeFlowMeter Instance = new MeasurementTypeFlowMeter(1, @"FlowMeter", @"Flow Meter", @"gallons", @"pumped", @"gallon", @"gallons");
    }

    public partial class MeasurementTypeContinuityMeter : MeasurementType
    {
        private MeasurementTypeContinuityMeter(int measurementTypeID, string measurementTypeName, string measurementTypeDisplayName, string influxMeasurementName, string influxFieldName, string unitsDisplay, string unitsDisplayPlural) : base(measurementTypeID, measurementTypeName, measurementTypeDisplayName, influxMeasurementName, influxFieldName, unitsDisplay, unitsDisplayPlural) {}
        public static readonly MeasurementTypeContinuityMeter Instance = new MeasurementTypeContinuityMeter(2, @"ContinuityMeter", @"Continuity Meter", @"continuity", @"on", @"gallon", @"gallons");
    }

    public partial class MeasurementTypeElectricalUsage : MeasurementType
    {
        private MeasurementTypeElectricalUsage(int measurementTypeID, string measurementTypeName, string measurementTypeDisplayName, string influxMeasurementName, string influxFieldName, string unitsDisplay, string unitsDisplayPlural) : base(measurementTypeID, measurementTypeName, measurementTypeDisplayName, influxMeasurementName, influxFieldName, unitsDisplay, unitsDisplayPlural) {}
        public static readonly MeasurementTypeElectricalUsage Instance = new MeasurementTypeElectricalUsage(3, @"ElectricalUsage", @"Electrical Usage", null, null, @"gallon", @"gallons");
    }

    public partial class MeasurementTypeWellPressure : MeasurementType
    {
        private MeasurementTypeWellPressure(int measurementTypeID, string measurementTypeName, string measurementTypeDisplayName, string influxMeasurementName, string influxFieldName, string unitsDisplay, string unitsDisplayPlural) : base(measurementTypeID, measurementTypeName, measurementTypeDisplayName, influxMeasurementName, influxFieldName, unitsDisplay, unitsDisplayPlural) {}
        public static readonly MeasurementTypeWellPressure Instance = new MeasurementTypeWellPressure(4, @"WellPressure", @"Well Pressure", @"depth", @"water-bgl", @"foot", @"feet");
    }

    public partial class MeasurementTypeBatteryVoltage : MeasurementType
    {
        private MeasurementTypeBatteryVoltage(int measurementTypeID, string measurementTypeName, string measurementTypeDisplayName, string influxMeasurementName, string influxFieldName, string unitsDisplay, string unitsDisplayPlural) : base(measurementTypeID, measurementTypeName, measurementTypeDisplayName, influxMeasurementName, influxFieldName, unitsDisplay, unitsDisplayPlural) {}
        public static readonly MeasurementTypeBatteryVoltage Instance = new MeasurementTypeBatteryVoltage(5, @"BatteryVoltage", @"Battery Voltage", @"battery-voltage", @"millivolts", @"millivolt", @"millivolts");
    }
}