using DotNetXPlat.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotNetXPlat.Models
{
    public class MyDB : IdentityDbContext
    {
        public MyDB(DbContextOptions<MyDB> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}