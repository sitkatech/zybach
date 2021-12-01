//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[ChemigationPermitAnnualRecordApplicator]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ChemigationPermitAnnualRecordApplicatorExtensionMethods
    {
        public static ChemigationPermitAnnualRecordApplicatorDto AsDto(this ChemigationPermitAnnualRecordApplicator chemigationPermitAnnualRecordApplicator)
        {
            var chemigationPermitAnnualRecordApplicatorDto = new ChemigationPermitAnnualRecordApplicatorDto()
            {
                ChemigationPermitAnnualRecordApplicatorID = chemigationPermitAnnualRecordApplicator.ChemigationPermitAnnualRecordApplicatorID,
                ChemigationPermitAnnualRecord = chemigationPermitAnnualRecordApplicator.ChemigationPermitAnnualRecord.AsDto(),
                ApplicatorName = chemigationPermitAnnualRecordApplicator.ApplicatorName,
                CertificationNumber = chemigationPermitAnnualRecordApplicator.CertificationNumber,
                ExpirationYear = chemigationPermitAnnualRecordApplicator.ExpirationYear,
                HomePhone = chemigationPermitAnnualRecordApplicator.HomePhone,
                MobilePhone = chemigationPermitAnnualRecordApplicator.MobilePhone
            };
            DoCustomMappings(chemigationPermitAnnualRecordApplicator, chemigationPermitAnnualRecordApplicatorDto);
            return chemigationPermitAnnualRecordApplicatorDto;
        }

        static partial void DoCustomMappings(ChemigationPermitAnnualRecordApplicator chemigationPermitAnnualRecordApplicator, ChemigationPermitAnnualRecordApplicatorDto chemigationPermitAnnualRecordApplicatorDto);

        public static ChemigationPermitAnnualRecordApplicatorSimpleDto AsSimpleDto(this ChemigationPermitAnnualRecordApplicator chemigationPermitAnnualRecordApplicator)
        {
            var chemigationPermitAnnualRecordApplicatorSimpleDto = new ChemigationPermitAnnualRecordApplicatorSimpleDto()
            {
                ChemigationPermitAnnualRecordApplicatorID = chemigationPermitAnnualRecordApplicator.ChemigationPermitAnnualRecordApplicatorID,
                ChemigationPermitAnnualRecordID = chemigationPermitAnnualRecordApplicator.ChemigationPermitAnnualRecordID,
                ApplicatorName = chemigationPermitAnnualRecordApplicator.ApplicatorName,
                CertificationNumber = chemigationPermitAnnualRecordApplicator.CertificationNumber,
                ExpirationYear = chemigationPermitAnnualRecordApplicator.ExpirationYear,
                HomePhone = chemigationPermitAnnualRecordApplicator.HomePhone,
                MobilePhone = chemigationPermitAnnualRecordApplicator.MobilePhone
            };
            DoCustomSimpleDtoMappings(chemigationPermitAnnualRecordApplicator, chemigationPermitAnnualRecordApplicatorSimpleDto);
            return chemigationPermitAnnualRecordApplicatorSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(ChemigationPermitAnnualRecordApplicator chemigationPermitAnnualRecordApplicator, ChemigationPermitAnnualRecordApplicatorSimpleDto chemigationPermitAnnualRecordApplicatorSimpleDto);
    }
}