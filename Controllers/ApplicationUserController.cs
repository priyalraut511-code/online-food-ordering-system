using FoodOrderSystem.Data;
using FoodOrderSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderSystem.Controllers
{
    [Authorize]
    public class ApplicationUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ApplicationUserController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // =========================
        // ALL USERS
        // =========================
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var currentUser =
                await _userManager.GetUserAsync(User);

            if (currentUser.Role != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var users = await _context.Users.ToListAsync();

            return View(users);
        }

        // =========================
        // USER DETAILS
        // =========================
        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // =========================
        // MAKE ADMIN
        // =========================
        [Authorize]
        public async Task<IActionResult> MakeAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Role = "Admin";

            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // REMOVE ADMIN
        // =========================
        [Authorize]
        public async Task<IActionResult> RemoveAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Role = "User";

            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DELETE USER
        // =========================
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }
    }
}