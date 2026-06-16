using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        public int FoodId { get; set; }

        public string FoodName { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string UserId { get; set; }
    }
}