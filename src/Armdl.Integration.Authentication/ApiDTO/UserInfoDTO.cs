using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Armdl.Integration.ApiDTO
{
    internal class UserInfoDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        [JsonProperty("email_verified_at")]
        public string EmailVerifiedAt { get; set; }

        public decimal Balance { get; set; }

        [JsonProperty("api_token")]
        public string ApiToken { get; set; }

        public int Type { get; set; }

        [JsonProperty("org_name")]
        public string OrganizationName { get; set; }

        [JsonProperty("org_inn")]
        public string OrganizationInn { get; set; }

        [JsonProperty("org_address")]
        public string OrganizationAddress { get; set; }

        [JsonProperty("telephone")]
        public string Phone { get; set; }

        [JsonProperty("passport_requisite")]
        public string PassportRequisite { get; set; }

        [JsonProperty("org_requisite")]
        public string OrgRequisite { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("plan_type_id")]
        public long PlanTypeId { get; set; }

        [JsonProperty("started_at")]
        public string StartedAt { get; set; }

        [JsonProperty("ended_at")]
        public string EndedAt { get; set; }

        [JsonProperty("is_admin")]
        public bool IsAdmin { get; set; }

        public UserLicenseInfoDTO License { get; set; }
    }
}
