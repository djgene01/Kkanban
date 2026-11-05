namespace Kkanban.Data.Models;

public class BoardColumn
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Title { get; set; } = string.Empty;

    public int Order { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? ArchivedAt { get; set; }

    public ICollection<KanbanCard> Cards { get; set; } = new List<KanbanCard>();
}
