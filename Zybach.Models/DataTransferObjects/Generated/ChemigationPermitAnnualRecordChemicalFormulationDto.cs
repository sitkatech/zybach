//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermitAnnualRecordChemicalFormulation]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class ChemigationPermitAnnualRecordChemicalFormulationDto
    {
        public int ChemigationPermitAnnualRecordChemicalFormulationID { get; set; }
        public ChemigationPermitAnnualRecordDto ChemigationPermitAnnualRecord { get; set; }
        public ChemicalFormulationDto ChemicalFormulation { get; set; }
        public ChemicalUnitDto ChemicalUnit { get; set; }
        public decimal? TotalApplied { get; set; }
        public decimal AcresTreated { get; set; }
    }

    public partial class ChemigationPermitAnnualRecordChemicalFormulationSimpleDto
    {
        public int ChemigationPermitAnnualRecordChemicalFormulationID { get; set; }
        public System.Int32 ChemigationPermitAnnualRecordID { get; set; }
        public System.Int32 ChemicalFormulationID { get; set; }
        public System.Int32 ChemicalUnitID { get; set; }
        public decimal? TotalApplied { get; set; }
        public decimal AcresTreated { get; set; }
    }

}