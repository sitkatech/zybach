using System.ComponentModel.DataAnnotations.Schema;

namespace Zybach.EFModels.Entities
{
    public partial class vOpenETMostRecentSyncHistoryForYearAndMonth
    {
        [ForeignKey(nameof(WaterYearMonthID))]
        public virtual WaterYearMonth WaterYearMonth { get; set; }

        public OpenETSyncResultType OpenETSyncResultType => OpenETSyncResultType.AllLookupDictionary[OpenETSyncResultTypeID];
    }
}