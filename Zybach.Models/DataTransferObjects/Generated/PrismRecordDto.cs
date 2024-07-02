//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[PrismRecord]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class PrismRecordDto
    {
        public int PrismRecordID { get; set; }
        public string ElementType { get; set; }
        public DateTime? Date { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }
        public double? Value { get; set; }
    }

    public partial class PrismRecordSimpleDto
    {
        public int PrismRecordID { get; set; }
        public string ElementType { get; set; }
        public DateTime? Date { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }
        public double? Value { get; set; }
    }

}