using Kkanban.Data;
using Kkanban.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kkanban.Services;

public class BoardService : IBoardService
{
    private readonly KkanbanContext _context;

    public BoardService(KkanbanContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BoardColumn>> GetAllColumnsAsync(bool includeDeleted = false)
    {
        return await _context.Columns
            .Where(c => !includeDeleted || !c.IsDeleted)
            .OrderBy(c => c.Order)
            .ToListAsync();
    }

    public async Task<BoardColumn> GetColumnAsync(int id)
    {
        return await _context.Columns.FindAsync(id) 
            ?? throw new KeyNotFoundException($"Column {id} not found");
    }

    public async Task AddColumnAsync(string title)
    {
        int maxOrder = await _context.Columns
            .Where(c => !c.IsDeleted)
            .Select(c => (int)c.Order)
            .DefaultIfEmpty(0)
            .MaxAsync();

        var column = new BoardColumn
        {
            Title = title,
            Order = (short)(maxOrder + 1)
        };

        _context.Columns.Add(column);
        await _context.SaveChangesAsync();
    }

    public async Task RenameColumnAsync(int id, string newTitle)
    {
        var column = await GetColumnAsync(id) 
            ?? throw new KeyNotFoundException($"Column {id} not found");

        column.Title = newTitle;
        await _context.SaveChangesAsync();
    }

    public async Task ReorderColumnsAsync(IEnumerable<int> orderedColumnIds)
    {
        var columns = orderedColumnIds.ToList();
        for (int i = 0; i < columns.Count; i++)
        {
            var column = await _context.Columns.FindAsync(columns[i]);
            if (column != null)
            {
                column.Order = (short)i;
            }
        }
        await _context.SaveChangesAsync();
    }

    public async Task ArchiveColumnAsync(int id)
    {
        var column = await GetColumnAsync(id);
        column.IsDeleted = true;
        column.ArchivedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task RestoreColumnAsync(int id)
    {
        var column = await GetColumnAsync(id);
        column.IsDeleted = false;
        column.ArchivedAt = null;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteColumnAsync(int id)
    {
        var column = await GetColumnAsync(id);
        
        // Archive all cards in this column
        var cards = await _context.Cards
            .Where(c => c.BoardColumnId == id && !c.IsDeleted)
            .ToListAsync();
        
        foreach (var card in cards)
        {
            card.IsDeleted = true;
            card.ArchivedAt = DateTime.UtcNow;
        }

        // Archive the column
        column.IsDeleted = true;
        column.ArchivedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }
}
