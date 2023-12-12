//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[OpenETSync]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class OpenETSyncDto
    {
        public int OpenETSyncID { get; set; }
        public OpenETDataTypeDto OpenETDataType { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime? FinalizeDate { get; set; }
    }

    public partial class OpenETSyncSimpleDto
    {
        public int OpenETSyncID { get; set; }
        public System.Int32 OpenETDataTypeID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime? FinalizeDate { get; set; }
    }

}