using Auditor.Models;
using Auditor.Data;
using Microsoft.EntityFrameworkCore;
using Auditor.Services.Interfaces;

public class BoardManager : IBoardManager
{
    private readonly AuditorDb _db;

    public BoardManager(AuditorDb db)
    {
        _db = db;
    }

    public async Task<Board?> GetBoardAsync(int id)
    {
        return await _db.Boards.FindAsync(id);
    }

    public async Task<IEnumerable<Board>> GetAllBoardsAsync()
    {
        return await Task.FromResult(_db.Boards.AsEnumerable());
    }

    public async Task<Board> CreateBoardAsync(User user, string? boardname = null)
    {
        Board newBoard = new Board
        {
            Name = boardname ?? $"{user.Name}'s Board"
        };
        _db.Boards.Add(newBoard);
        await _db.SaveChangesAsync();
        newBoard.UserPermissions.Add(new UserPermission
        {
            UserId = user.Id,
            BoardId = newBoard.Id,
            Grant = PermissionGrant.Admin
        });
        await _db.SaveChangesAsync();
        return newBoard;
    }

    public Task<IEnumerable<Board>> GetAllUserBoardsAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<Board?> GetUserFirstBoardAsync(int userId)
    {
        var boards = _db.UserPermissions
            .Where(up => up.UserId == userId && up.Grant == PermissionGrant.Admin)
            .OrderBy(up => up.Id)
            .Select(up => up.Board);

        return await boards.FirstOrDefaultAsync();
    }

    public Task UpdateUserPermissionAsync(int boardId, int userId, PermissionGrant permission)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByBoardAsync(int boardId)
    {
        var transactions = _db.Transactions
            .Where(t => t.BoardId == boardId)
            .OrderByDescending(t => t.Date)
            .ThenByDescending(t => t.Id);

        return await transactions.ToListAsync();
    }
}