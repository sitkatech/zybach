using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NodaTime.TimeZones;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers
{
    [ApiController]
    public class ChemigationPermitAnnualRecordChemicalFormulationController : SitkaController<ChemigationPermitAnnualRecordChemicalFormulationController>
    {
        public ChemigationPermitAnnualRecordChemicalFormulationController(ZybachDbContext dbContext, ILogger<ChemigationPermitAnnualRecordChemicalFormulationController> logger,
            KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext,
            logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("/api/chemicalFormulations")]
        [ZybachViewFeature]
        public ActionResult<List<ChemicalFormulationDto>> GetChemicalFormulations()
        {
            var chemicalFormulations = _dbContext.ChemicalFormulations.Select(x => x.AsDto()).ToList();
            return Ok(chemicalFormulations);
        }

        [HttpGet("/api/chemicalUnits")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<ChemicalUnitDto>> GetChemicalUnits()
        {
            var chemicalUnitsDto = _dbContext.ChemicalUnits.Select(x => x.AsDto()).ToList();
            return Ok(chemicalUnitsDto);
        }

        [HttpGet("/api/chemigationPermits/{chemigationPermitNumber}/{recordYear}/chemicalFormulations")]
        [ZybachViewFeature]
        public ActionResult<List<ChemigationPermitAnnualRecordChemicalFormulationSimpleDto>> GetChemigationPermitAnnualRecordChemicalFormulationByPermitNumberAndRecordYear([FromRoute] int chemigationPermitNumber, [FromRoute] int recordYear)
        {
            var chemigationPermitAnnualRecordDto = ChemigationPermitAnnualRecord.GetByPermitNumberAndRecordYearAsDetailedDto(_dbContext, chemigationPermitNumber, recordYear);
            if (chemigationPermitAnnualRecordDto == null)
            {
                var notFoundMessage = $"There is no annual record found for Chemigation Permit #{chemigationPermitNumber}, Year {recordYear}!";
                _logger.LogError(notFoundMessage);
                return NotFound(notFoundMessage);
            }

            var chemigationPermitAnnualRecordChemicalFormulations = _dbContext
                .ChemigationPermitAnnualRecordChemicalFormulations
                .Include(x => x.ChemicalFormulation)
                .Include(x => x.ChemicalUnit)
                .Where(x =>
                    x.ChemigationPermitAnnualRecordID ==
                    chemigationPermitAnnualRecordDto.ChemigationPermitAnnualRecordID).Select(x => x.AsSimpleDto())
                .ToList();

            return Ok(chemigationPermitAnnualRecordChemicalFormulations);
        }

        [HttpGet("/api/chemicalFormulationYearlyTotals")]
        [ZybachViewFeature]
        public ActionResult<List<ChemicalFormulationYearlyTotalDto>> GetChemicalFormulationYearlyTotals()
        {
            var chemigationPermitAnnualRecordChemicalFormulations = _dbContext
                .ChemigationPermitAnnualRecordChemicalFormulations
                .Include(x => x.ChemigationPermitAnnualRecord)
                .ToList();

            var chemicalFormulationYearlyTotals = chemigationPermitAnnualRecordChemicalFormulations
                .GroupBy(x => new
                    { x.ChemigationPermitAnnualRecord.RecordYear, x.ChemicalFormulationID, x.ChemicalUnitID })
                .Select(x => new ChemicalFormulationYearlyTotalDto()
                {
                    RecordYear = x.Key.RecordYear,
                    ChemicalFormulationID = x.Key.ChemicalFormulationID,
                    ChemicalUnitID = x.Key.ChemicalUnitID,
                    TotalApplied = x.Where(z => z.ChemicalFormulationID == x.Key.ChemicalFormulationID &&
                                                z.ChemigationPermitAnnualRecord.RecordYear == x.Key.RecordYear &&
                                                z.ChemicalUnitID == x.Key.ChemicalUnitID).Sum(y => y.TotalApplied),
                    AcresTreated = x.Where(z => z.ChemicalFormulationID == x.Key.ChemicalFormulationID &&
                                                z.ChemigationPermitAnnualRecord.RecordYear == x.Key.RecordYear &&
                                                z.ChemicalUnitID == x.Key.ChemicalUnitID).Sum(y => y.AcresTreated)
                }).ToList();

            return Ok(chemicalFormulationYearlyTotals);
        }
    }
}