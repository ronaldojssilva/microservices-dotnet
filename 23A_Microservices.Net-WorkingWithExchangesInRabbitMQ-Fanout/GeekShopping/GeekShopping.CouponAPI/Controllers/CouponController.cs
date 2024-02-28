using GeekShopping.CouponAPI.Data.ValueObjects;
using GeekShopping.CouponAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]

    public class CouponController : ControllerBase
    {
        private ICouponRepository _repository;

        public CouponController(ICouponRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [Authorize]
        [HttpGet("{couponCode}")]
        public async Task<ActionResult<CouponVO>> GetByCouponCode(string couponCode)
        {
            var coupon = await _repository.GetByCouponCode(couponCode);
            if (coupon.Id <= 0) return NotFound();
            return Ok(coupon);
        }
    }
}
