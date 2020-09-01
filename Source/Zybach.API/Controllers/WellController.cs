using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zybach.Models.DataTransferObjects;
namespace Zybach.API.Controllers
{
    [ApiController]
    public class WellController : ControllerBase
    {
        /// <summary>
        /// Returns a time series representing pumped volume at a well, averaged on a chosen reporting interval, for a given date range.
        /// Each point in the output time series represents the average pumped volume over the previous reporting interval.
        /// </summary>
        /// <param name="wellRegistrationID">The Well Registration ID for the requested Well.</param>
        /// <param name="reportingIntervalMinutes">The reporting interval, in minutes</param>
        /// <param name="startDateISO">The start date for the report, formatted as an ISO date string with a timezone</param>
        /// <param name="endDateISO">The end date for the report, formatted as an ISO date string with a timezone</param>
        /// <returns>A time series representing the average pumped volume for the given date range.</returns>
        /// <response code="200">Returns the requested time series</response>
        /// <response code="404">If the requested Well was not found</response>
        /// <response code="400">If the inputs are improperly-formatted or the date range or reporting interval are invalid. Error message will describe the invalid parameter(s)</response>
        [HttpGet("/well/{wellRegistrationID}/pumpedVolume")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PumpedVolumeDto> PumpedVolume([FromRoute] string wellRegistrationID,
            [FromQuery] int reportingIntervalMinutes, [FromQuery] string startDateISO, [FromQuery] string endDateISO)
        {
            var startDate = DateTime.Parse(startDateISO, null, DateTimeStyles.RoundtripKind).Date;
            var endDate = DateTime.Parse(startDateISO, null, DateTimeStyles.RoundtripKind).Date;

            // todo: implement endpoint

            return Ok(new PumpedVolumeDto
            {
                ReportingIntervalMinutes = reportingIntervalMinutes
            });
        }
        /// <summary>
        /// Returns a dictionary containing key summary statistics for a given well
        /// </summary>
        /// <param name="wellRegistrationID">The Well Registration ID for the requested Well</param>
        /// <returns>A dictionary containing key summary statistics for a given well</returns>
        /// <response code="200">Returns the requested summary statistics</response>
        /// <response code="404">If the requested Well was not found</response>
        [HttpGet("/well/{wellRegistrationID}/summaryStatistics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<WellSummaryStatisticsDto> SummaryStatistics([FromRoute] string wellRegistrationID)
        {
            // todo: implement endpoint
            return Ok(new WellSummaryStatisticsDto());
        }
    }
}
