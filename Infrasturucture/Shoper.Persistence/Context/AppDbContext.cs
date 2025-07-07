using Microsoft.EntityFrameworkCore;
using Shoper.Domain.Entities;

namespace Shoper.Persistence.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Category> Categories{ get; set; }
}