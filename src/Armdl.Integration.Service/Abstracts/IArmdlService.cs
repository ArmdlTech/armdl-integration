using Armdl.Integration.Service.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Armdl.Integration.Service.Abstracts
{
    /// <summary>
    /// Defines an interface of armdl service.
    /// </summary>
    public interface IArmdlService
    {
        /// <summary>
        /// Get the access token.
        /// </summary>
        /// <param name="authorizationCode">The authorization code retrieved via callback URL.</param>
        /// <param name="callbackUrl">The URL, on which user going after authentication on armdl service.</param>
        /// <returns>The token details.</returns>
        Task<ArmdlToken> GetAccessToken(string authorizationCode, string callbackUrl);

        /// <summary>
        /// Refresh the access token and get new.
        /// </summary>
        /// <param name="refreshToken">The special refresh token value to getting new.</param>
        /// <returns>The token details.</returns>
        Task<ArmdlToken> RefreshAccessToken(string refreshToken);

        /// <summary>
        /// Get the user info.
        /// </summary>
        /// <param name="accessToken">The access token value.</param>
        /// <returns>The user defails.</returns>
        Task<ArmdlUser> GetUser(string accessToken);

        /// <summary>
        /// Get the user license info.
        /// </summary>
        /// <param name="accessToken">The access token value.</param>
        /// <returns>The license details.</returns>
        Task<ArmdlLicense> GetUserLicence(string accessToken);
    }
}
