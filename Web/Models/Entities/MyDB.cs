using DotNetXPlat.Shared.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotNetXPlat.Web.Models
{
    public class MyDB : IdentityDbContext
    {
        public MyDB() : base()
        {
        }

        public MyDB(DbContextOptions<MyDB> options) : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}