namespace Kkanban.Data.Models;

public class CardLabel
{
    public int CardId { get; set; }
    public KanbanCard Card { get; set; } = null!;

    public int LabelId { get; set; }
    public Label Label { get; set; } = null!;
}
