using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public string CustomerName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; }

        public string UserId { get; set; }

        public decimal TotalAmount { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    = new List<OrderDetail>();
        public string? Email { get; internal set; }
    }
}