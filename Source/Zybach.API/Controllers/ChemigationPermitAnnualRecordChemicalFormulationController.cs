using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            var chemigationPermitAnnualRecordDto = ChemigationPermitAnnualRecord.GetAnnualRecordByPermitNumberAndRecordYear(_dbContext, chemigationPermitNumber, recordYear);
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

        [HttpPost("/api/chemigationPermits/{chemigationPermitNumber}/{recordYear}/chemicalFormulations")]
        [AdminFeature]
        public ActionResult CreateChemigationPermitAnnualRecordChemicalFormulation([FromRoute] int chemigationPermitNumber, [FromRoute] int recordYear,
                [FromBody] List<ChemigationPermitAnnualRecordChemicalFormulationSimpleDto> chemigationPermitAnnualRecordChemicalFormulationsDto)
        {
            var chemigationPermitAnnualRecordDto = ChemigationPermitAnnualRecord.GetAnnualRecordByPermitNumberAndRecordYear(_dbContext, chemigationPermitNumber, recordYear);
            if (chemigationPermitAnnualRecordDto == null)
            {
                var notFoundMessage = $"There is no annual record found for Chemigation Permit #{chemigationPermitNumber}, Year {recordYear}!";
                _logger.LogError(notFoundMessage);
                return NotFound(notFoundMessage);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ChemigationPermitAnnualRecordChemicalFormulations.UpdateChemicalFormulations(_dbContext, chemigationPermitAnnualRecordDto.ChemigationPermitAnnualRecordID, chemigationPermitAnnualRecordChemicalFormulationsDto);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}