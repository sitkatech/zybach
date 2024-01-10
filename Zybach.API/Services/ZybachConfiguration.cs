namespace Zybach.API.Services
{
    public class ZybachConfiguration
    {
        public string KEYSTONE_HOST { get; set; }
        public string DB_CONNECTION_STRING { get; set; }
        public string SITKA_EMAIL_REDIRECT { get; set; }
        public string WEB_URL { get; set; }
        public string KEYSTONE_REDIRECT_URL { get; set; }
        public string DoNotReplyEmail { get; set; }
        public string AppAlertsEmail { get; set; }
        public string SupportEmail { get; set; }
        public string APPINSIGHTS_INSTRUMENTATIONKEY { get; set; }
        public string INFLUXDB_URL { get; set; }
        public string INFLUXDB_TOKEN { get; set; }
        public string INFLUXDB_ORG { get; set; }
        public string INFLUX_BUCKET { get; set; }
        public string GEOOPTIX_API_KEY { get; set; }
        public string GEOOPTIX_HOSTNAME { get; set; }
        public string AGHUB_API_BASE_URL { get; set; }
        public string AGHUB_API_BUCKET { get; set; }
        public string AGHUB_API_KEY { get; set; }
        public string GET_API_BASE_URL { get; set; }
        public string GET_API_SUBSCRIPTION_KEY { get; set; }
        public int GET_ROBUST_REVIEW_SCENARIO_RUN_CUSTOMER_ID { get; set; }
        public int GET_ROBUST_REVIEW_SCENARIO_RUN_USER_ID { get; set; }
        public int GET_ROBUST_REVIEW_SCENARIO_RUN_MODEL_ID { get; set; }
        public int GET_ROBUST_REVIEW_SCENARIO_RUN_SCENARIO_ID { get; set; }
        public string VEGA_RENDER_URL { get; set; }
        public string SendGridApiKey { get; set; }
        public string OPENET_API_KEY { get; set; }
        public string OPENET_SHAPEFILE_PATH { get; set; }
        public string OpenETAPIBaseUrl { get; set; }
        public string OpenETRasterTimeSeriesMultipolygonRoute { get; set; }
        public string OpenETRasterMetadataRoute { get; set; }
        public string DefaultBoundingBoxLeft { get; set; }
        public string DefaultBoundingBoxRight { get; set; }
        public string DefaultBoundingBoxTop { get; set; }
        public string DefaultBoundingBoxBottom { get; set; }
        public string OpenETRasterTimeseriesMultipolygonColumnToUseAsIdentifier { get; set; }
        public bool AllowOpenETSync { get; set; }
    }
}