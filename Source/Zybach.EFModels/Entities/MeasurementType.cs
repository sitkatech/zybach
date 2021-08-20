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
            var measurementTypes = dbContext.MeasurementTypes
                .AsNoTracking()
                .Select(x => x.AsDto());

            return measurementTypes;
        }

        public static MeasurementTypeDto GetByMeasurementTypeID(ZybachDbContext dbContext, int measurementTypeID)
        {
            var measurementType = dbContext.MeasurementTypes
                .AsNoTracking()
                .FirstOrDefault(x => x.MeasurementTypeID == measurementTypeID);

            return measurementType?.AsDto();
        }
    }

    public enum MeasurementTypeEnum
    {
        FlowMeter = 1,
        ContinuityMeter = 2,
        ElectricalUsage = 3
    }
}