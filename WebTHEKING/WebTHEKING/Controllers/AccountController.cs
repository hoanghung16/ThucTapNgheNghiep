using Microsoft.AspNetCore.Identity; // Cần cho việc mã hóa mật khẩu
using Microsoft.AspNetCore.Mvc;
using System.Linq; // Cần cho .Any()
using WebTHEKING.Data;
using WebTHEKING.Models;
using WebTHEKING.ViewModels;
using Microsoft.AspNetCore.Authentication; 
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace WebTHEKING.Controllers

{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // [GET] /Account/Register
        // Hiển thị form đăng ký
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            // Kiểm tra xem dữ liệu người dùng nhập có hợp lệ không (dựa trên các [Required], [Compare]...)
            if (ModelState.IsValid)
            {
                // Kiểm tra xem Username hoặc Email đã tồn tại trong database chưa
                if (_context.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập này đã tồn tại.");
                    return View(model);
                }
                if (_context.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email này đã được sử dụng.");
                    return View(model);
                }

                // Mã hóa mật khẩu trước khi lưu
                var passwordHasher = new PasswordHasher<User>();
                var user = new User
                {
                    Username = model.Username,
                    FullName = model.FullName,
                    Email = model.Email,
                    Role = "Customer", // Mặc định người dùng mới là "Customer"
                    // Mã hóa mật khẩu người dùng nhập và lưu vào DB
                    PasswordHash = passwordHasher.HashPassword(null, model.Password)
                };

                // Thêm người dùng mới vào DbContext và lưu thay đổi
                _context.Users.Add(user);
                _context.SaveChanges();

                // Chuyển hướng người dùng đến trang đăng nhập sau khi đăng ký thành công
                return RedirectToAction("Login", "Account");
            }

            // Nếu dữ liệu không hợp lệ, hiển thị lại form với các lỗi
            return View(model);
        }
        // [GET] /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // [POST] /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1. Tìm người dùng trong database dựa trên Username
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);

                // 2. Nếu tìm thấy người dùng
                if (user != null)
                {
                    var passwordHasher = new PasswordHasher<User>();
                    // 3. Kiểm tra mật khẩu người dùng nhập với mật khẩu đã mã hóa trong DB
                    var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

                    // 4. Nếu mật khẩu chính xác
                    if (result == PasswordVerificationResult.Success)
                    {
                        // === TẠO PHIÊN ĐĂNG NHẬP (COOKIE) ===
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim("FullName", user.FullName),
                    new Claim(ClaimTypes.Role, user.Role) // Lưu vai trò (Role)
                };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            // Tùy chọn: cho phép "ghi nhớ đăng nhập"
                            // IsPersistent = true
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        // Chuyển hướng về trang chủ sau khi đăng nhập thành công
                        // Kiểm tra vai trò của người dùng
                        if (user.Role == "Admin")
                        {
                            // Nếu là Admin, chuyển hướng đến trang Dashboard trong khu vực Admin
                            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                        }
                        else
                        {
                            // Nếu là các vai trò khác (ví dụ: Customer), chuyển hướng về trang chủ
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }

                // 5. Nếu username không tồn tại hoặc mật khẩu sai
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác.");
            }

            // Nếu có lỗi, hiển thị lại form đăng nhập
            return View(model);
        }
        //Đăng xuất
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Xóa cookie xác thực
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // Chuyển về trang chủ
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            // Lấy username của người dùng đang đăng nhập từ cookie
            var username = User.Identity.Name;

            // Tìm thông tin người dùng trong database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            // Nếu không tìm thấy người dùng (trường hợp hiếm), chuyển về trang chủ
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Gửi đối tượng user đến View
            return View(user);
        }
    }
}
