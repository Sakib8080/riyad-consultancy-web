using ReyadConsultancy.Domain.Entities;

namespace ReyadConsultancy.Application.Common.Interfaces;

public interface IMediaService
{
    /// <summary>Validates and stores an image; returns the public URL (/media/{id}). Throws InvalidOperationException for invalid files.</summary>
    Task<string> SaveImageAsync(string fileName, string contentType, Stream content, long length);

    Task<MediaFile?> GetAsync(Guid id);

    /// <summary>Deletes the stored file when the URL points to /media/{id}; ignores any other URL.</summary>
    Task DeleteByUrlAsync(string? url);
}
