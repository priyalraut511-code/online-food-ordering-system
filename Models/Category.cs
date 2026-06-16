using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

            public string CategoryName { get; set; }

            public ICollection<FoodItem>? FoodItems { get; set; }
        
    }
}



