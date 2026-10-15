using System.ComponentModel.DataAnnotations;

namespace ReyadConsultancy.Domain.Entities;

/// <summary>Uploaded image stored inside MS SQL (varbinary) so ALL data lives in the database.</summary>
public class MediaFile
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, StringLength(260)]
    public string FileName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string ContentType { get; set; } = string.Empty;

    public byte[] Data { get; set; } = Array.Empty<byte>();

    public long Length { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
