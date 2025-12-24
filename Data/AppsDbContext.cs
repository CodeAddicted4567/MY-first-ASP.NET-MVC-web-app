using Microsoft.EntityFrameworkCore;
using DrinksStoreManage.Models;

namespace DrinksStoreManage.Data;

public class AppsDbContext : DbContext
{
    public AppsDbContext(DbContextOptions options) : base(options)//use this 'base' keyword when parent class contains parameterized constructor
    {
    }
    public DbSet<Drinks> Drinks { get; set; }
}
