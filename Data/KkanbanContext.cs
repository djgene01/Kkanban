using Microsoft.EntityFrameworkCore;
using Kkanban.Data.Models;

namespace Kkanban.Data;

public class KkanbanContext : DbContext
{
    public KkanbanContext(DbContextOptions<KkanbanContext> options)
        : base(options)
    {
    }

    public DbSet<BoardColumn> Columns => Set<BoardColumn>();
    public DbSet<KanbanCard> Cards => Set<KanbanCard>();
    public DbSet<Label> Labels => Set<Label>();
    public DbSet<CardLabel> CardLabels => Set<CardLabel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed default columns
        modelBuilder.Entity<BoardColumn>().HasData(
            new BoardColumn { Id = 1, Title = "To Do", Order = 0 },
            new BoardColumn { Id = 2, Title = "In Progress", Order = 1 },
            new BoardColumn { Id = 3, Title = "Done", Order = 2 }
        );

        // Relationships
        modelBuilder.Entity<KanbanCard>()
            .HasOne(c => c.Column)
            .WithMany(c => c.Cards)
            .HasForeignKey(c => c.BoardColumnId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CardLabel>()
            .HasKey(cl => new { cl.CardId, cl.LabelId });

        modelBuilder.Entity<CardLabel>()
            .HasOne(cl => cl.Card)
            .WithMany(c => c.Labels)
            .HasForeignKey(cl => cl.CardId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CardLabel>()
            .HasOne(cl => cl.Label)
            .WithMany(l => l.CardLabels)
            .HasForeignKey(cl => cl.LabelId)
            .OnDelete(DeleteBehavior.Cascade);

        // Default labels
        modelBuilder.Entity<Label>().HasData(
            new Label { Id = 1, Name = "High Priority", Colour = "#FF6B6B" },
            new Label { Id = 2, Name = "Medium Priority", Colour = "#FFC93C" },
            new Label { Id = 3, Name = "Low Priority", Colour = "#6BCB77" },
            new Label { Id = 4, Name = "Bug", Colour = "#FF4757" },
            new Label { Id = 5, Name = "Feature", Colour = "#4A90D9" },
            new Label { Id = 6, Name = "Enhancement", Colour = "#9B59B6" }
        );
    }
}
