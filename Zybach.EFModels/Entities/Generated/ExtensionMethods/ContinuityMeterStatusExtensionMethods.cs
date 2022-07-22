//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ContinuityMeterStatus]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ContinuityMeterStatusExtensionMethods
    {
        public static ContinuityMeterStatusDto AsDto(this ContinuityMeterStatus continuityMeterStatus)
        {
            var continuityMeterStatusDto = new ContinuityMeterStatusDto()
            {
                ContinuityMeterStatusID = continuityMeterStatus.ContinuityMeterStatusID,
                ContinuityMeterStatusName = continuityMeterStatus.ContinuityMeterStatusName,
                ContinuityMeterStatusDisplayName = continuityMeterStatus.ContinuityMeterStatusDisplayName
            };
            DoCustomMappings(continuityMeterStatus, continuityMeterStatusDto);
            return continuityMeterStatusDto;
        }

        static partial void DoCustomMappings(ContinuityMeterStatus continuityMeterStatus, ContinuityMeterStatusDto continuityMeterStatusDto);

        public static ContinuityMeterStatusSimpleDto AsSimpleDto(this ContinuityMeterStatus continuityMeterStatus)
        {
            var continuityMeterStatusSimpleDto = new ContinuityMeterStatusSimpleDto()
            {
                ContinuityMeterStatusID = continuityMeterStatus.ContinuityMeterStatusID,
                ContinuityMeterStatusName = continuityMeterStatus.ContinuityMeterStatusName,
                ContinuityMeterStatusDisplayName = continuityMeterStatus.ContinuityMeterStatusDisplayName
            };
            DoCustomSimpleDtoMappings(continuityMeterStatus, continuityMeterStatusSimpleDto);
            return continuityMeterStatusSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(ContinuityMeterStatus continuityMeterStatus, ContinuityMeterStatusSimpleDto continuityMeterStatusSimpleDto);
    }
}