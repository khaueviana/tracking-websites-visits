﻿namespace Infrastructure.CrossCutting.Extensions
{
    public static class StringExtensions
    {
        public static string NullIfEmpty(this string value) => string.IsNullOrWhiteSpace(value) ? "null" : value;
    }
}
