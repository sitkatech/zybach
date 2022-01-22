//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermitAnnualRecordFeeType]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class ChemigationPermitAnnualRecordFeeTypeDto
    {
        public int ChemigationPermitAnnualRecordFeeTypeID { get; set; }
        public string ChemigationPermitAnnualRecordFeeTypeName { get; set; }
        public string ChemigationPermitAnnualRecordFeeTypeDisplayName { get; set; }
        public decimal FeeAmount { get; set; }
    }

    public partial class ChemigationPermitAnnualRecordFeeTypeSimpleDto
    {
        public int ChemigationPermitAnnualRecordFeeTypeID { get; set; }
        public string ChemigationPermitAnnualRecordFeeTypeName { get; set; }
        public string ChemigationPermitAnnualRecordFeeTypeDisplayName { get; set; }
        public decimal FeeAmount { get; set; }
    }

}