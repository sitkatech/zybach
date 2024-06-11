using System;

namespace Zybach.Models.DataTransferObjects;

public class WellForFlowTestReportPageDto
{
    public int WellID { get; set; }
    public string WellRegistrationID { get; set; }
    public int ChemigationPermitNumber { get; set; }
    public string ChemigationPermitNumberDisplay { get; set; }
    public string FieldName { get; set; }
    public DateTime? LastInspected { get; set; }
    public DateTime? LastFlowTest { get; set; }
    public string AgHubRegisteredUser { get; set; }
    public string ChemigationPermitApplicantFirstName { get; set; }
    public string ChemigationPermitApplicantLastName { get; set; }
    public string ChemigationPermitApplicatorNames { get; set; }
}