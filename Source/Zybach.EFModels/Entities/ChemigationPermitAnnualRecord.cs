namespace Zybach.EFModels.Entities
{
    public partial class ChemigationPermitAnnualRecord
    {
        public string ApplicantName
        {
            get
            {
                var applicantFullNameLastFirst = $"{(!string.IsNullOrWhiteSpace(ApplicantLastName) ? $"{ApplicantLastName}, " : "")}{(!string.IsNullOrWhiteSpace(ApplicantFirstName) ? ApplicantFirstName : "")}";
                if (!string.IsNullOrWhiteSpace(ApplicantCompany))
                {
                    return !string.IsNullOrWhiteSpace(applicantFullNameLastFirst) ? $"{ApplicantCompany} ({applicantFullNameLastFirst})" : ApplicantCompany;
                }

                return applicantFullNameLastFirst;
            }
        }
    }
}