namespace AlbaVulpes.API.Models.Config
{
    public class AppSettings
    {
        public string AuthCookieName { get; set; }
        public AppAWSSettings AWS { get; set; }
    }

    public class AppAWSSettings
    {
        public string CredentialsProfileName { get; set; }
        public string AppSecretsId { get; set; }
    }
}