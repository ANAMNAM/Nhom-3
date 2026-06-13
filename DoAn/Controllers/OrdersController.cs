using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAn.Models;

namespace DoAn.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult History()
        {
            var customerIdText =
                HttpContext.Session.GetString("CustomerId");

            if (string.IsNullOrEmpty(customerIdText))
            {
                TempData["LoginMessage"] =
                    "Vui lòng đăng nhập để xem lịch sử mua hàng.";

                return RedirectToAction(
                    "Login",
                    "Account");
            }

            long customerId =
                long.Parse(customerIdText);

            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }
        public IActionResult Details(long id)
        {
            var customerIdText = HttpContext.Session.GetString("CustomerId");

            if (string.IsNullOrEmpty(customerIdText))
            {
                TempData["LoginMessage"] = "Vui lòng đăng nhập để xem chi tiết đơn hàng.";
                TempData["ShowLoginModal"] = true;
                return RedirectToAction("Index", "Products");
            }

            long customerId = long.Parse(customerIdText);

            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.Id == id && o.CustomerId == customerId);

            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}