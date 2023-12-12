//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermitAnnualRecord]
namespace Zybach.EFModels.Entities
{
    public partial class ChemigationPermitAnnualRecord
    {
        public int PrimaryKey => ChemigationPermitAnnualRecordID;
        public ChemigationPermitAnnualRecordStatus ChemigationPermitAnnualRecordStatus => ChemigationPermitAnnualRecordStatus.AllLookupDictionary[ChemigationPermitAnnualRecordStatusID];
        public ChemigationInjectionUnitType ChemigationInjectionUnitType => ChemigationInjectionUnitType.AllLookupDictionary[ChemigationInjectionUnitTypeID];
        public ChemigationPermitAnnualRecordFeeType ChemigationPermitAnnualRecordFeeType => ChemigationPermitAnnualRecordFeeTypeID.HasValue ? ChemigationPermitAnnualRecordFeeType.AllLookupDictionary[ChemigationPermitAnnualRecordFeeTypeID.Value] : null;

        public static class FieldLengths
        {
            public const int PivotName = 100;
            public const int ApplicantFirstName = 200;
            public const int ApplicantLastName = 200;
            public const int ApplicantMailingAddress = 100;
            public const int ApplicantCity = 50;
            public const int ApplicantState = 20;
            public const int ApplicantZipCode = 10;
            public const int ApplicantPhone = 30;
            public const int ApplicantMobilePhone = 30;
            public const int ApplicantEmail = 255;
            public const int TownshipRangeSection = 100;
            public const int ApplicantCompany = 200;
            public const int AnnualNotes = 500;
        }
    }
}