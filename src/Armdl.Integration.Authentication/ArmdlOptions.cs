using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Armdl.Integration.Authentication
{
    /// <summary>
    /// Configuration options for <see cref="ArmdlHandler"/>.
    /// </summary>
    public class ArmdlOptions : OAuthOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArmdlOptions"/> class.
        /// </summary>
        public ArmdlOptions()
        {
            this.CallbackPath = new PathString("/api/callback");
            this.AuthorizationEndpoint = ArmdlDefaults.AuthorizationEndpoint;
            this.TokenEndpoint = ArmdlDefaults.TokenEndpoint;
            this.UserInformationEndpoint = ArmdlDefaults.UserInformationEndpoint;
            this.UserLicenseEndpoint = ArmdlDefaults.UserLicenseEndpoint;

            this.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            this.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            this.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

            this.SaveTokens = true;
        }

        /// <summary>
        /// Gets or sets user license info URL.
        /// </summary>
        public string UserLicenseEndpoint { get; set; }
    }
}
