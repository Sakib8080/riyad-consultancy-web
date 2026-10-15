namespace ReyadConsultancy.Web.Areas.Admin.Models;

public enum CellKind { Text, Image, Bool, Date }

public class CrudCell
{
    public CellKind Kind { get; set; }
    public string? Text { get; set; }
    public bool Flag { get; set; }
}

public class CrudRow
{
    public int Id { get; set; }
    public List<CrudCell> Cells { get; set; } = new();
}

public class CrudIndexViewModel
{
    public string Title { get; set; } = string.Empty;
    public string EntityTitle { get; set; } = string.Empty;
    public bool CanCreate { get; set; } = true;
    public List<string> Headers { get; set; } = new();
    public List<CrudRow> Rows { get; set; } = new();
}

/// <summary>Describes one column of the generic admin list table.</summary>
public class CrudColumn<T>
{
    private readonly Func<T, CrudCell> _render;

    private CrudColumn(string header, Func<T, CrudCell> render)
    {
        Header = header;
        _render = render;
    }

    public string Header { get; }

    public CrudCell Render(T item) => _render(item);

    public static CrudColumn<T> Text(string header, Func<T, object?> value) =>
        new CrudColumn<T>(header, e => new CrudCell { Kind = CellKind.Text, Text = Shorten(value(e)?.ToString()) });

    public static CrudColumn<T> Image(string header, Func<T, string?> value) =>
        new CrudColumn<T>(header, e => new CrudCell { Kind = CellKind.Image, Text = value(e) });

    public static CrudColumn<T> Bool(string header, Func<T, bool> value) =>
        new CrudColumn<T>(header, e => new CrudCell { Kind = CellKind.Bool, Flag = value(e) });

    public static CrudColumn<T> Date(string header, Func<T, DateTime?> value) =>
        new CrudColumn<T>(header, e => new CrudCell { Kind = CellKind.Date, Text = value(e)?.ToString("dd MMM yyyy") });

    private static string? Shorten(string? text)
    {
        if (text == null) return null;
        text = System.Text.RegularExpressions.Regex.Replace(text, "<.*?>", " ");   // strip HTML for list display
        return text.Length > 80 ? text.Substring(0, 80) + "..." : text;
    }
}
