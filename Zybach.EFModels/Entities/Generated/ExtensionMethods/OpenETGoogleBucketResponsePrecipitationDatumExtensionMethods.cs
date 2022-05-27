//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[OpenETGoogleBucketResponsePrecipitationDatum]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class OpenETGoogleBucketResponsePrecipitationDatumExtensionMethods
    {
        public static OpenETGoogleBucketResponsePrecipitationDatumDto AsDto(this OpenETGoogleBucketResponsePrecipitationDatum openETGoogleBucketResponsePrecipitationDatum)
        {
            var openETGoogleBucketResponsePrecipitationDatumDto = new OpenETGoogleBucketResponsePrecipitationDatumDto()
            {
                OpenETGoogleBucketResponsePrecipitationDatumID = openETGoogleBucketResponsePrecipitationDatum.OpenETGoogleBucketResponsePrecipitationDatumID,
                WellTPID = openETGoogleBucketResponsePrecipitationDatum.WellTPID,
                WaterMonth = openETGoogleBucketResponsePrecipitationDatum.WaterMonth,
                WaterYear = openETGoogleBucketResponsePrecipitationDatum.WaterYear,
                PrecipitationAcreFeet = openETGoogleBucketResponsePrecipitationDatum.PrecipitationAcreFeet,
                PrecipitationInches = openETGoogleBucketResponsePrecipitationDatum.PrecipitationInches
            };
            DoCustomMappings(openETGoogleBucketResponsePrecipitationDatum, openETGoogleBucketResponsePrecipitationDatumDto);
            return openETGoogleBucketResponsePrecipitationDatumDto;
        }

        static partial void DoCustomMappings(OpenETGoogleBucketResponsePrecipitationDatum openETGoogleBucketResponsePrecipitationDatum, OpenETGoogleBucketResponsePrecipitationDatumDto openETGoogleBucketResponsePrecipitationDatumDto);

        public static OpenETGoogleBucketResponsePrecipitationDatumSimpleDto AsSimpleDto(this OpenETGoogleBucketResponsePrecipitationDatum openETGoogleBucketResponsePrecipitationDatum)
        {
            var openETGoogleBucketResponsePrecipitationDatumSimpleDto = new OpenETGoogleBucketResponsePrecipitationDatumSimpleDto()
            {
                OpenETGoogleBucketResponsePrecipitationDatumID = openETGoogleBucketResponsePrecipitationDatum.OpenETGoogleBucketResponsePrecipitationDatumID,
                WellTPID = openETGoogleBucketResponsePrecipitationDatum.WellTPID,
                WaterMonth = openETGoogleBucketResponsePrecipitationDatum.WaterMonth,
                WaterYear = openETGoogleBucketResponsePrecipitationDatum.WaterYear,
                PrecipitationAcreFeet = openETGoogleBucketResponsePrecipitationDatum.PrecipitationAcreFeet,
                PrecipitationInches = openETGoogleBucketResponsePrecipitationDatum.PrecipitationInches
            };
            DoCustomSimpleDtoMappings(openETGoogleBucketResponsePrecipitationDatum, openETGoogleBucketResponsePrecipitationDatumSimpleDto);
            return openETGoogleBucketResponsePrecipitationDatumSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(OpenETGoogleBucketResponsePrecipitationDatum openETGoogleBucketResponsePrecipitationDatum, OpenETGoogleBucketResponsePrecipitationDatumSimpleDto openETGoogleBucketResponsePrecipitationDatumSimpleDto);
    }
}