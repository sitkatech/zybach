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
        public string KEYSTONE_AUTHORITY_URL { get; set; }
        public string GEOOPTIX_USERNAME { get; set; }
        public string GEOOPTIX_PASSWORD { get; set; }
        public string GEOOPTIX_CLIENT_ID { get; set; }
        public string GEOOPTIX_CLIENT_SECRET { get; set; }
        public string GEOOPTIX_HOST_NAME { get; set; }
    }
}