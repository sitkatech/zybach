//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[RunoffCalculationStatus]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class RunoffCalculationStatusExtensionMethods
    {
        public static RunoffCalculationStatusDto AsDto(this RunoffCalculationStatus runoffCalculationStatus)
        {
            var runoffCalculationStatusDto = new RunoffCalculationStatusDto()
            {
                RunoffCalculationStatusID = runoffCalculationStatus.RunoffCalculationStatusID,
                RunoffCalculationStatusName = runoffCalculationStatus.RunoffCalculationStatusName,
                RunoffCalculationStatusDisplayName = runoffCalculationStatus.RunoffCalculationStatusDisplayName
            };
            DoCustomMappings(runoffCalculationStatus, runoffCalculationStatusDto);
            return runoffCalculationStatusDto;
        }

        static partial void DoCustomMappings(RunoffCalculationStatus runoffCalculationStatus, RunoffCalculationStatusDto runoffCalculationStatusDto);

        public static RunoffCalculationStatusSimpleDto AsSimpleDto(this RunoffCalculationStatus runoffCalculationStatus)
        {
            var runoffCalculationStatusSimpleDto = new RunoffCalculationStatusSimpleDto()
            {
                RunoffCalculationStatusID = runoffCalculationStatus.RunoffCalculationStatusID,
                RunoffCalculationStatusName = runoffCalculationStatus.RunoffCalculationStatusName,
                RunoffCalculationStatusDisplayName = runoffCalculationStatus.RunoffCalculationStatusDisplayName
            };
            DoCustomSimpleDtoMappings(runoffCalculationStatus, runoffCalculationStatusSimpleDto);
            return runoffCalculationStatusSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(RunoffCalculationStatus runoffCalculationStatus, RunoffCalculationStatusSimpleDto runoffCalculationStatusSimpleDto);
    }
}