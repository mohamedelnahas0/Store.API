using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.Order
{
    public class OrderWithItemSpecification : 
        BaseSpecification<Data.Entities.OrderEntities.Order>
    {
        public OrderWithItemSpecification(string buyerEmail)
            :base(order => order.BuyerEmail==buyerEmail)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
            AddOrderByDescending(order => order.OrderDate);
        }
        public OrderWithItemSpecification(Guid id,string buyerEmail)
           : base(order => order.BuyerEmail == buyerEmail && order.Id == id)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
        }
    }
}
