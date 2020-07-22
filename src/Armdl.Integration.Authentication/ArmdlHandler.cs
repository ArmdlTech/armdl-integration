using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Armdl.Integration.Authentication
{
    /// <summary>
    /// Defines the armdl authentication requests handler.
    /// </summary>
    public class ArmdlHandler : OAuthHandler<ArmdlOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArmdlHandler"/> class.
        /// </summary>
        /// <param name="options">The authentication options.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="encoder">The URL encoder.</param>
        /// <param name="clock">The system time meter.</param>
        public ArmdlHandler(IOptionsMonitor<ArmdlOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        /// <summary>
        /// Create the ticket to getting user info.
        /// </summary>
        /// <param name="identity">The claims identity.</param>
        /// <param name="properties">The authentication properties.</param>
        /// <param name="tokens">The token info.</param>
        /// <returns>The authentication ticket result.</returns>
        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            var userInfoResponse = await this.DoRequest(this.Options.UserInformationEndpoint, tokens.AccessToken);
            var licenseInfoResponse = await this.DoRequest(this.Options.UserLicenseEndpoint, tokens.AccessToken);

            var userData = JsonDocument.Parse(await userInfoResponse.Content.ReadAsStringAsync());
            var licenseData = JsonDocument.Parse(await licenseInfoResponse.Content.ReadAsStringAsync());

            try
            {
                var context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, Context, Scheme, Options, Backchannel, tokens, userData.RootElement);
                context.RunClaimActions();

                //// Add missing user and license info properties to claims.
                var userClaims = userData.RootElement.GetClaims("user").Where(x => !context.Identity.Claims.Any(v => v.Type == x.Type)).ToList();
                var licenseClaims = licenseData.RootElement.GetClaims("user:license").Where(x => !context.Identity.Claims.Any(v => v.Type == x.Type)).ToList();

                context.Identity.AddClaims(userClaims);
                context.Identity.AddClaims(licenseClaims);

                await Events.CreatingTicket(context);
                return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
            }
            finally
            {
                userData.Dispose();
                licenseData.Dispose();
            }
        }

        private async Task<HttpResponseMessage> DoRequest(string url, string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await Backchannel.SendAsync(request, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"An error occurred when retrieving Armdl information by url = [{url}] ({response.StatusCode}). " +
                    $"Please check if the authentication information is correct and the corresponding Armdl Account API is enabled.");
            }

            return response;
        }
    }
}
