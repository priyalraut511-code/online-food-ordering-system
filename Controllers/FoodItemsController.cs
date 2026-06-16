using FoodOrderSystem.Data;
using FoodOrderSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderSystem.Controllers
{
    public class FoodItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FoodItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // ALL FOODS
        // =========================
        public async Task<IActionResult> Index()
        {
            return View(await _context.FoodItems.ToListAsync());
        }

        // =========================
        // CREATE GET
        // =========================
        public IActionResult Create()
        {
            ViewBag.Categories =
    _context.Categories.ToList();
            return View();
        }

        // =========================
        // CREATE POST
        // =========================
        [HttpPost]
        public async Task<IActionResult> Create(
            FoodItem foodItem,
            IFormFile ImageFile)
        {
            if (ImageFile != null)
            {
                string fileName =
                    Guid.NewGuid().ToString()
                    + Path.GetExtension(ImageFile.FileName);

                string imagePath =
                    Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/images",
                        fileName);

                using (var stream =
                    new FileStream(imagePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                foodItem.ImageUrl =
                    "/images/" + fileName;
            }

            _context.FoodItems.Add(foodItem);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // EDIT GET
        // =========================
        public async Task<IActionResult> Edit(int id)
        {
            var food =
                await _context.FoodItems.FindAsync(id);

            if (food == null)
            {
                return NotFound();
            }

            ViewBag.Categories =
                _context.Categories.ToList();

            return View(food);
        }

        // =========================
        // EDIT POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
     int id,
     FoodItem foodItem,
     IFormFile ImageFile)
        {
            if (id != foodItem.FoodId)
            {
                return NotFound();
            }

            if (ImageFile != null)
            {
                string fileName =
                    Guid.NewGuid().ToString()
                    + Path.GetExtension(ImageFile.FileName);

                string imagePath =
                    Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/images",
                        fileName);

                using (var stream =
                    new FileStream(imagePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                foodItem.ImageUrl =
                    "/images/" + fileName;
            }

            _context.Update(foodItem);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        // =========================
        // DELETE GET
        // =========================
        public async Task<IActionResult> Delete(int id)
        {
            var food =
                await _context.FoodItems.FindAsync(id);

            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // =========================
        // DELETE POST
        // =========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(
            FoodItem foodItem)
        {
            var food =
                await _context.FoodItems
                .FindAsync(foodItem.FoodId);

            if (food != null)
            {
                _context.FoodItems.Remove(food);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}