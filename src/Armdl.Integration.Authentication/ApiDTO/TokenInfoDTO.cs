using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Armdl.Integration.ApiDTO
{
    internal class TokenInfoDTO
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
