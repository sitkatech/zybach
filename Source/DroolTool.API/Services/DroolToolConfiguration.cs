namespace DroolTool.API.Services
{
    public class DroolToolConfiguration
    {
        public string KEYSTONE_HOST { get; set; }
        public string DB_CONNECTION_STRING { get; set; }
        public string SMTP_HOST { get; set; }
        public int SMTP_PORT { get; set; }
        public string SITKA_EMAIL_REDIRECT { get; set; }
        public string DROOLTOOL_WEB_URL { get; set; }
        public string KEYSTONE_REDIRECT_URL { get; set; }
    }
}