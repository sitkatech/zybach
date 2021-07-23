using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers
{
    [ApiController]
    public class ChemigationInspectionController : SitkaController<ChemigationInspectionController>
    {
        public ChemigationInspectionController(ZybachDbContext dbContext, ILogger<ChemigationInspectionController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }


        [HttpGet("/api/chemigation/summaries")]
        public List<ChemigationInspectionDto> GetChemigationInspections()
        {
            var chemigationInspectionDtos = ChemigationInspection.List(_dbContext);
            return chemigationInspectionDtos;
        }
    }
}