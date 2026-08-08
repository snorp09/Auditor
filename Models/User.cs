namespace Auditor.Models;

public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }

    public List<UserPermission> UserPermissions { get; set; } = [];

    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Email: {Email}";
    }
}