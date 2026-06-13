using Microsoft.AspNetCore.Mvc;
using DoAn.Models;
using Microsoft.EntityFrameworkCore;
namespace DoAn.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var account = _context.Accounts
                .FirstOrDefault(x => x.Username == username && x.Password == password);

            if (account == null)
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
                return View();
            }

            HttpContext.Session.SetString("UserName", account.Username);
            HttpContext.Session.SetString("FullName", account.FullName);
            HttpContext.Session.SetString("AccountId", account.Id.ToString());
            if (account.CustomerId != null)
            {
                HttpContext.Session.SetString("CustomerId", account.CustomerId?.ToString() ?? "");
            }
            if (account.CustomerId == null)
            {
                ViewBag.Error = "Tài khoản chưa liên kết khách hàng.";
                return View();
            }

            HttpContext.Session.SetString("CustomerId", account.CustomerId.Value.ToString());
            TempData["SuccessMessage"] = "Đăng nhập thành công!";
            return RedirectToAction("Index", "Products");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(
            string fullName,
            string phone,
            string email,
            string address,
            string username,
            string password)
        {
            var usernameExists = _context.Accounts.Any(x => x.Username == username);

            if (usernameExists)
            {
                ViewBag.Error = "Tên tài khoản đã tồn tại";
                return View();
            }

            var phoneExists = _context.Customers.Any(x => x.Phone == phone)
                           || _context.Accounts.Any(x => x.Phone == phone);

            if (phoneExists)
            {
                ViewBag.Error = "Số điện thoại đã được sử dụng";
                return View();
            }
            var emailExists = _context.Customers.Any(x => x.Email == email);

            if (emailExists)
            {
                ViewBag.Error = "Email đã được sử dụng";
                return View();
            }
            var lastCustomer = _context.Customers
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            int nextNumber = 1;

            if (lastCustomer != null && !string.IsNullOrEmpty(lastCustomer.CustomerCode))
            {
                var numberText = lastCustomer.CustomerCode.Replace("KH", "");

                if (int.TryParse(numberText, out int currentNumber))
                {
                    nextNumber = currentNumber + 1;
                }
                else
                {
                    nextNumber = (int)lastCustomer.Id + 1;
                }
            }

            var customerCode = "KH" + nextNumber.ToString("000");

            var customer = new Customer
            {
                CustomerCode = customerCode,
                FullName = fullName,
                Phone = phone,
                Email = email,
                Gender = "other",
                Address = address,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();

            var account = new Account
            {
                FullName = fullName,
                Phone = phone,
                Address = address,
                Username = username,
                Password = password,
                CustomerId = customer.Id,
                CreatedAt = DateTime.Now
            };

            _context.Accounts.Add(account);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Tạo tài khoản thành công, vui lòng đăng nhập!";
            TempData["ShowLoginModal"] = true;

            return RedirectToAction("Index", "Products");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Products");
        }
        //profile khách hàng
        public IActionResult Profile()
        {
            var accountId = HttpContext.Session.GetString("AccountId");

            if (string.IsNullOrEmpty(accountId))
            {
                TempData["LoginMessage"] = "Vui lòng đăng nhập để xem thông tin khách hàng.";
                return RedirectToAction("Login");
            }

            var account = _context.Accounts
                .Include(x => x.Customer)
                .FirstOrDefault(x => x.Id == long.Parse(accountId));

            if (account == null)
                return NotFound();

            return View(account);
        }
        public IActionResult EditProfile()
        {
            var accountId = HttpContext.Session.GetString("AccountId");

            if (string.IsNullOrEmpty(accountId))
                return RedirectToAction("Login");

            var account = _context.Accounts
                .FirstOrDefault(x => x.Id == long.Parse(accountId));

            if (account == null)
                return NotFound();

            return View(account);
        }

        [HttpPost]
        public IActionResult EditProfile(
    long id,
    string fullName,
    string phone,
    string email,
    string address,
    string password)
        {
            var accountIdText = HttpContext.Session.GetString("AccountId");

            if (string.IsNullOrEmpty(accountIdText))
                return RedirectToAction("Login");

            long accountId = long.Parse(accountIdText);

            if (id != accountId)
                return RedirectToAction("Login");

            var account = _context.Accounts
                .Include(x => x.Customer)
                .FirstOrDefault(x => x.Id == accountId);

            if (account == null)
                return NotFound();

            account.FullName = fullName;
            account.Phone = phone;
            account.Address = address;

            if (account.Customer != null)
            {
                account.Customer.FullName = fullName;
                account.Customer.Phone = phone;
                account.Customer.Email = email;
                account.Customer.Address = address;
                account.Customer.UpdatedAt = DateTime.Now;
            }

            if (!string.IsNullOrWhiteSpace(password))
            {
                account.Password = password;
            }

            _context.SaveChanges();

            HttpContext.Session.SetString("FullName", account.FullName);

            TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("Profile");
        }
    }
}