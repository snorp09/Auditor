using Auditor.Models;

public interface IBoardManager
{
    Task<Board?> GetBoardAsync(int id);
    Task<IEnumerable<Board>> GetAllBoardsAsync();
    Task<Board> CreateBoardAsync(User user, string? boardname = null);

    public Task<IEnumerable<Board>> GetAllUserBoardsAsync(int userId);

    public Task<Board?> GetUserFirstBoardAsync(int userId);

    public Task UpdateUserPermissionAsync(int boardId, int userId, PermissionGrant permission);
}