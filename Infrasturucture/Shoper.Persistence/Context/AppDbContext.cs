using Microsoft.EntityFrameworkCore;
using Shoper.Domain.Entities;

namespace Shoper.Persistence.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Category> Categories{ get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<City> City { get; set; }
    public DbSet<Town> Town { get; set; }
    public DbSet<Subscriber> Subscribers { get; set; }
    public DbSet<Help> Helps { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Favorites> Favorites { get; set; }
}