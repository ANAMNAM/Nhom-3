using Microsoft.AspNetCore.Mvc;
using DoAn.Models;
using System.Text.Json;

namespace DoAn.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("AccountId") == null)
            {
                TempData["LoginMessage"] = "Bạn chưa đăng nhập. Vui lòng đăng nhập để tiếp tục.";
                TempData["ShowLoginModal"] = true;

                return RedirectToAction("Index", "Products");
            }
            var username = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(username))
            {
                TempData["LoginMessage"] = "Bạn chưa đăng nhập. Vui lòng đăng nhập để xem giỏ hàng.";
                return RedirectToAction("Login", "Account");
            }

            var cart = GetCart();

            var productIds = cart.Select(x => x.ProductId).ToList();

            var recommendedIds =
                from a in _context.AssociationRuleAntecedents
                join r in _context.AssociationRules on a.RuleId equals r.Id
                join c in _context.AssociationRuleConsequents on r.Id equals c.RuleId
                where productIds.Contains(a.ProductId) && r.IsActive == true
                orderby r.ConfidenceValue descending, r.LiftValue descending
                select c.ProductId;

            var recommendations = _context.Products
                .Where(p => recommendedIds.Distinct().Take(6).Contains(p.Id)
                            && !productIds.Contains(p.Id))
                .ToList();

            ViewBag.Recommendations = recommendations;

            return View(cart);
        }

        public IActionResult Add(long id)
        {
            if (HttpContext.Session.GetString("AccountId") == null)
            {
                TempData["LoginMessage"] = "Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng.";
                TempData["ShowLoginModal"] = true;

                return RedirectToAction("Index", "Products");
            }

            //Chặn mua hàng nếu chưa đăng nhập
            var username = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(username))
            {
                TempData["LoginMessage"] = "Bạn chưa đăng nhập. Vui lòng đăng nhập để mua sản phẩm.";
                return RedirectToAction("Login", "Account");
            }

            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item == null)
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.BasePrice ?? 0,
                    Quantity = 1,
                    ImageUrl = product.ImageUrl
                });
            }
            else
            {
                item.Quantity++;
            }

            SaveCart(cart);

            TempData["SuccessMessage"] = "Đã thêm sản phẩm vào giỏ hàng!";

            var referer = Request.Headers["Referer"].ToString();

            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }

            return RedirectToAction("Index", "Products");
        }
        public IActionResult Increase(long id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                item.Quantity++;
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Decrease(long id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                item.Quantity--;

                if (item.Quantity <= 0)
                    cart.Remove(item);

                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }
        public IActionResult Remove(long id)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        private List<CartItem> GetCart()
        {
            var json = HttpContext.Session.GetString("cart");

            if (string.IsNullOrEmpty(json))
                return new List<CartItem>();

            return JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            var json = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("cart", json);
        }
    }
}