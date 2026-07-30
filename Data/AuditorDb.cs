using Microsoft.EntityFrameworkCore;
using Auditor.Models;

namespace Auditor.Data;

public class AuditorDb : DbContext
{
    public AuditorDb(DbContextOptions<AuditorDb> options) : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }
}