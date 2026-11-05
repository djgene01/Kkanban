using Kkanban.Data.Models;

namespace Kkanban.Services;

public interface IBoardService
{
    Task<IEnumerable<BoardColumn>> GetAllColumnsAsync(bool includeDeleted = false);
    Task<BoardColumn> GetColumnAsync(int id);
    Task AddColumnAsync(string title);
    Task RenameColumnAsync(int id, string newTitle);
    Task ReorderColumnsAsync(IEnumerable<int> orderedColumnIds);
    Task ArchiveColumnAsync(int id);
    Task RestoreColumnAsync(int id);
    Task DeleteColumnAsync(int id);
}
