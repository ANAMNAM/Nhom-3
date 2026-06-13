using DoAn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DoAn.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(username))
            {
                TempData["SuccessMessage"] = "Vui lòng đăng nhập trước khi thanh toán!";
                return RedirectToAction("Login", "Account");
            }

            var json = HttpContext.Session.GetString("cart");
            var cart = string.IsNullOrEmpty(json)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();

            if (!cart.Any())
                return RedirectToAction("Index", "Cart");

            return View(cart);
        }
        private readonly AppDbContext _context;

        public CheckoutController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Confirm()
        {
            var customerIdText =
                HttpContext.Session.GetString("CustomerId");

            if (string.IsNullOrEmpty(customerIdText))
            {
                TempData["LoginMessage"] =
                    "Vui lòng đăng nhập trước khi thanh toán.";

                return RedirectToAction("Login", "Account");
            }

            long customerId = long.Parse(customerIdText);

            var json = HttpContext.Session.GetString("cart");

            var cart = string.IsNullOrEmpty(json)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(json)
                    ?? new List<CartItem>();

            if (!cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var totalAmount =
                cart.Sum(x => x.Price * x.Quantity);

            var totalQuantity =
                cart.Sum(x => x.Quantity);

            var order = new Order
            {
                CustomerId = customerId,

                InvoiceCode =
                    "HD" + DateTime.Now.ToString("yyyyMMddHHmmss"),

                OrderDate = DateTime.Now,

                TotalQuantity = totalQuantity,

                TotalAmount = totalAmount,

                DiscountAmount = 0,

                FinalAmount = totalAmount,

                PaymentMethod = "COD",

                Status = "completed",

                CreatedAt = DateTime.Now,

                UpdatedAt = DateTime.Now
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in cart)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,

                    ProductId = item.ProductId,

                    Quantity = item.Quantity,

                    UnitPrice = item.Price,

                    DiscountAmount = 0,

                    TotalPrice =
                        item.Price * item.Quantity,

                    CreatedAt = DateTime.Now
                };

                _context.OrderItems.Add(orderItem);
            }

            _context.SaveChanges();

            HttpContext.Session.Remove("cart");

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}