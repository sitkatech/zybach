using System;

namespace Zybach.Models.DataTransferObjects;

public class WellForFlowTestReportPageDto
{
    public int WellID { get; set; }
    public string WellRegistrationNumber { get; set; }
    public int PermitNumber { get; set; }
    public string FieldName { get; set; }
    public DateTime? LastInspected { get; set; }
    public DateTime? LastFlowTest { get; set; }
    public string AgHubRegisteredUserName { get; set; }
    public string ChemigationPermitApplicantFirstName { get; set; }
    public string ChemigationPermitApplicantLastName { get; set; }
    public string ChemigationPermitApplicatorNames { get; set; }
}