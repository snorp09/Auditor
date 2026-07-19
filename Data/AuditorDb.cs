using Microsoft.EntityFrameworkCore;
using Auditor.Models;

namespace Auditor.Data;

public class AuditorDb : DbContext
{
    public AuditorDb(DbContextOptions<AuditorDb> options) : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; }
}