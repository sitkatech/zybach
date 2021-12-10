namespace Zybach.EFModels.Entities
{
    public partial class ChemigationPermit
    {
        public string ChemigationPermitNumberDisplay => ChemigationPermitNumber.ToString("D4");
    }
}