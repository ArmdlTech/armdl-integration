using System;
using System.Collections.Generic;
using System.Text;

namespace Armdl.Integration.Service.Data
{
    /// <summary>
    /// Defines oauth token information.
    /// </summary>
    public class ArmdlToken
    {
        /// <summary>
        /// Gets the token type.
        /// </summary>
        public string Type { get; internal set; }

        /// <summary>
        /// Gets the expiration date.
        /// </summary>
        public DateTime ExpiresInUTC { get; internal set; }

        /// <summary>
        /// Gets the access token value.
        /// </summary>
        public string AccessToken { get; internal set; }

        /// <summary>
        /// Gets the refresh token value.
        /// </summary>
        public string RefreshToken { get; internal set; }
    }
}
