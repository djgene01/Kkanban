using Kkanban.Data;
using Kkanban.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kkanban.Services;

public class CardService : ICardService
{
    private readonly KkanbanContext _context;

    public CardService(KkanbanContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<KanbanCard>> GetCardsByColumnAsync(int columnId)
    {
        return await _context.Cards
            .Where(c => c.BoardColumnId == columnId && !c.IsDeleted)
            .OrderBy(c => c.Order)
            .ThenBy(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<KanbanCard>> GetAllCardsAsync()
    {
        return await _context.Cards
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.BoardColumnId)
            .ThenBy(c => c.Order)
            .ThenBy(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<KanbanCard> GetCardAsync(int id)
    {
        return await _context.Cards.FindAsync(id) 
            ?? throw new KeyNotFoundException($"Card {id} not found");
    }

    public async Task CreateCardAsync(int columnId, string title, string description, string? assignee, IEnumerable<int> labelIds)
    {
        int maxOrder = await _context.Cards
            .Where(c => c.BoardColumnId == columnId && !c.IsDeleted)
            .Select(c => (int)c.Order)
            .DefaultIfEmpty(0)
            .MaxAsync();

        var card = new KanbanCard
        {
            BoardColumnId = columnId,
            Title = title,
            Description = description,
            Assignee = assignee,
            Order = (short)(maxOrder + 1),
            CreatedAt = DateTime.UtcNow
        };

        if (labelIds != null && labelIds.Any())
        {
            var labels = await _context.Labels
                .Where(l => labelIds.Contains(l.Id))
                .ToListAsync();

            foreach (var label in labels)
            {
                card.CardLabels.Add(new CardLabel { Label = label });
            }
        }

        _context.Cards.Add(card);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCardAsync(KanbanCard card, IEnumerable<int> labelIds)
    {
        if (labelIds != null && labelIds.Any())
        {
            // Remove existing labels
            var existingLabels = _context.CardLabels
                .Where(cl => cl.CardId == card.Id)
                .ToList();

            _context.CardLabels.RemoveRange(existingLabels);
            await _context.SaveChangesAsync();

            // Add new labels
            var labels = await _context.Labels
                .Where(l => labelIds.Contains(l.Id))
                .ToListAsync();

            foreach (var label in labels)
            {
                card.CardLabels.Add(new CardLabel { Label = label });
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task MoveCardAsync(int cardId, int targetColumnId)
    {
        var card = await GetCardAsync(cardId);
        card.BoardColumnId = targetColumnId;

        int maxOrder = await _context.Cards
            .Where(c => c.BoardColumnId == targetColumnId && c.Id != cardId && !c.IsDeleted)
            .Select(c => (int)c.Order)
            .DefaultIfEmpty(0)
            .MaxAsync();

        card.Order = (short)(maxOrder + 1);
        await _context.SaveChangesAsync();
    }

    public async Task ReorderCardsAsync(int columnId, IEnumerable<int> orderedCardIds)
    {
        var cards = orderedCardIds.ToList();
        for (int i = 0; i < cards.Count; i++)
        {
            var card = await _context.Cards.FindAsync(cards[i]);
            if (card != null && card.BoardColumnId == columnId)
            {
                card.Order = (short)i;
            }
        }
        await _context.SaveChangesAsync();
    }

    public async Task ArchiveCardAsync(int cardId)
    {
        var card = await GetCardAsync(cardId);
        card.IsDeleted = true;
        card.ArchivedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task RestoreCardAsync(int cardId)
    {
        var card = await GetCardAsync(cardId);
        card.IsDeleted = false;
        card.ArchivedAt = null;
        await _context.SaveChangesAsync();
    }
}
