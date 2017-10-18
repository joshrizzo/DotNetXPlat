using System;
using System.Linq;
using DotNetXPlat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetXPlat.Controllers
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

        [HttpGet]
        public IActionResult Product(Guid productId) {
            var existingProduct = getProductById(productId);
            return View(existingProduct);
        }

        [HttpPost]
        public IActionResult Product(Product product) {
            var existingProduct = getProductById(product.Id);

            if (existingProduct == default(Product)) {
                context.Products.Add(product);
            } else {
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
            }

            context.SaveChanges();

            return Redirect("Index");
        }

        private Product getProductById(Guid Id) {
            return context.Products.SingleOrDefault(prod => prod.Id == Id);
        }
    }
}
