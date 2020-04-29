using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zybach.EFModels.Entities
{
    public partial class DatabaseMigration
    {
        [Key]
        public int DatabaseMigrationNumber { get; set; }
    }
}
