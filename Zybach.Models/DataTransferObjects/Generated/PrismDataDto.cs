//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[PrismData]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class PrismDataDto
    {
        public int PrismDataID { get; set; }
        public string ElementType { get; set; }
        public DateTime? Date { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }
        public double? Value { get; set; }
    }

    public partial class PrismDataSimpleDto
    {
        public int PrismDataID { get; set; }
        public string ElementType { get; set; }
        public DateTime? Date { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }
        public double? Value { get; set; }
    }

}