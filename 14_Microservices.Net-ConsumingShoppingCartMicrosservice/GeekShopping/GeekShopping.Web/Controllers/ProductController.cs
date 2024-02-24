﻿using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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

        private async Task<string> GetToken()
        {
            return await HttpContext.GetTokenAsync("access_token");
        }

        public async Task<IActionResult> Index()
        {
            string token = await GetToken();
            var products = await _productService.FindAll(token);
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                string token = await GetToken();
                var response = await _productService.Create(model, token);
                if (response != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Update(int id)
        {
            string token = await GetToken();
            var model = await _productService.FindById(id, token);
            if (model != null)
                return View(model);
            return NotFound();
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                string token = await GetToken();
                var response = await _productService.Update(model, token);
                if (response != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            string token = await GetToken();
            var model = await _productService.FindById(id, token);
            if (model != null)
                return View(model);
            return NotFound();
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> Delete(ProductViewModel model)
        {
            string token = await GetToken();
            var response = await _productService.DeleteById(model.Id, token);
            if (response)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
