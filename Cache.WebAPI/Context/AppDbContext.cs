using Cache.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Cache.WebAPI.Context;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product>? Products { get; set; }
}
