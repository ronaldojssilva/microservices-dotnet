using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly MySQLContext _context;
        private IMapper _mapper;

        public CouponRepository(MySQLContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CouponVO> GetByCouponCode(string code, string token)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Code == code);
            return _mapper.Map<CouponVO>(coupon);
        }
    }
}
