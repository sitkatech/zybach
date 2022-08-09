using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class AgHubWellIrrigatedAcreExtensionMethods
    {

        public static IrrigatedAcresPerYearDto AsIrrigatedAcresPerYearDto(this AgHubWellIrrigatedAcre agHubWellIrrigatedAcre)
        {
            var irrigatedAcresPerYearDto = new IrrigatedAcresPerYearDto()
            {
                Year = agHubWellIrrigatedAcre.IrrigationYear,
                Acres = agHubWellIrrigatedAcre.Acres
                // TODO: Populate CropType when Mark updates AgHub API
                // CropType =
            };
            
            return irrigatedAcresPerYearDto;
        }

    }
}
