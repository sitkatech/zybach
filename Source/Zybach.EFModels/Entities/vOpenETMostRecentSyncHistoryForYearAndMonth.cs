using System.ComponentModel.DataAnnotations.Schema;

namespace Zybach.EFModels.Entities
{
    public partial class vOpenETMostRecentSyncHistoryForYearAndMonth
    {
        [ForeignKey(nameof(WaterYearMonthID))]
        public virtual WaterYearMonth WaterYearMonth { get; set; }

        [ForeignKey(nameof(OpenETSyncResultTypeID))]
        public virtual OpenETSyncResultType OpenETSyncResultType { get; set; }
        //public OpenETSyncResultType OpenETSyncResultType => OpenETSyncResultType.AllLookupDictionary[OpenETSyncResultTypeID];
    }
}