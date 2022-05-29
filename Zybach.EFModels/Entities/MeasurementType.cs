using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class MeasurementType
    {
        public static IEnumerable<MeasurementTypeDto> List(ZybachDbContext dbContext)
        {
            return MeasurementType.AllAsDto;
        }

        public static MeasurementTypeDto GetByMeasurementTypeID(ZybachDbContext dbContext, int measurementTypeID)
        {
            return MeasurementType.AllAsDtoLookupDictionary[measurementTypeID];
        }
    }

    /// <summary>
    /// MeasurementTypes and DataSources are synonymous
    /// </summary>
    public struct MeasurementTypes
    {
        public const string ContinuityMeter = "Continuity Meter";
        public const string FlowMeter = "Flow Meter";
        public const string ElectricalUsage = "Electrical Usage";
        public const string WellPressure = "Well Pressure";
    }
}