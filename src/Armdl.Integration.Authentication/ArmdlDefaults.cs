using System;
using System.Collections.Generic;
using System.Text;

namespace Armdl.Integration.Authentication
{
    public static class ArmdlDefaults
    {
        internal static TimeZoneInfo OfficeTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Russia Time Zone 3");

        public const string AuthenticationScheme = "Armdl";

        public static readonly string DisplayName = "Armdl";

        public static readonly string BaseAddress = "http://armdl.ru";

        public static readonly string AuthorizationEndpoint = BaseAddress + "/oauth/authorize";

        public static readonly string TokenEndpoint = BaseAddress + "/oauth/token";

        public static readonly string UserInformationEndpoint = BaseAddress + "/api/user";

        public static readonly string UserLicenseEndpoint = BaseAddress + "/api/user-license";

        public static readonly string RegistrationEndpoint = BaseAddress + "/register";
    }
}
