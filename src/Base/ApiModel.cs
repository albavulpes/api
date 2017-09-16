using System;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Polenter.Serialization;

namespace AlbaVulpes.API.Base
{
    public class ApiModel
    {
        public Guid Id { get; set; }

        [JsonIgnore]
        public string Hash { get; set; }

        public virtual void ComputeHash()
        {
            // Remove current hash
            Hash = null;

            // Compute new hash
            using (var memoryStream = new MemoryStream())
            using (var hasher = MD5.Create())
            {
                var serializer = new SharpSerializer(true);
                serializer.Serialize(this, memoryStream);

                var hash = hasher.ComputeHash(memoryStream.ToArray());

                Hash = Convert.ToBase64String(hash);
            }
        }
    }
}
