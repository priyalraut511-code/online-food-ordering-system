using FoodOrderSystem.Data;
using FoodOrderSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using FoodOrderSystem.Data;
using FoodOrderSystem.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrderController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [Authorize]
    [HttpGet]
    public IActionResult Checkout()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public IActionResult Checkout(Order order)
    {
        if (order == null)
        {
            return Content("Order is Null");
        }

        // Get Cart From Session

        var sessionCart =
            HttpContext.Session.GetString("Cart");

        if (string.IsNullOrEmpty(sessionCart))
        {
            return Content("Cart is Empty");
        }

        // Convert Session To List

        var cart =
            JsonConvert.DeserializeObject<List<Cart>>(sessionCart);

        // Save Order

        order.OrderDate = DateTime.Now;

        order.Status = "Pending";

        order.TotalAmount =
            cart.Sum(x => x.Price * x.Quantity);

        // Save Logged In User Details
        var user = _userManager.GetUserAsync(User).Result;

        order.CustomerName = user.FullName;
        order.Address = user.Address;
        order.Phone = user.PhoneNumber;

        order.Email = user.Email;
        order.UserId = user.Id;

        _context.Orders.Add(order);

        _context.SaveChanges();

        // Save Order Details
        foreach (var cartItem in cart)
        {
            var orderDetail = new OrderDetail
            {
                OrderId = order.OrderId,
                FoodId = cartItem.FoodId,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price
            };

            _context.OrderDetails.Add(orderDetail);
        }

        _context.SaveChanges();


        // Clear Cart Completely

        HttpContext.Session.Remove("Cart");
        HttpContext.Session.Remove("CartCount");

        // Redirect

        return RedirectToAction("Success");
    }
    public IActionResult Success()
    {
        return View();
    }
  
    [HttpGet]
    public JsonResult CheckPaymentStatus()
    {
        // Real PhonePe API verification logic here

        bool paymentSuccess = true;

        return Json(new
        {
            success = paymentSuccess
        });
    }

    
    [Authorize]
    public IActionResult MyOrders()
    {
        var email = User.Identity.Name;

        var orders = _context.Orders
                             .Include(x => x.OrderDetails)
                             .Where(x => x.Email == email)
                             .ToList();

        ViewBag.FoodItems = _context.FoodItems.ToList();

        return View(orders);
    }

    public IActionResult Index(string search, string status)
    {
        var orders = _context.Orders
            .Include(o => o.OrderDetails)
            .OrderByDescending(o => o.OrderDate)
            .ToList();

        // Customer Names for Search Suggestions
        ViewBag.CustomerNames = _context.Orders
            .Select(x => x.CustomerName)
            .Distinct()
            .ToList();

        // Search by Customer Name
        if (!string.IsNullOrEmpty(search))
        {
            orders = orders
                .Where(x => x.CustomerName.Contains(search))
                .ToList();
        }

        // Filter by Status
        if (!string.IsNullOrEmpty(status))
        {
            orders = orders
                .Where(x => x.Status == status)
                .ToList();
        }

        ViewBag.FoodItems = _context.FoodItems.ToList();

        return View(orders);
    }
    [HttpPost]
    public IActionResult UpdateStatus(int id, string status)
    {
        var order = _context.Orders.FirstOrDefault(x => x.OrderId == id);

        if (order != null)
        {
            order.Status = status;
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }
}