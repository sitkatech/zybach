using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities;

public static partial class PrismDailyRecordExtensionMethods
{
    static partial void DoCustomSimpleDtoMappings(PrismDailyRecord prismDailyRecord, PrismDailyRecordSimpleDto prismDailyRecordSimpleDto)
    {
        prismDailyRecordSimpleDto.BlobResourceCanonicalName = prismDailyRecord.BlobResource.BlobResourceCanonicalName;
        prismDailyRecordSimpleDto.BlobFileName = prismDailyRecord.BlobResource.OriginalBaseFilename;
    }
}