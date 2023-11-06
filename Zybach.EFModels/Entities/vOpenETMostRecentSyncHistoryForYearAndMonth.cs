using System.ComponentModel.DataAnnotations.Schema;

namespace Zybach.EFModels.Entities
{
    public partial class vOpenETMostRecentSyncHistoryForYearAndMonth
    {
        [ForeignKey(nameof(OpenETSyncID))]
        public virtual OpenETSync OpenETSync { get; set; }

        public OpenETSyncResultType OpenETSyncResultType => OpenETSyncResultType.AllLookupDictionary[OpenETSyncResultTypeID];
    }
}