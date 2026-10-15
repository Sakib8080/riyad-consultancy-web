namespace ReyadConsultancy.Web.Helpers;

public static class ViewHelpers
{
    /// <summary>Returns the value, or the fallback when the admin has not set it.</summary>
    public static string Or(this string? value, string fallback) => string.IsNullOrWhiteSpace(value) ? fallback : value!;

    public static string Initials(this string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "?";
        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length == 1 ? parts[0].Substring(0, 1).ToUpperInvariant()
            : (parts[0].Substring(0, 1) + parts[^1].Substring(0, 1)).ToUpperInvariant();
    }

    public static string MonthName(int month) =>
        System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(month);

    public static string[] SplitTags(this string? tags) =>
        string.IsNullOrWhiteSpace(tags)
            ? Array.Empty<string>()
            : tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}
