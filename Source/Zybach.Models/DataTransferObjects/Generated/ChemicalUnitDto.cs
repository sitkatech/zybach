//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemicalUnit]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class ChemicalUnitDto
    {
        public int ChemicalUnitID { get; set; }
        public string ChemicalUnitName { get; set; }
        public string ChemicalUnitPluralName { get; set; }
        public string ChemicalUnitLowercaseShortName { get; set; }
    }

    public partial class ChemicalUnitSimpleDto
    {
        public int ChemicalUnitID { get; set; }
        public string ChemicalUnitName { get; set; }
        public string ChemicalUnitPluralName { get; set; }
        public string ChemicalUnitLowercaseShortName { get; set; }
    }

}