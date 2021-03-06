﻿namespace AlbaVulpes.API.Models.Config
{
    public class AppSecrets
    {
        public string Database_Host { get; set; }
        public string Database_Port { get; set; }
        public string Database_Name { get; set; }
        public string Database_Username { get; set; }
        public string Database_Password { get; set; }

        public string Google_ApiKey { get; set; }
        public string Google_ClientId { get; set; }
        public string Google_ClientSecret { get; set; }

        public string Seq_ApiKey { get; set; }

        public string AWS_S3BucketName { get; set; }
    }
}