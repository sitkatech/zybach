//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermitAnnualRecord]
namespace Zybach.EFModels.Entities
{
    public partial class ChemigationPermitAnnualRecord
    {
        public ChemigationPermitAnnualRecordStatus ChemigationPermitAnnualRecordStatus => ChemigationPermitAnnualRecordStatus.AllLookupDictionary[ChemigationPermitAnnualRecordStatusID];
        public ChemigationInjectionUnitType ChemigationInjectionUnitType => ChemigationInjectionUnitType.AllLookupDictionary[ChemigationInjectionUnitTypeID];
        public ChemigationPermitAnnualRecordFeeType ChemigationPermitAnnualRecordFeeType => ChemigationPermitAnnualRecordFeeTypeID.HasValue ? ChemigationPermitAnnualRecordFeeType.AllLookupDictionary[ChemigationPermitAnnualRecordFeeTypeID.Value] : null;
    }
}