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
        /// <summary>
        /// This is an endpoint in the Zybach API. Aren't it so special?
        /// </summary>
        /// <param name="wellRegistrationID">The Well Registration ID for the requested Well.</param>
        /// <param name="reportingIntervalMinutes">The "reporting interval" in minutes</param>
        /// <param name="startDateISO">The start date for the report, formatted as "yyyyMMdd"</param>
        /// <param name="endDateISO">The start date for the report, formatted as "yyyyMMdd"</param>
        /// <returns>A time series.</returns>
        [HttpGet("/waterReport/pumpedVolume/{wellRegistrationID}")]
        public ActionResult<PumpedVolumeDto> PumpedVolume([FromRoute] string wellRegistrationID,
            [FromQuery] int reportingIntervalMinutes, [FromQuery] string startDateISO, [FromQuery] string endDateISO)
        {
            var startDate = DateTime.ParseExact(startDateISO, "yyyyMMdd", CultureInfo.InvariantCulture).Date;
            var endDate = DateTime.ParseExact(endDateISO, "yyyyMMdd", CultureInfo.InvariantCulture).Date;

            // todo: implement endpoint

            return Ok(new PumpedVolumeDto
            {
                
            });
        }
    }
}
