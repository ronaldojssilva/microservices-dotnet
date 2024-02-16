﻿using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.FindAll();
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.Create(model);
                if (response != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Update(int id)
        {
            var model = await _productService.FindById(id);
            if (model != null)
                return View(model);
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.Update(model);
                if (response != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _productService.FindById(id);
            if (model != null)
                return View(model);
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductModel model)
        {
            var response = await _productService.DeleteById(model.Id);
            if (response)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
