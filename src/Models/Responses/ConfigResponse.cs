namespace AlbaVulpes.API.Models.Responses
{
    public class ConfigResponse
    {
        public string Environment { get; set; }

        public ConfigResponseExternal External { get; set; }
    }

    public class ConfigResponseExternal
    {
        public string GoogleApiKey { get; set; }
        public string GoogleClientId { get; set; }
    }
}