using ReyadConsultancy.Application.Common.Interfaces;

namespace ReyadConsultancy.Web.Areas.Admin.Helpers;

public static class MediaUpload
{
    /// <summary>Stores an uploaded image in the database and returns its URL, or null when no file was posted.</summary>
    public static async Task<string?> SaveAsync(IMediaService media, IFormFile? file)
    {
        if (file == null || file.Length == 0) return null;
        using var stream = file.OpenReadStream();
        return await media.SaveImageAsync(file.FileName, file.ContentType, stream, file.Length);
    }
}
