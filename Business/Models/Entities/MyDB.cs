using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotNetXPlat.Business.Models
{
    public class MyDB : IdentityDbContext
    {
        public MyDB() : base() {
        }

        public MyDB(DbContextOptions<MyDB> options) : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}