using Armdl.Integration.Service.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Armdl.Integration.Service.Extensions
{
    internal static class DateTimeExtensions
    {
        public static DateTime OfficeTimeToUtc(this DateTime instance) => TimeZoneInfo.ConvertTimeToUtc(instance, ArmdlConfig.OfficeTimeZone);
    }
}
