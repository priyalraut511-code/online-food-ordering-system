using FoodOrderSystem.Data;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            ViewBag.TotalOrders = _context.Orders.Count();
            ViewBag.TotalFoods = _context.FoodItems.Count();
            ViewBag.TotalCategories = _context.Categories.Count();

            return View();
        }
    }
}
