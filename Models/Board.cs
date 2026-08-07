namespace Auditor.Models;

public class Board
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<Transaction> Transactions { get; set; } = [];

    public List<UserPermission> UserPermissions { get; set; } = [];
}