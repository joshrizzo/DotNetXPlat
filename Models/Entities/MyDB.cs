using Microsoft.EntityFrameworkCore;

namespace DotNetXPlat.Models
{
    public class MyDB : DbContext
    {
        public MyDB(DbContextOptions<MyDB> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}