namespace Zybach.API.Services
{
    public class ZybachConfiguration
    {
        public string KEYSTONE_HOST { get; set; }
        public string DB_CONNECTION_STRING { get; set; }
        public string SMTP_HOST { get; set; }
        public int SMTP_PORT { get; set; }
        public string SITKA_EMAIL_REDIRECT { get; set; }
        public string WEB_URL { get; set; }
        public string KEYSTONE_REDIRECT_URL { get; set; }
        public string PlatformLongName { get; set; }
        public string PlatformShortName { get; set; }
        public string LeadOrganizationLongName { get; set; }
        public string LeadOrganizationShortName { get; set; }
        public string LeadOrganizationHomeUrl { get; set; }
        public string LeadOrganizationEmail { get; set; }
        public string APPINSIGHTS_INSTRUMENTATIONKEY { get; set; }
        public string API_KEY_VALUE { get; set; }
        public string INFLUXDB_URL { get; set; }
        public string INFLUXDB_TOKEN { get; set; }
        public string INFLUXDB_ORG { get; set; }
        public string INFLUX_BUCKET { get; set; }
        public string ZYBACH_CLIENT_ID { get; set; }
        public string ZYBACH_CLIENT_SECRET { get; set; }
        public string GEOOPTIX_API_KEY { get; set; }
        public string GEOOPTIX_HOSTNAME { get; set; }
        public string GEOOPTIX_SEARCH_HOSTNAME { get; set; }
        public string AGHUB_API_BASE_URL { get; set; }
        public string AGHUB_API_KEY { get; set; }
    }
}