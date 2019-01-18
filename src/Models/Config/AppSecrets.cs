namespace AlbaVulpes.API.Models.Config
{
    public class AppSecrets
    {
        public string Database_Host { get; set; }
        public string Database_Port { get; set; }
        public string Database_Name { get; set; }
        public string Database_Username { get; set; }
        public string Database_Password { get; set; }

        public string Seq_ApiKey { get; set; }
    }
}