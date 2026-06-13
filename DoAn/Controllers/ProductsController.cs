using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAn.Models;

namespace DoAn.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString, long? categoryId, string sortOrder, int page = 1)
        {
            int pageSize = 8;

            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString));
            }

            if (categoryId != null)
            {
                products = products.Where(p => p.CategoryId == categoryId);
            }

            products = sortOrder switch
            {
                "price_asc" => products.OrderBy(p => p.BasePrice),
                "price_desc" => products.OrderByDescending(p => p.BasePrice),
                "name_asc" => products.OrderBy(p => p.Name),
                _ => products.OrderBy(p => p.Id)
            };

            int totalItems = products.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var result = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.SearchString = searchString;
            ViewBag.CategoryId = categoryId;
            ViewBag.SortOrder = sortOrder;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Categories = _context.Categories.ToList();

            return View(result);
        }

        public IActionResult Details(long id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            var recommendedProductIds =
                from a in _context.AssociationRuleAntecedents
                join r in _context.AssociationRules on a.RuleId equals r.Id
                join c in _context.AssociationRuleConsequents on r.Id equals c.RuleId
                where a.ProductId == id && r.IsActive == true
                orderby r.ConfidenceValue descending, r.LiftValue descending
                select c.ProductId;

            var recommendations = _context.Products
                .Where(p => recommendedProductIds.Distinct().Take(6).Contains(p.Id))
                .ToList();

            ViewBag.Recommendations = recommendations;

            return View(product);
        }
        public IActionResult Suggest(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Json(new List<object>());

            var suggestions = _context.Products
                .Where(p => p.Name.Contains(term))
                .OrderBy(p => p.Name)
                .Take(8)
                .Select(p => new
                {
                    id = p.Id,
                    name = p.Name,
                    price = p.BasePrice
                })
                .ToList();

            return Json(suggestions);
        }
    }
}