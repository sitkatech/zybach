using System.Linq;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities;

public static partial class OpenETSyncExtensionMethods
{
    static partial void DoCustomMappings(OpenETSync openETSync, OpenETSyncDto openETSyncDto)
    {
        openETSyncDto.LastSuccessfulSyncDate = openETSync.OpenETSyncHistories
            .Where(x => x.OpenETSyncResultTypeID == (int)OpenETSyncResultTypeEnum.Succeeded).MaxBy(x => x.CreateDate)
            ?.UpdateDate;

        openETSyncDto.LastSyncDate = openETSync.OpenETSyncHistories.MaxBy(x => x.CreateDate)?.UpdateDate;

        openETSyncDto.LastSyncMessage = openETSync.OpenETSyncHistories.MaxBy(x => x.CreateDate)?.ErrorMessage;

        openETSyncDto.HasInProgressSync = openETSync.OpenETSyncHistories.Any(x => x.OpenETSyncResultTypeID == (int)OpenETSyncResultTypeEnum.InProgress);
    }
}