using System;
using System.Collections.Generic;

namespace OrderApi.Models
{
    public class OrderDetailDto:OrderDto
    {
        /// <summary>
        /// 订单商品
        /// </summary>
        public List<OrderProductDto> Products { get; set; }
    }
}
