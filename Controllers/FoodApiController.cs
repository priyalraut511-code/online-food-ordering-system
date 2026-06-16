using FoodOrderSystem.Data;
using FoodOrderSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FoodApiController
        (ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetFoods()
        {
            return Ok(_context.FoodItems.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetFood(int id)
        {
            var food =
                _context.FoodItems.Find(id);

            return Ok(food);
        }

        [HttpPost]
        public IActionResult AddFood(FoodItem food)
        {
            _context.FoodItems.Add(food);

            _context.SaveChanges();

            return Ok(food);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFood
        (int id, FoodItem food)
        {
            _context.FoodItems.Update(food);

            _context.SaveChanges();

            return Ok(food);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFood(int id)
        {
            var food =
                _context.FoodItems.Find(id);

            _context.FoodItems.Remove(food);

            _context.SaveChanges();

            return Ok();
        }
    }

}

