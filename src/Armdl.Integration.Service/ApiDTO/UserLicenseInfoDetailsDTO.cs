using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Armdl.Integration.Service.ApiDTO
{
    internal class UserLicenseInfoDetailsDTO
    {
        public long Id { get; set; }

        [JsonProperty("user_id")]
        public long UserId { get; set; }

        [JsonProperty("module_id")]
        public long ModuleId { get; set; }

        [JsonProperty("ended_at")]
        public string EndedAt { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("started_at")]
        public string StartedAt { get; set; }

        public string Token { get; set; }
    }
}
