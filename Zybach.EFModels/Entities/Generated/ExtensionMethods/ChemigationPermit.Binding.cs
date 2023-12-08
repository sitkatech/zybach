//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermit]
namespace Zybach.EFModels.Entities
{
    public partial class ChemigationPermit
    {
        public int PrimaryKey => ChemigationPermitID;
        public ChemigationPermitStatus ChemigationPermitStatus => ChemigationPermitStatus.AllLookupDictionary[ChemigationPermitStatusID];
        public County County => County.AllLookupDictionary[CountyID];

        public static class FieldLengths
        {

        }
    }
}