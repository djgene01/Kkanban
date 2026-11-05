namespace Kkanban.Data.Models;

public class Label
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(7)]
    public string Colour { get; set; } = string.Empty;

    public ICollection<CardLabel> CardLabels { get; set; } = new List<CardLabel>();
}
