using FoodOrderSystem.Data;
using FoodOrderSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FoodOrderSystem.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Add To Cart

        public IActionResult AddToCart(int id)
        {
            var food = _context.FoodItems.Find(id);

            List<Cart> cart;

            var sessionCart =
                HttpContext.Session.GetString("Cart");

            if (sessionCart == null)
            {
                cart = new List<Cart>();
            }
            else
            {
                cart = JsonConvert.DeserializeObject<List<Cart>>
                    (sessionCart);
            }

            cart.Add(new Cart
            {
                FoodId = food.FoodId,
                FoodName = food.FoodName,
                Price = food.Price,
                Quantity = 1
            });

            HttpContext.Session.SetString
            (
                "Cart",
                JsonConvert.SerializeObject(cart)
            );

            // Cart Count

            HttpContext.Session.SetInt32
            (
                "CartCount",
                cart.Count
            );

            return RedirectToAction("Index", "Home");
        }

        // Cart Page

        public IActionResult Index()
        {
            var sessionCart =
                HttpContext.Session.GetString("Cart");

            if (sessionCart == null)
            {
                return View(new List<Cart>());
            }

            var cart =
                JsonConvert.DeserializeObject<List<Cart>>
                (sessionCart);

            return View(cart);
        }

        // Remove Item

        public IActionResult Remove(int id)
        {
            var sessionCart =
                HttpContext.Session.GetString("Cart");

            var cart =
                JsonConvert.DeserializeObject<List<Cart>>
                (sessionCart);

            var item =
                cart.FirstOrDefault(x => x.FoodId == id);

            cart.Remove(item);

            HttpContext.Session.SetString
            (
                "Cart",
                JsonConvert.SerializeObject(cart)
            );

            // Update Cart Count

            HttpContext.Session.SetInt32
            (
                "CartCount",
                cart.Count
            );

            return RedirectToAction("Index");
        }
        //Increase quantity
       public IActionResult IncreaseQuantity(int id)
{
    var sessionCart =
        HttpContext.Session.GetString("Cart");

    var cart =
        JsonConvert.DeserializeObject<List<Cart>>
        (sessionCart);

    var item =
        cart.FirstOrDefault(x => x.FoodId == id);

    if (item != null)
    {
        item.Quantity++;
    }

    HttpContext.Session.SetString
    (
        "Cart",
        JsonConvert.SerializeObject(cart)
    );

    return RedirectToAction("Index");
}

        public IActionResult DecreaseQuantity(int id)
        {
            var sessionCart =
                HttpContext.Session.GetString("Cart");

            var cart =
                JsonConvert.DeserializeObject<List<Cart>>
                (sessionCart);

            var item =
                cart.FirstOrDefault(x => x.FoodId == id);

            if (item != null)
            {
                item.Quantity--;

                if (item.Quantity <= 0)
                {
                    cart.Remove(item);
                }
            }

            HttpContext.Session.SetString
            (
                "Cart",
                JsonConvert.SerializeObject(cart)
            );

            return RedirectToAction("Index");
        }






    }



}