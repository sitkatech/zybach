namespace Zybach.EFModels.Entities
{
    public class BulkChemigationPermitAnnualRecordCreationResult
    {
        public int ChemigationPermitsRenewed { get; set; }
        public int ChemigationInspectionsCreated { get; set; }

        public BulkChemigationPermitAnnualRecordCreationResult()
        {
        }

        public BulkChemigationPermitAnnualRecordCreationResult(int chemigationPermitsRenewed, int chemigationInspectionsCreated)
        {
            ChemigationPermitsRenewed = chemigationPermitsRenewed;
            ChemigationInspectionsCreated = chemigationInspectionsCreated;
        }
    }
}