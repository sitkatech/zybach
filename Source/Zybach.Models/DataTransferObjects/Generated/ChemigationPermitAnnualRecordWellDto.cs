//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermitAnnualRecordWell]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class ChemigationPermitAnnualRecordWellDto
    {
        public int ChemigationPermitAnnualRecordWellID { get; set; }
        public ChemigationPermitAnnualRecordDto ChemigationPermitAnnualRecord { get; set; }
        public WellDto Well { get; set; }
    }

    public partial class ChemigationPermitAnnualRecordWellSimpleDto
    {
        public int ChemigationPermitAnnualRecordWellID { get; set; }
        public int ChemigationPermitAnnualRecordID { get; set; }
        public int WellID { get; set; }
    }

}