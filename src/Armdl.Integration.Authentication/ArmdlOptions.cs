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
            this.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            this.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            this.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

            this.SaveTokens = true;

            this.ConfigureUrls(ArmdlDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Gets or sets user license info URL.
        /// </summary>
        public string UserLicenseEndpoint { get; set; }

        /// <summary>
        /// Configure auth protocol URLs by scheme 
        /// <see cref="ArmdlDefaults.AuthenticationScheme"/> or <see cref="ArmdlDefaults.AuthenticationSchemeTech"/>.
        /// </summary>
        /// <param name="schemeName">The scheme value, by which URLs will be builded.</param>
        public void ConfigureUrls(string schemeName)
        {
            var baseAddress = ArmdlDefaults.GetBaseAddress(schemeName);

            this.AuthorizationEndpoint = baseAddress + ArmdlDefaults.AuthorizationEndpoint;
            this.TokenEndpoint = baseAddress + ArmdlDefaults.TokenEndpoint;
            this.UserInformationEndpoint = baseAddress + ArmdlDefaults.UserInformationEndpoint;
            this.UserLicenseEndpoint = baseAddress + ArmdlDefaults.UserLicenseEndpoint;

            //this.CallbackPath = new PathString($"/api/{schemeName.ToLowerInvariant()}-callback");
            this.CallbackPath = new PathString($"/api/callback");
        }
    }
}
