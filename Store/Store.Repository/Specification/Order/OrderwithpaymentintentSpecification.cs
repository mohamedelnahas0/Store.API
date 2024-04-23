using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.Order
{
    public class OrderwithpaymentintentSpecification : BaseSpecification<Data.Entities.OrderEntities.Order>
    {
        public OrderwithpaymentintentSpecification(string? paymentIntentId)
           : base(order => order.PaymentIntentId == paymentIntentId)
        {
          
        }
    }
}
