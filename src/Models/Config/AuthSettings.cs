namespace AlbaVulpes.API.Models.Config
{
    public class AuthSettings
    {
        public GoogleAuthSettings Google { get; set; }
    }

    public class GoogleAuthSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}