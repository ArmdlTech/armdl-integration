using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Armdl.Integration.Authentication
{
    /// <summary>
    /// Defines extensions methods to adding armdl oauth authentication.
    /// </summary>
    public static class ArmdlExtensions
    {
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
        /// Add the armdl authentication supporting.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configureOptions">The action to configure armdl options.</param>
        /// <returns>The source referenced and configured authentication builder.</returns>
        public static AuthenticationBuilder AddArmdl(this AuthenticationBuilder builder, Action<ArmdlOptions> configureOptions)
            => builder.AddOAuth<ArmdlOptions, ArmdlHandler>(ArmdlDefaults.AuthenticationScheme, ArmdlDefaults.DisplayName, configureOptions);
    }
}
