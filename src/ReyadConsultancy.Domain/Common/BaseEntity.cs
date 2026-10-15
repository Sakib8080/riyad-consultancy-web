namespace ReyadConsultancy.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>Entities that the admin can order manually.</summary>
public interface ISortable
{
    int SortOrder { get; set; }
}
