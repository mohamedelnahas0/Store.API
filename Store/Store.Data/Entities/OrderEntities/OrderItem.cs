namespace Store.Data.Entities.OrderEntities
{
    public class OrderItem : BaseEntity<Guid>
    {
        public decimal price { get; set; }
        public int Quantity { get; set; }
        public ProductItemOrder ItemOrder { get; set; }
    }
}