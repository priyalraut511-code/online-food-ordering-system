using FoodOrderSystem.Data;
using FoodOrderSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string category)
        {
            // Get all food items with category
            var foodItems = _context.FoodItems
                .Include(f => f.Category)
                .AsQueryable();

            // Category Filter
            if (!string.IsNullOrEmpty(category))
            {
                foodItems = foodItems.Where(f =>
                    f.Category.CategoryName == category);
            }

            // Send Categories To View
            ViewBag.Categories = _context.Categories.ToList();

            // Store Selected Category
            ViewBag.Search = category;

            return View(foodItems.ToList());
        }
    }
}