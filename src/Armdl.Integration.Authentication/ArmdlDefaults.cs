using System;
using System.Collections.Generic;
using System.Text;
using TimeZoneConverter;

namespace Armdl.Integration.Authentication
{
    public static class ArmdlDefaults
    {
        public static TimeZoneInfo OfficeTimeZone = TZConvert.GetTimeZoneInfo("Russia Time Zone 3");

        public const string AuthenticationScheme = "Armdl";

        public const string DisplayName = "Armdl";

        public const string BaseAddress = "https://armdl.ru";

        public const string AuthenticationSchemeTech = "ArmdlTech";

        public const string DisplayNameTech = "ArmdlTech";

        public const string BaseAddressTech = "https://armdl.tech";

        public const string AuthorizationEndpoint = "/oauth/authorize";

        public const string TokenEndpoint = "/oauth/token";

        public const string UserInformationEndpoint = "/api/user";

        public const string UserLicenseEndpoint = "/api/user-license";

        public const string RegistrationEndpoint = "/register";

        public static string GetSchemeName(Uri uri) 
            => IsArmdlRu(uri) ? AuthenticationScheme : AuthenticationSchemeTech;

        public static string GetDisplayName(Uri uri) => IsArmdlRu(uri) ? DisplayName : DisplayNameTech;

        public static string GetRegistrationEndpoint(Uri uri) => GetBaseAddress(uri) + RegistrationEndpoint;

        public static string GetBaseAddress(Uri uri) => IsArmdlRu(uri) ? BaseAddress : BaseAddressTech;

        public static string GetBaseAddress(string schemeName) => 
            schemeName == AuthenticationScheme ? BaseAddress : BaseAddressTech;

        private static bool IsArmdlRu(Uri uri) => uri.Host.ToLowerInvariant().EndsWith(".armdl.ru");
    }
}
