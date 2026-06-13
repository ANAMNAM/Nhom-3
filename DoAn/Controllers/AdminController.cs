using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAn.Models;

namespace DoAn.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            ViewBag.TotalOrders = _context.Orders.Count();

            ViewBag.TotalRevenue = _context.Orders
                .Sum(o => o.FinalAmount);

            ViewBag.BestSellingProducts = _context.OrderItems
                .Include(oi => oi.Product)
                .GroupBy(oi => new
                {
                    oi.ProductId,
                    oi.Product.Name
                })
                .Select(g => new
                {
                    ProductName = g.Key.Name,
                    TotalQuantity = g.Sum(x => x.Quantity),
                    TotalRevenue = g.Sum(x => x.TotalPrice)
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(5)
                .ToList();

            var orders = _context.Orders
                .Include(o => o.Customer)
                .OrderByDescending(o => o.OrderDate)
                .Take(20)
                .ToList();
            ViewBag.AprioriRules = _context.AssociationRules
                .Include(r => r.AssociationRuleAntecedents)
                    .ThenInclude(a => a.Product)
                .Include(r => r.AssociationRuleConsequents)
                    .ThenInclude(c => c.Product)
                .OrderByDescending(r => r.ConfidenceValue)
                .ThenByDescending(r => r.LiftValue)
                .Take(10)
                .ToList();
            return View(orders);
        }
    }
}