//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[OpenETGoogleBucketResponsePrecipitationDatum]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class OpenETGoogleBucketResponsePrecipitationDatumDto
    {
        public int OpenETGoogleBucketResponsePrecipitationDatumID { get; set; }
        public string WellTPID { get; set; }
        public int WaterMonth { get; set; }
        public int WaterYear { get; set; }
        public decimal? PrecipitationAcreFeet { get; set; }
        public decimal? PrecipitationInches { get; set; }
    }

    public partial class OpenETGoogleBucketResponsePrecipitationDatumSimpleDto
    {
        public int OpenETGoogleBucketResponsePrecipitationDatumID { get; set; }
        public string WellTPID { get; set; }
        public int WaterMonth { get; set; }
        public int WaterYear { get; set; }
        public decimal? PrecipitationAcreFeet { get; set; }
        public decimal? PrecipitationInches { get; set; }
    }

}