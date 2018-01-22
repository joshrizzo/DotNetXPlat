using System;
using System.Linq;
using DotNetXPlat.Shared.Models;
using DotNetXPlat.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omu.ValueInjecter;

namespace DotNetXPlat.Web.Controllers
{
    [Authorize]
    public class EFTestController: Controller
    {
        private readonly MyDB context;

        public EFTestController(MyDB context) {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Index() {
            var viewModel = context.Products.ToList();
            return View(viewModel);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        public IActionResult Product(Guid productId) {
            var existingProduct = getProductById(productId);
            return View(existingProduct);
        }

        [Authorize(Policy = "EditProduct")]
        [HttpPost]
        public IActionResult Product(Product product) {
            var existingProduct = getProductById(product.Id);

            if (existingProduct == default(Product)) {
                context.Products.Add(product);
            } else {
                existingProduct.InjectFrom(product);
            }

            context.SaveChanges();

            return Redirect("Index");
        }

        private Product getProductById(Guid Id) {
            return context.Products.SingleOrDefault(prod => prod.Id == Id);
        }
    }
}
