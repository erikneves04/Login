using Access.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace Access.Data;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options) { }

    public DbSet<User> User { get; set; }
    public DbSet<AccessLogger> AccessLoggers { get; set; }
}
