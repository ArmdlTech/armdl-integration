using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Armdl.Integration.Service.ApiDTO
{
    internal class UserInfoDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        [JsonProperty("email_verified_at")]
        public string EmailVerifiedAt { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        public string Balance { get; set; }

        [JsonProperty("api_token")]
        public string ApiToken { get; set; }

        public int Type { get; set; }

        [JsonProperty("org_name")]
        public string OrganizationName { get; set; }

        [JsonProperty("org_inn")]
        public string OrganizationInn { get; set; }

        [JsonProperty("org_address")]
        public string OrganizationAddress { get; set; }
    }
}
