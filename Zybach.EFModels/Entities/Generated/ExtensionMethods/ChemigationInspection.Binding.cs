//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationInspection]
namespace Zybach.EFModels.Entities
{
    public partial class ChemigationInspection
    {
        public ChemigationInspectionStatus ChemigationInspectionStatus => ChemigationInspectionStatus.AllLookupDictionary[ChemigationInspectionStatusID];
        public ChemigationInspectionType ChemigationInspectionType => ChemigationInspectionTypeID.HasValue ? ChemigationInspectionType.AllLookupDictionary[ChemigationInspectionTypeID.Value] : null;
        public ChemigationLowPressureValve ChemigationLowPressureValve => ChemigationLowPressureValveID.HasValue ? ChemigationLowPressureValve.AllLookupDictionary[ChemigationLowPressureValveID.Value] : null;
        public ChemigationInterlockType ChemigationInterlockType => ChemigationInterlockTypeID.HasValue ? ChemigationInterlockType.AllLookupDictionary[ChemigationInterlockTypeID.Value] : null;
    }
}