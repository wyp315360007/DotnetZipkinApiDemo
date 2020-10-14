namespace OrderApi.Models
{
    public class OrderDto
    {
        /// <summary>
        /// 订单id
        /// </summary>
        public long Id { get; internal set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal TotalMoney { get; internal set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        public string Address { get; internal set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public string Addressee { get; internal set; }

        /// <summary>
        /// 发件人
        /// </summary>
        public string From { get; internal set; }

        /// <summary>
        /// 发件地址
        /// </summary>
        public string SendAddress { get; internal set; }
    }
}