using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zybach.Models.DataTransferObjects;
namespace Zybach.API.Controllers
{
    [ApiController]
    public class WaterReportController : ControllerBase
    {
        [HttpGet("/waterReport/pumpedVolume/{wellRegistrationID}")]
        public ActionResult<PumpedVolumeDto> PumpedVolume([FromRoute] string wellRegistrationID,
            [FromQuery] int reportingIntervalMinutes, [FromQuery] string startDateISO, [FromQuery] string endDateISO)
        {
            var startDate = DateTime.ParseExact(startDateISO, "yyyyMMdd", CultureInfo.InvariantCulture).Date;
            
            var endDate = DateTime.ParseExact(endDateISO, "yyyyMMdd", CultureInfo.InvariantCulture).Date;
            return Ok(new PumpedVolumeDto
            {
                WellRegistrationID = wellRegistrationID,
                StartDate = startDate,
                EndDate = endDate,
                ReportingIntervalMinutes = reportingIntervalMinutes
            });
        }
    }
}
