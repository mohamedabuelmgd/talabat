using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class Order :BaseEntity
    {
        public Order()
        {

        }
        public Order(string buerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal supTotal,string paymentIntentId)
        {
            BuerEmail = buerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            PaymentIntendId = paymentIntentId;
            SupTotal = supTotal;
        }

        public string BuerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public ICollection<OrderItem> Items { get; set; }
        public decimal SupTotal { get; set; }
        public string PaymentIntendId { get; set; }
        public decimal GetTotal()
            => SupTotal + DeliveryMethod.Cost;


    }
}
