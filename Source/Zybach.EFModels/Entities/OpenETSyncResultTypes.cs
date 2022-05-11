using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zybach.EFModels.Entities
{
    public partial class OpenETSyncResultTypes
    {
        public enum OpenETSyncResultTypeEnum
        {
            InProgress = 1,
            Succeeded = 2,
            Failed = 3,
            NoNewData = 4,
            DataNotAvailable = 5,
            Created = 6
        }
    }
}
