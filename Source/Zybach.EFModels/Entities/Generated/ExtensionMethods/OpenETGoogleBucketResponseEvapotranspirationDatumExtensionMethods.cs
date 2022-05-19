//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[OpenETGoogleBucketResponseEvapotranspirationDatum]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class OpenETGoogleBucketResponseEvapotranspirationDatumExtensionMethods
    {
        public static OpenETGoogleBucketResponseEvapotranspirationDatumDto AsDto(this OpenETGoogleBucketResponseEvapotranspirationDatum openETGoogleBucketResponseEvapotranspirationDatum)
        {
            var openETGoogleBucketResponseEvapotranspirationDatumDto = new OpenETGoogleBucketResponseEvapotranspirationDatumDto()
            {
                OpenETGoogleBucketResponseEvapotranspirationDatumID = openETGoogleBucketResponseEvapotranspirationDatum.OpenETGoogleBucketResponseEvapotranspirationDatumID,
                WellTPID = openETGoogleBucketResponseEvapotranspirationDatum.WellTPID,
                WaterMonth = openETGoogleBucketResponseEvapotranspirationDatum.WaterMonth,
                WaterYear = openETGoogleBucketResponseEvapotranspirationDatum.WaterYear,
                EvapotranspirationRateInches = openETGoogleBucketResponseEvapotranspirationDatum.EvapotranspirationRateInches,
                EvapotranspirationRateAcreFeet = openETGoogleBucketResponseEvapotranspirationDatum.EvapotranspirationRateAcreFeet
            };
            DoCustomMappings(openETGoogleBucketResponseEvapotranspirationDatum, openETGoogleBucketResponseEvapotranspirationDatumDto);
            return openETGoogleBucketResponseEvapotranspirationDatumDto;
        }

        static partial void DoCustomMappings(OpenETGoogleBucketResponseEvapotranspirationDatum openETGoogleBucketResponseEvapotranspirationDatum, OpenETGoogleBucketResponseEvapotranspirationDatumDto openETGoogleBucketResponseEvapotranspirationDatumDto);

        public static OpenETGoogleBucketResponseEvapotranspirationDatumSimpleDto AsSimpleDto(this OpenETGoogleBucketResponseEvapotranspirationDatum openETGoogleBucketResponseEvapotranspirationDatum)
        {
            var openETGoogleBucketResponseEvapotranspirationDatumSimpleDto = new OpenETGoogleBucketResponseEvapotranspirationDatumSimpleDto()
            {
                OpenETGoogleBucketResponseEvapotranspirationDatumID = openETGoogleBucketResponseEvapotranspirationDatum.OpenETGoogleBucketResponseEvapotranspirationDatumID,
                WellTPID = openETGoogleBucketResponseEvapotranspirationDatum.WellTPID,
                WaterMonth = openETGoogleBucketResponseEvapotranspirationDatum.WaterMonth,
                WaterYear = openETGoogleBucketResponseEvapotranspirationDatum.WaterYear,
                EvapotranspirationRateInches = openETGoogleBucketResponseEvapotranspirationDatum.EvapotranspirationRateInches,
                EvapotranspirationRateAcreFeet = openETGoogleBucketResponseEvapotranspirationDatum.EvapotranspirationRateAcreFeet
            };
            DoCustomSimpleDtoMappings(openETGoogleBucketResponseEvapotranspirationDatum, openETGoogleBucketResponseEvapotranspirationDatumSimpleDto);
            return openETGoogleBucketResponseEvapotranspirationDatumSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(OpenETGoogleBucketResponseEvapotranspirationDatum openETGoogleBucketResponseEvapotranspirationDatum, OpenETGoogleBucketResponseEvapotranspirationDatumSimpleDto openETGoogleBucketResponseEvapotranspirationDatumSimpleDto);
    }
}