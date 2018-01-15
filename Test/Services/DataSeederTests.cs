using System.Security.Claims;
using System.Threading.Tasks;
using DotNetXPlat.Business.Services;
using DotNetXPlat.Business.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DotNetXPlat.Test
{
    [TestClass]
    public class DataSeederTests
    {
        private DataSeeder target;
        private Mock<DbSet<Product>> products;
        private Mock<RoleManager<IdentityRole>> roleManager;

        [TestInitialize]
        public void Setup() {
            var dbContext = new Mock<MyDB>(); 
            products = new Mock<DbSet<Product>>();
            dbContext.SetupGet(mock => mock.Products).Returns(products.Object);

            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            roleManager = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);

            target = new DataSeeder(dbContext.Object, roleManager.Object);
        }

        [TestMethod]
        public void SeedDB_ShouldAddProducts()
        {
            target.SeedDB();
            products.Verify(mock => mock.Add(It.IsAny<Product>()), Times.Exactly(2));
        }

        [TestMethod]
        public void SeedDB_ShouldAddPermissions()
        {
            target.SeedDB();
            roleManager.Verify(mock => mock.CreateAsync(It.IsAny<IdentityRole>()), Times.Exactly(1));
            roleManager.Verify(mock => mock.AddClaimAsync(It.IsAny<IdentityRole>(), It.IsAny<Claim>()), Times.Exactly(2));
        }
    }
}
