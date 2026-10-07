using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Concepcion_Midterm_Store.Data;

namespace Concepcion_Midterm_Store.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // READ CART
        public async Task<IActionResult> Index()
        {
            var cartItems = await _context.CartItems.ToListAsync();

            ViewBag.Total = cartItems.Sum(item =>
                item.Price * item.Quantity);

            return View(cartItems);
        }

        // UPDATE QUANTITY
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(
            int id,
            int quantity)
        {
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem == null)
            {
                return NotFound();
            }

            if (quantity < 1)
            {
                quantity = 1;
            }

            cartItem.Quantity = quantity;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // REMOVE ITEM
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}