using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebTHEKING.Data;
using WebTHEKING.Enums;
using WebTHEKING.Models;

namespace WebTHEKING.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Detail(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy sản phẩm
            }
            return View(product); // Gửi đối tượng product đến View
        }
        public IActionResult Sale()
        {
            // Logic để lấy danh sách sản phẩm đang sale
            var saleProducts = _context.Products.Where(p => p.Id <= 4).ToList(); // Ví dụ
            ViewData["Title"] = "Sản Phẩm Khuyến Mãi";
            return View(saleProducts);
        }
        public IActionResult Accessories()
        {
            var accessories = _context.Products
                                 .Where(p => p.Type == ProductType.Accessory)
                                 .ToList();

            // Gửi danh sách đã lọc đến View
            return View(accessories);
        }
    }
}
