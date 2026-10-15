using System.Text;
using System.Text.RegularExpressions;

namespace ReyadConsultancy.Application.Common;

public static class Slugger
{
    public static string Create(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return "item";
        var sb = new StringBuilder();
        foreach (var ch in text.Trim().ToLowerInvariant())
        {
            if (char.IsLetterOrDigit(ch) && ch < 128) sb.Append(ch);
            else sb.Append('-');
        }
        var slug = Regex.Replace(sb.ToString(), "-{2,}", "-").Trim('-');
        return slug.Length == 0 ? "item" : (slug.Length > 180 ? slug.Substring(0, 180).Trim('-') : slug);
    }

    public static string Unique(string slug, IEnumerable<string?> existing)
    {
        var set = new HashSet<string>(existing.Where(s => s != null).Select(s => s!), StringComparer.OrdinalIgnoreCase);
        if (!set.Contains(slug)) return slug;
        var i = 2;
        while (set.Contains($"{slug}-{i}")) i++;
        return $"{slug}-{i}";
    }
}
