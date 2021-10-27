//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationInspection]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class ChemigationInspectionDto
    {
        public int ChemigationInspectionID { get; set; }
        public string WellRegistrationID { get; set; }
        public string ProtocolCanonicalName { get; set; }
        public string Status { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}