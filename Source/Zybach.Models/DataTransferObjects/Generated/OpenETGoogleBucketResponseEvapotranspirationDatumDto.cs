//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[OpenETGoogleBucketResponseEvapotranspirationDatum]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class OpenETGoogleBucketResponseEvapotranspirationDatumDto
    {
        public int OpenETGoogleBucketResponseEvapotranspirationDatumID { get; set; }
        public string WellTPID { get; set; }
        public int WaterMonth { get; set; }
        public int WaterYear { get; set; }
        public decimal? EvapotranspirationRateInches { get; set; }
        public decimal? EvapotranspirationRateAcreFeet { get; set; }
    }

    public partial class OpenETGoogleBucketResponseEvapotranspirationDatumSimpleDto
    {
        public int OpenETGoogleBucketResponseEvapotranspirationDatumID { get; set; }
        public string WellTPID { get; set; }
        public int WaterMonth { get; set; }
        public int WaterYear { get; set; }
        public decimal? EvapotranspirationRateInches { get; set; }
        public decimal? EvapotranspirationRateAcreFeet { get; set; }
    }

}