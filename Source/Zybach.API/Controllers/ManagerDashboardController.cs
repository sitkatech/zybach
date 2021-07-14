using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers
{
    [ApiController]
    public class ManagerDashboardController : SitkaController<ManagerDashboardController>
    {
        public ManagerDashboardController(ZybachDbContext dbContext, ILogger<ManagerDashboardController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }


        [HttpGet("districtStatistics/{year}")]
        public ActionResult<FieldDefinitionDto> GetDistrictStatistics([FromRoute] int year)
        {
            var fieldDefinitionDto = FieldDefinition.GetByFieldDefinitionTypeID(_dbContext, year);
            return RequireNotNullThrowNotFound(fieldDefinitionDto, "FieldDefinition", year);
        }

        [HttpGet("streamFlowZonePumpingDepths")]
        public ActionResult<List<StreamFlowZoneWellsDto>> GetStreamFlowZonePumpingDepths()
        {
            // Currently, we are only accounting for electrical data when color-coding the SFZ map;
            // hence, we can confine our attention to the aghub wells and the electrical estimate time series

            var currentYear = DateTime.Today.Year;
            var years = Enumerable.Range(2019, currentYear - 2019 + 1);

            // Step 1. Get a mapping from SFZs to Wells
            var streamFlowZoneWellMap = StreamFlowZone.ListStreamFlowZonesAndWellsWithinZone(_dbContext);
            return streamFlowZoneWellMap;
            //const results = await Promise.all(years.map(async year => {
            //    // Step 2. Get the total pumped volume for each well.
            //    // This is represented as a mapping from Well Registration IDs to pumped volumes
            //    let pumpedVolumes: Map<string, number> = await this.influxService.getAnnualEstimatedPumpedVolumeByWellForYear(year);

            //// Step 3. For each SFZ, calculate the pumping depth
            //const pumpingDepths: streamFlowZonePumpingDepthDto[] = [];
            //for (const [zone, wells] of streamFlowZoneWellMap) {
            //    if (!wells?.length)
            //    {
            //        pumpingDepths.push({ streamFlowZoneFeatureID: zone.properties.FeatureID, pumpingDepth: 0, totalIrrigatedAcres: 0, totalPumpedVolume: 0 });
            //        continue;
            //    }

            //    // Sum the irrigated acres and pumped volume for each well
            //    const totals = wells.reduce((runningTotals, currentWell) => {
            //        const wellAcres = currentWell.irrigatedAcresPerYear.find(x => x.year === year)?.acres ?? 0
            //        const wellPumpedVolume = pumpedVolumes.get(currentWell.wellRegistrationID) ?? 0;

            //        return {
            //            totalAcres: runningTotals.totalAcres + wellAcres,
            //            totalVolume: runningTotals.totalVolume + wellPumpedVolume
            //        }
            //    }, { totalAcres: 0, totalVolume: 0 })

            //    // todo: this is reporting in gallons/acres right now and we probably want acre-inch per acre
            //    pumpingDepths.push({ streamFlowZoneFeatureID: zone.properties.FeatureID, pumpingDepth: GALLON_TO_ACRE_INCH* totals.totalVolume / totals.totalAcres, totalIrrigatedAcres: totals.totalAcres, totalPumpedVolume: GALLON_TO_ACRE_INCH* totals.totalVolume })
            //}

            //return { year, streamFlowZonePumpingDepths: pumpingDepths };

        }

    }
}