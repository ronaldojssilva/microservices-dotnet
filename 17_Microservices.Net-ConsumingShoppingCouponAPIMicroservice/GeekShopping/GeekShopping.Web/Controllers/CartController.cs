using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers
{
    public class CartController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly ICouponService _couponService;

        public CartController(IProductService productService, ICartService cartService, ICouponService couponService)
        {
            _productService = productService;
            _cartService = cartService;
            _couponService = couponService;
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
                if (!string.IsNullOrEmpty(response.CartHeader.CouponCode))
                {
                    var coupon = await _couponService.GetCoupon(response.CartHeader.CouponCode, token);
                    if (coupon?.Code != null)
                    {
                        response.CartHeader.DiscountAmount = coupon.DiscountAmount;
                    }
                }
                foreach (var detail in response.CartDetails)
                {
                    response.CartHeader.PurchaseAmount += detail.Product.Price * detail.Count;
                }
                response.CartHeader.PurchaseAmount -= response.CartHeader.DiscountAmount;
            }

            return response;
        }

        [HttpPost]
        [ActionName("ApplyCoupon")]
        public async Task<IActionResult> ApplyCoupon(CartViewModel model)
        {
            string token = await GetAccessToken();
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var response = await _cartService.ApplyCoupon(model, token);

            if (response)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        [ActionName("RemoveCoupon")]
        public async Task<IActionResult> RemoveCoupon()
        {
            string token = await GetAccessToken();
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var response = await _cartService.RemoveCoupon(userId, token);

            if (response)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
        public async Task<IActionResult> Remove(int id)
        {
            string token = await GetAccessToken();
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var response = await _cartService.RemoveFromCart(id, token);

            if (response)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

    }
}
