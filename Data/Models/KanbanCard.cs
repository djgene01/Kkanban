namespace Kkanban.Data.Models;

public class KanbanCard
{
    public int Id { get; set; }

    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Assignee { get; set; }

    public int Order { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; }

    public DateTime? ArchivedAt { get; set; }

    public int BoardColumnId { get; set; }

    public BoardColumn Column { get; set; } = null!;

    public ICollection<CardLabel> CardLabels { get; set; } = new List<CardLabel>();
}
