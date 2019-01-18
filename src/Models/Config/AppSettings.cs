namespace AlbaVulpes.API.Models.Config
{
    public class AppSettings
    {
        public string AuthCookieName { get; set; }
        public string AppSecretsId { get; set; }

        public AppSeqSettings Seq { get; set; }
    }

    public class AppSeqSettings
    {
        public string ServerUrl { get; set; }
    }
}