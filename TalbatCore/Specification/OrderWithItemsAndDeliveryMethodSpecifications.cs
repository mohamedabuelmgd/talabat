using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specification
{
    public class OrderWithItemsAndDeliveryMethodSpecifications:BaseSpecification<Order> 
    {
        //this ctor to get all orders for user
        public OrderWithItemsAndDeliveryMethodSpecifications(string buyerEmail)
            :base(O=>O.BuerEmail == buyerEmail)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);


            AddOrderByDescending(O => O.OrderDate);
        }
        //this to get order by id for user
        public OrderWithItemsAndDeliveryMethodSpecifications(string buyerEmail ,int orderId)
               : base(O => O.BuerEmail == buyerEmail && O.Id==orderId)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);


        }
    }
}
