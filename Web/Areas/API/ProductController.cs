using System;
using System.Linq;
using System.Threading.Tasks;
using DotNetXPlat.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.API
{
    [Area("API")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly MyDB dbContext;

        public ProductController(MyDB dbContext) {
            this.dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult Add(Product product) {
            dbContext.Add(product);
            dbContext.SaveChanges();
            return Ok(product.Id);
        }

        [HttpGet]
        public IActionResult GetById(Guid Id) {
            return Ok(dbContext.Products.Single(prod => prod.Id == Id));
        }

        [HttpGet]
        public IActionResult List() {
            return Ok(dbContext.Products.ToList());
        }
    }
}