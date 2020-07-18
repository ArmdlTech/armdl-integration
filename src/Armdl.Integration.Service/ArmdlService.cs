using Armdl.Integration.Service.Abstracts;
using Armdl.Integration.Service.ApiDTO;
using Armdl.Integration.Service.Configuration;
using Armdl.Integration.Service.Data;
using Armdl.Integration.Service.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Armdl.Integration.Service
{
    /// <summary>
    /// Defines the main armdl integration requests.
    /// </summary>
    public sealed class ArmdlService : IArmdlService
    {
        #region private fields.
        /// <summary>
        /// The unique client ID.
        /// </summary>
        private readonly string clientId;

        /// <summary>
        /// The related client secret with client ID.
        /// </summary>
        private readonly string clientSecret;
        #endregion

        #region construstors.
        /// <summary>
        /// Initializes a new instance of the <see cref="ArmdlService"/> class.
        /// </summary>
        /// <param name="clientId">The unique client ID registered on armdl service.</param>
        /// <param name="clientSecret">The related client secret key.</param>
        public ArmdlService(string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }
        #endregion

        #region public methods.
        /// <summary>
        /// Get the access token.
        /// </summary>
        /// <param name="authorizationCode">The authorization code retrieved via callback URL.</param>
        /// <param name="callbackUrl">The URL, on which user going after authentication on armdl service.</param>
        /// <returns>The token details.</returns>
        public async Task<ArmdlToken> GetAccessToken(string authorizationCode, string callbackUrl)
        {
            var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("client_id", this.clientId),
                new KeyValuePair<string, string>("client_secret", this.clientSecret),
                new KeyValuePair<string, string>("redirect_uri", callbackUrl),
                new KeyValuePair<string, string>("code", authorizationCode)
            });

            var response = await this.GetNewHttpClient().PostAsync(ArmdlConfig.TokenUrl, content);
            var responseMsg = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Can't receive the access token. Message: " + responseMsg);
            }

            var tokenInfo = JsonConvert.DeserializeObject<TokenInfoDTO>(responseMsg);

            return new ArmdlToken
            {
                Type = tokenInfo.TokenType,
                AccessToken = tokenInfo.AccessToken,
                RefreshToken = tokenInfo.RefreshToken,
                ExpiresInUTC = DateTime.UtcNow.AddMinutes(tokenInfo.ExpiresIn)
            };
        }

        /// <summary>
        /// Get the user info.
        /// </summary>
        /// <param name="accessToken">The access token value.</param>
        /// <returns>The user defails.</returns>
        public async Task<ArmdlUser> GetUser(string accessToken)
        {
            var response = await this.GetNewHttpClient(accessToken).GetAsync(ArmdlConfig.UserInfoUrl);
            var responseMsg = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Can't receive the user info. Message: " + responseMsg);
            }

            var userInfo = JsonConvert.DeserializeObject<UserInfoDTO>(responseMsg);

            return new ArmdlUser
            {
                ApiToken = userInfo.ApiToken,
                Balance = decimal.Parse(userInfo.Balance),
                CreatedAtUTC = DateTime.Parse(userInfo.CreatedAt).OfficeTimeToUtc(),
                Email = userInfo.Email,
                EmailVerifiedAtUTC = string.IsNullOrEmpty(userInfo.EmailVerifiedAt)
                    ? default(DateTime?)
                    : DateTime.Parse(userInfo.EmailVerifiedAt).OfficeTimeToUtc(),
                Id = userInfo.Id,
                Name = userInfo.Name,
                OrganizationAddress = userInfo.OrganizationAddress,
                OrganizationInn = userInfo.OrganizationInn,
                OrganizationName = userInfo.OrganizationName,
                Type = userInfo.Type,
                UpdatedAtUTC = DateTime.Parse(userInfo.UpdatedAt).OfficeTimeToUtc()
            };
        }

        /// <summary>
        /// Get the user license info.
        /// </summary>
        /// <param name="accessToken">The access token value.</param>
        /// <returns>The license details.</returns>
        public async Task<ArmdlLicense> GetUserLicence(string accessToken)
        {
            var response = await this.GetNewHttpClient(accessToken).GetAsync(ArmdlConfig.UserLicenseInfoUrl);
            var responseMsg = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Can't receive the license. Message: " + responseMsg);
            }

            var licenseInfo = JsonConvert.DeserializeObject<UserLicenseInfoDTO>(responseMsg);

            var hasLicense = licenseInfo.Info != null && licenseInfo.Status.ToLowerInvariant() == "success";
            var defaultNoLicenseDate = new DateTime(1900, 01, 01);

            return new ArmdlLicense
            {
                IsValid = hasLicense,
                Status = licenseInfo.Status,
                CreatedAtUTC = !hasLicense || string.IsNullOrEmpty(licenseInfo.Info.CreatedAt)
                        ? default(DateTime?)
                        : DateTime.Parse(licenseInfo.Info.CreatedAt).OfficeTimeToUtc(),
                EndedAtUTC = !hasLicense ? defaultNoLicenseDate : DateTime.Parse(licenseInfo.Info.EndedAt).OfficeTimeToUtc(),
                StartedAtUTC = !hasLicense ? defaultNoLicenseDate : DateTime.Parse(licenseInfo.Info.StartedAt).OfficeTimeToUtc(),
                UpdatedAtUTC = !hasLicense || string.IsNullOrEmpty(licenseInfo.Info.UpdatedAt)
                        ? default(DateTime?) :
                        DateTime.Parse(licenseInfo.Info.UpdatedAt).OfficeTimeToUtc(),
                Id = !hasLicense ? -1 : licenseInfo.Info.Id,
                ModuleId = !hasLicense ? -1 : licenseInfo.Info.ModuleId,
                Token = !hasLicense ? Guid.Empty.ToString() : licenseInfo.Info.Token,
                UserId = !hasLicense ? -1 : licenseInfo.Info.UserId
            };
        }

        /// <summary>
        /// Refresh the access token and get new.
        /// </summary>
        /// <param name="refreshToken">The special refresh token value to getting new.</param>
        /// <returns>The token details.</returns>
        public async Task<ArmdlToken> RefreshAccessToken(string refreshToken)
        {
            var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("client_id", this.clientId),
                new KeyValuePair<string, string>("client_secret", this.clientSecret),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("scope", "*")
            });

            var response = await this.GetNewHttpClient().PostAsync(ArmdlConfig.TokenUrl, content);
            var responseMsg = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Can't refresh the access token. Message: " + responseMsg);
            }

            var tokenInfo = JsonConvert.DeserializeObject<TokenInfoDTO>(responseMsg);

            return new ArmdlToken
            {
                Type = tokenInfo.TokenType,
                AccessToken = tokenInfo.AccessToken,
                RefreshToken = tokenInfo.RefreshToken,
                ExpiresInUTC = DateTime.UtcNow.AddMinutes(tokenInfo.ExpiresIn)
            };
        }
        #endregion

        #region private methods.
        private HttpClient GetNewHttpClient(string accessToken = null)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(ArmdlConfig.BaseUrl)
            };

            if (!String.IsNullOrEmpty(accessToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            return client;
        }
        #endregion
    }
}
