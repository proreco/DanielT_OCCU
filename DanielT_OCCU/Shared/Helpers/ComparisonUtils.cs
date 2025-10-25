namespace DanielT_OCCU.Shared
{
    public static class ComparisonUtils
    {
        public static bool EqualsIgnoreCase(string? a, string? b) =>
            string.Equals(a, b, StringComparison.OrdinalIgnoreCase);

        public static bool IsDifferent(string? a, string? b) =>
            !string.Equals(a, b, StringComparison.Ordinal);
    }
}