using GeekShopping.CartAPI.Data.ValueObjects;

namespace GeekShopping.CartAPI.Repository
{
    public interface ICouponRepository
    {
        Task<CouponVO> GetByCoupon(string couponCode, string token);
    }
}
