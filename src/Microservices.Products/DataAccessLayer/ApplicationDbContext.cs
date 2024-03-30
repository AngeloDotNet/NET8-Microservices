using Microservices.Products.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Products.DataAccessLayer;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
}