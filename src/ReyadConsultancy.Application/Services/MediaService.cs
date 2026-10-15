using Microsoft.EntityFrameworkCore;
using ReyadConsultancy.Application.Common.Interfaces;
using ReyadConsultancy.Domain.Entities;

namespace ReyadConsultancy.Application.Services;

public class MediaService : IMediaService
{
    private const long MaxBytes = 5 * 1024 * 1024;
    private const string UrlPrefix = "/media/";
    private static readonly HashSet<string> AllowedTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg", "image/png", "image/gif", "image/webp"
    };

    private readonly IApplicationDbContext _db;

    public MediaService(IApplicationDbContext db) => _db = db;

    public async Task<string> SaveImageAsync(string fileName, string contentType, Stream content, long length)
    {
        if (!AllowedTypes.Contains(contentType))
            throw new InvalidOperationException("Only JPG, PNG, GIF or WEBP images are allowed.");
        if (length <= 0 || length > MaxBytes)
            throw new InvalidOperationException("Image must be smaller than 5 MB.");

        using var ms = new MemoryStream();
        await content.CopyToAsync(ms);
        var bytes = ms.ToArray();
        if (bytes.Length > MaxBytes) throw new InvalidOperationException("Image must be smaller than 5 MB.");
        if (!LooksLikeImage(bytes)) throw new InvalidOperationException("The file is not a valid image.");

        var file = new MediaFile
        {
            FileName = Path.GetFileName(fileName),
            ContentType = contentType.ToLowerInvariant(),
            Data = bytes,
            Length = bytes.Length
        };
        _db.Set<MediaFile>().Add(file);
        await _db.SaveChangesAsync();
        return UrlPrefix + file.Id;
    }

    public Task<MediaFile?> GetAsync(Guid id) => _db.Set<MediaFile>().AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

    public async Task DeleteByUrlAsync(string? url)
    {
        if (string.IsNullOrWhiteSpace(url) || !url.StartsWith(UrlPrefix, StringComparison.OrdinalIgnoreCase)) return;
        if (!Guid.TryParse(url.Substring(UrlPrefix.Length), out var id)) return;
        var file = await _db.Set<MediaFile>().FirstOrDefaultAsync(m => m.Id == id);
        if (file == null) return;
        _db.Set<MediaFile>().Remove(file);
        await _db.SaveChangesAsync();
    }

    // Magic-number check so a renamed .exe / .html cannot be stored as an "image".
    private static bool LooksLikeImage(byte[] b)
    {
        if (b.Length < 12) return false;
        if (b[0] == 0xFF && b[1] == 0xD8 && b[2] == 0xFF) return true;                                   // JPEG
        if (b[0] == 0x89 && b[1] == 0x50 && b[2] == 0x4E && b[3] == 0x47) return true;                  // PNG
        if (b[0] == 'G' && b[1] == 'I' && b[2] == 'F' && b[3] == '8') return true;                      // GIF
        if (b[0] == 'R' && b[1] == 'I' && b[2] == 'F' && b[3] == 'F' &&
            b[8] == 'W' && b[9] == 'E' && b[10] == 'B' && b[11] == 'P') return true;                    // WEBP
        return false;
    }
}
