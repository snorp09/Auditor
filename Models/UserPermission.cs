namespace Auditor.Models;

public enum PermissionGrant
{
    None = 0,
    Read = 1,
    Write = 2,
    Admin = 3
}

public class UserPermission
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int BoardId { get; set; }
    public Board? Board { get; set; }
    public PermissionGrant Grant { get; set; }
}