namespace Infrastructure.CrossCutting.Extensions
{
    using System;
    using System.Globalization;

    public static class DateTimeExtensions
    {
        public static string ToIso8601String(this DateTime dateTime) => dateTime.ToString("o", CultureInfo.InvariantCulture);
    }
}