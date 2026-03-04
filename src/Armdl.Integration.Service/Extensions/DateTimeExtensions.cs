using Armdl.Integration.Service.Configuration;

namespace Armdl.Integration.Service.Extensions
{
    internal static class DateTimeExtensions
    {
        public static DateTime OfficeTimeToUtc(this DateTime instance) => TimeZoneInfo.ConvertTimeToUtc(instance, ArmdlConfig.OfficeTimeZone);
    }
}
