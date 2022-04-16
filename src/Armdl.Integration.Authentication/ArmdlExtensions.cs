using Armdl.Integration.ApiDTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Armdl.Integration.Authentication
{
    /// <summary>
    /// Defines extensions methods to adding armdl oauth authentication.
    /// </summary>
    public static class ArmdlExtensions
    {
        internal static DateTime OfficeTimeToUtc(this DateTime instance) => TimeZoneInfo.ConvertTimeToUtc(instance, ArmdlDefaults.OfficeTimeZone);

        internal static void AddClaims(this ClaimsIdentity identity, IEnumerable<Claim> claims)
        {
            foreach (var claim in claims)
            {
                identity.AddClaim(claim);
            }
        }

        internal static IEnumerable<Claim> GetClaims(this JsonElement source, string parentName = "")
            => source.GetPropsRecursively(parentName).Select(x => new Claim(x.Name, x.Value));

        internal static List<(string Name, string Value)> GetPropsRecursively(this JsonElement source, string parentName = "")
        {
            parentName = parentName != string.Empty ? parentName + ":" : string.Empty;
            var result = new List<(string Name, string Value)>();
            foreach (var item in source.EnumerateObject())
            {
                if (item.Value.ValueKind == JsonValueKind.Object)
                {
                    result.AddRange(GetPropsRecursively(item.Value, $"{parentName}{item.Name}"));
                }
                else
                {
                    result.Add(($"{parentName}{item.Name}", item.Value.ToString()));
                }
            }

            return result;
        }

        /// <summary>
        /// Convert Armdl claims to user info object.
        /// </summary>
        /// <param name="claims">The armdl claims list.</param>
        /// <returns>The user info with license.</returns>
        public static ArmdlUser ToArmdlUser(this IEnumerable<Claim> claims)
        {
            var providerKey = ArmdlDefaults.AuthenticationScheme.ToLowerInvariant();

            var regex = new Regex(@"([\dA-z_]+:)+([\dA-z_]+)");
            var groups = claims
                .Where(x => x.Type.ToLowerInvariant().StartsWith(providerKey)
                    && regex.Match(x.Type).Success)
                .GroupBy(g => string.Join(string.Empty, regex.Match(g.Type).Groups[1].Captures.Select(x => x.Value.ToLowerInvariant())))
                .OrderBy(x => x.Key)
                .ToList();

            var userNodeKey = $"{providerKey}:user:";
            var licenseNodeKey = $"{providerKey}:user:license:";
            var licenseInfoNodeKey = $"{providerKey}:user:license:info:";

            dynamic ParseTypedValue(string value) => long.TryParse(value, out var number) ? (dynamic)number : value;

            JToken userObj = new JObject();
            foreach (var group in groups)
            {
                if (group.Key == userNodeKey)
                {
                    group.ToList().ForEach(x => userObj[regex.Match(x.Type).Groups[2].Value] = ParseTypedValue(x.Value));
                }
                else if (group.Key == licenseNodeKey)
                {
                    userObj["license"] = new JObject();
                    group.ToList().ForEach(x =>
                    {
                        var value = x.Value == "[]" ? null : ParseTypedValue(x.Value);

                        userObj["license"][regex.Match(x.Type).Groups[2].Value] = value;
                    });
                }
                else if (group.Key == licenseInfoNodeKey)
                {
                    userObj["license"]["info"] = new JObject();
                    group.ToList().ForEach(x =>
                    {
                        var value = x.Value == "[]" ? null : ParseTypedValue(x.Value);

                        userObj["license"]["info"][regex.Match(x.Type).Groups[2].Value] = value;
                    });
                }
            }

            return userObj.ToObject<UserInfoDTO>().ToArmdlUser();
        }

        /// <summary>
        /// Add the armdl authentication supporting by default scheme <see cref="ArmdlDefaults.AuthenticationScheme"/>.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configureOptions">The action to configure armdl options.</param>
        /// <returns>The source referenced and configured authentication builder.</returns>
        public static AuthenticationBuilder AddArmdl(this AuthenticationBuilder builder, Action<ArmdlOptions> configureOptions)
            => builder.AddOAuth<ArmdlOptions, ArmdlHandler>(ArmdlDefaults.AuthenticationScheme, ArmdlDefaults.DisplayName, configureOptions);

        public static AuthenticationBuilder AddArmdl(
            this AuthenticationBuilder builder, 
            string authenticationScheme, 
            string displayName, 
            Action<ArmdlOptions> configureOptions)
        {
            return builder.AddOAuth<ArmdlOptions, ArmdlHandler>(authenticationScheme, displayName, configureOptions);
        }

        private static ArmdlUser ToArmdlUser(this UserInfoDTO userInfo)
        {
            var licenseInfo = userInfo.License;

            var hasLicense = licenseInfo.Info != null && licenseInfo.Status.ToLowerInvariant() == "success";
            var defaultNoLicenseDate = new DateTime(1900, 01, 01);
            var endedAtUTCLicenseDate = defaultNoLicenseDate;
            if (hasLicense)
            {
                endedAtUTCLicenseDate = ParseDateInUtc(licenseInfo.Info.EndedAt);
            }

            var license = new ArmdlLicense
            {
                IsValid = hasLicense && DateTime.UtcNow < endedAtUTCLicenseDate,
                Status = licenseInfo.Status,
                CreatedAtUTC = !hasLicense || string.IsNullOrEmpty(licenseInfo.Info.CreatedAt)
                    ? default(DateTime?)
                    : ParseDateInUtc(licenseInfo.Info.CreatedAt),
                EndedAtUTC = endedAtUTCLicenseDate,
                StartedAtUTC = !hasLicense || string.IsNullOrEmpty(licenseInfo.Info.StartedAt) 
                    ? defaultNoLicenseDate 
                    : ParseDateInUtc(licenseInfo.Info.StartedAt),
                UpdatedAtUTC = !hasLicense || string.IsNullOrEmpty(licenseInfo.Info.UpdatedAt)
                    ? default(DateTime?) :
                    ParseDateInUtc(licenseInfo.Info.UpdatedAt),
                Id = !hasLicense ? -1 : licenseInfo.Info.Id,
                ModuleId = !hasLicense ? -1 : licenseInfo.Info.ModuleId,
                Token = !hasLicense ? Guid.Empty.ToString() : licenseInfo.Info.Token,
                UserId = !hasLicense ? -1 : licenseInfo.Info.UserId
            };

            return new ArmdlUser
            {

                Id = userInfo.Id,
                Name = userInfo.Name,
                Email = userInfo.Email,
                EmailVerifiedAtUTC = string.IsNullOrEmpty(userInfo.EmailVerifiedAt)
                    ? default(DateTime?)
                    : ParseDateInUtc(userInfo.EmailVerifiedAt),
                ApiToken = userInfo.ApiToken,
                Balance = userInfo.Balance,
                Type = userInfo.Type,
                OrganizationName = userInfo.OrganizationName,
                OrganizationInn = userInfo.OrganizationInn,
                OrganizationAddress = userInfo.OrganizationAddress,
                Phone = userInfo.Phone,
                PassportRequisite = userInfo.PassportRequisite,
                OrgRequisite = userInfo.OrgRequisite,
                CreatedAtUTC = ParseDateInUtc(userInfo.CreatedAt),
                UpdatedAtUTC = ParseDateInUtc(userInfo.UpdatedAt),
                PlanTypeId = userInfo.PlanTypeId,
                StartedAtUTC = !string.IsNullOrEmpty(userInfo.StartedAt) 
                    ? ParseDateInUtc(userInfo.StartedAt) 
                    : default,
                EndedAtUTC = !string.IsNullOrEmpty(userInfo.EndedAt)
                    ? ParseDateInUtc(userInfo.StartedAt)
                    : default,
                IsAdmin = userInfo.IsAdmin,
                License = license
            };
        }

        private static DateTime ParseDateInUtc(string source)
        {
            if (DateTimeOffset.TryParse(source, out var offsetDate))
            {
                return offsetDate.UtcDateTime;
            }
            else
            {
                return DateTime.Parse(source).OfficeTimeToUtc();
            }
        }
    }
}
