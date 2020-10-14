using System;
namespace OrderApi.Models
{
    public class OrderProductDto
    {
        /// <summary>
        /// 商品id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; } = 1;
    }
}
