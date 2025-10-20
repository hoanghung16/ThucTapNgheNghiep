using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebTHEKING.Data;             // Thêm để dùng ApplicationDbContext
using WebTHEKING.Enums;             // Thêm để dùng ProductType
using WebTHEKING.Models;           // Thêm để dùng Product
using WebTHEKING.ViewModels;       // Thêm để dùng ErrorViewModel

namespace WebTHEKING.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        // "Tiêm" DbContext vào controller thông qua constructor
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Lấy 4 sản phẩm thuộc loại "Phone" để làm sản phẩm bán chạy
            var bestSellingProducts = _context.Products
                                              .Where(p => p.Type == ProductType.Phone)
                                              .Take(4)
                                              .ToList();

            // Gửi danh sách sản phẩm này đến View
            return View(bestSellingProducts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}