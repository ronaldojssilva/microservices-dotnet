﻿using GeekShopping.CartAPI.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.CartAPI.Model
{
    [Table("cart_header")]
    public class CartHeader: BaseEntity
    {
        [Column("user_d")]
        public string UserId { get; set; }

        [Column("coupon_code")]
        public string? CouponCode { get; set; }
    }
}
