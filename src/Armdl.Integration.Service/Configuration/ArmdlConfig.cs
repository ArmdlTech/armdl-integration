using System;
using System.Collections.Generic;
using System.Text;

namespace Armdl.Integration.Service.Configuration
{
    /// <summary>
    /// Defines integration configuration.
    /// </summary>
    public static class ArmdlConfig
    {
        /// <summary>
        /// Gets the office timezone.
        /// </summary>
        internal static TimeZoneInfo OfficeTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Russia Time Zone 3");

        /// <summary>
        /// Gets the token getting URL.
        /// </summary>
        internal static string TokenUrl = "/oauth/token";

        /// <summary>
        /// Gets the user info URL.
        /// </summary>
        internal static string UserInfoUrl = "/api/user";

        /// <summary>
        /// Gets the user license info URL.
        /// </summary>
        internal static string UserLicenseInfoUrl = "/user-license";

        /// <summary>
        /// Gets the base service address.
        /// </summary>
        public static string BaseUrl = "http://armdl.ru";

        /// <summary>
        /// Gets the registration URL.
        /// </summary>
        public static string RegistrationUrl = "/register";

        /// <summary>
        /// Gets the authorization URL with format parameters:
        /// 0 - client ID.
        /// 1 - redirect URL.
        /// 2 - state.
        /// </summary>
        public static string AuthorizationUrl = "/oauth/authorize?client_id={0}&amp;redirect_uri={1}&amp;response_type=code&amp;scope=*&amp;state={2}";
    }
}
