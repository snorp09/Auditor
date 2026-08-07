namespace Auditor.Models;

public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }

    public List<UserPermission> UserPermissions { get; set; } = [];

    public List<Board> ViewableBoards { get => UserPermissions
        .Where(up => up.Grant >= PermissionGrant.Read)
        .Select(up => up.Board)
        .Where(b => b != null)
        .ToList()!;
    }
}