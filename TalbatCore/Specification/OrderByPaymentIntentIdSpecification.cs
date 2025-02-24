using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specification
{
    public class OrderByPaymentIntentIdSpecification:BaseSpecification<Order>
    {
        public OrderByPaymentIntentIdSpecification(string paymentIntentId)
            :base(o=>o.PaymentIntendId==paymentIntentId)
        {

        }
    }
}
