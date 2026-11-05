using Kkanban.Data.Models;

namespace Kkanban.Services;

public interface ICardService
{
    Task<IEnumerable<KanbanCard>> GetCardsByColumnAsync(int columnId);
    Task<IEnumerable<KanbanCard>> GetAllCardsAsync();
    Task<KanbanCard> GetCardAsync(int id);
    Task CreateCardAsync(int columnId, string title, string description, string? assignee, IEnumerable<int> labelIds);
    Task UpdateCardAsync(KanbanCard card, IEnumerable<int> labelIds);
    Task MoveCardAsync(int cardId, int targetColumnId);
    Task ReorderCardsAsync(int columnId, IEnumerable<int> orderedCardIds);
    Task ArchiveCardAsync(int cardId);
    Task RestoreCardAsync(int cardId);
}
