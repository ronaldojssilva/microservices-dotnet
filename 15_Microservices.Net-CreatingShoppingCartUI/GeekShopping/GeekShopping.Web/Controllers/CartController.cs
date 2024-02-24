using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;

namespace GeekShopping.Web.Controllers
{
    public class CartController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public CartController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            CartViewModel? response = await FindCart();
            return View(response);
        }

        private async Task<CartViewModel?> FindCart()
        {
            string token = await GetAccessToken();
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var response = await _cartService.FindCartByUserId(userId, token);
            if (response?.CartHeader != null)
            {
                foreach (var detail in response.CartDetails)
                {
                    response.CartHeader.PurchaseAmount += detail.Product.Price * detail.Count;
                }
            }

            return response;
        }
    }
}
