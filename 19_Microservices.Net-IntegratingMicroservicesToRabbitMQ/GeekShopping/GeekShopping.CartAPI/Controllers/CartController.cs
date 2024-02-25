using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Messages;
using GeekShopping.CartAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CartAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {
        private ICartRepository _repository;

        public CartController(ICartRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("find-cart/{id}")]
        public async Task<ActionResult<CartVO>> FindById(string id)
        {
            var cart = await _repository.FindCartByUserId(id);
            if (cart == null) 
                return NotFound();
            return Ok(cart);

        }

        [HttpPost("add-cart")]
        public async Task<ActionResult<CartVO>> AddCart(CartVO cartVO)
        {
            var cart = await _repository.SaveOrUpdate(cartVO);
            if (cart == null) 
                return NotFound();
            return Ok(cart);

        }

        [HttpPut("update-cart")]
        public async Task<ActionResult<CartVO>> UpdateCart(CartVO cartVO)
        {
            var cart = await _repository.SaveOrUpdate(cartVO);
            if (cart == null) 
                return NotFound();
            return Ok(cart);

        }

        [HttpDelete("remove-cart/{id}")]
        public async Task<ActionResult<CartVO>> RemoveCart(int id)
        {
            var status = await _repository.RemoveFromCart(id);
            if (!status) 
                return BadRequest();
            return Ok(status);

        }

        [HttpPost("apply-coupon")]
        public async Task<ActionResult<CartVO>> ApplyCoupon(CartVO cartVO)
        {
            var status = await _repository.ApplyCoupon(cartVO.CartHeader.UserId, cartVO.CartHeader.CouponCode);
            if (!status)
                return NotFound();
            return Ok(status);

        }

        [HttpDelete("remove-coupon/{userId}")]
        public async Task<ActionResult<CartVO>> RemoveCoupon(string userId)
        {
            var status = await _repository.RemoveCoupon(userId);
            if (!status)
                return NotFound();
            return Ok(status);

        }

        [HttpPost("checkout")]
        public async Task<ActionResult<CheckoutHeaderVO>> Checkout(CheckoutHeaderVO checkoutHeaderVO)
        {
            if (checkoutHeaderVO?.UserId == null)
                return BadRequest();
            var cart = await _repository.FindCartByUserId(checkoutHeaderVO.UserId);
            if (cart == null)
                return NotFound();
            checkoutHeaderVO.CartDetails = cart.CartDetails;
            checkoutHeaderVO.DateTime = DateTime.Now;

            //TASK RabbitMQ logic comes here!!!

            return Ok(checkoutHeaderVO);

        }

    }
}