using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;       
using WebTHEKING.Data;

namespace WebTHEKING.Areas.Admin.Controllers
{
    [Area("Admin")] // Đảm bảo Controller thuộc khu vực Admin
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        // Biến private để giữ tham chiếu đến database context
        private readonly ApplicationDbContext _context;

        // Constructor này nhận ApplicationDbContext thông qua Dependency Injection
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Action sẽ được chạy khi bạn truy cập /Admin/Dashboard
        public IActionResult Index()
        {
            // Lấy danh sách đơn hàng từ database
            // .Include(o => o.User) sẽ lấy kèm thông tin của người dùng đã đặt hàng
            // .OrderByDescending(o => o.OrderDate) để xếp các đơn hàng mới nhất lên đầu
            var orders = _context.Orders
                                 .Include(o => o.User)
                                 .OrderByDescending(o => o.OrderDate)
                                 .ToList();

            // Gửi danh sách đơn hàng đến View để hiển thị
            return View(orders);
        }
    }
}