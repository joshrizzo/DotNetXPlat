using System.Security.Claims;
using System.Threading.Tasks;
using DotNetXPlat.Business.Models;
using Microsoft.AspNetCore.Identity;

namespace DotNetXPlat.Business.Services
{
    public interface IDataSeeder
    {
        void SeedDB();
    }

    public class DataSeeder : IDataSeeder
    {
        private readonly MyDB dbContext;
        private readonly RoleManager<IdentityRole> roleManager;

        public DataSeeder(MyDB dbContext, RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
            this.dbContext = dbContext;
        }

        public void SeedDB()
        {
            SeedPermissions();
            SeedProducts();
        }

        private void SeedProducts()
        {
            dbContext.Products.Add(new Product
            {
                Name = "Test1",
                Description = "Desc1"
            });

            dbContext.Products.Add(new Product
            {
                Name = "Test2",
                Description = "Desc2"
            });

            dbContext.SaveChanges();
        }

        private void SeedPermissions()
        {
            var adminRole = new IdentityRole(Roles.Admin);
            roleManager.CreateAsync(adminRole).Wait();

            var claimTasks = new Task[] {
                roleManager.AddClaimAsync (adminRole, new Claim (Claims.Product.View, "")),
                roleManager.AddClaimAsync (adminRole, new Claim (Claims.Product.Edit, ""))
            };
            Task.WaitAll(claimTasks);
        }
    }
}