using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using WebTHEKING.Data;
using WebTHEKING.Models;
using System.Linq;
using System.Threading.Tasks;

namespace WebTHEKING.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // Chỉ tài khoản có Role là "Admin" mới truy cập được
    public class ProductAdminController : Controller
    {
        // "Tiêm" DbContext để có thể làm việc với cơ sở dữ liệu
        private readonly ApplicationDbContext _context;

        public ProductAdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        // Action này lấy dữ liệu từ CSDL và trả về View
        public async Task<IActionResult> Index()
        {
            // Truy vấn vào bảng Products, sắp xếp theo ID mới nhất và chuyển thành một List
            var products = await _context.Products.OrderByDescending(p => p.Id).ToListAsync();

            // Gửi danh sách sản phẩm đã lấy được đến View để hiển thị
            return View(products);
        }
        //Thêm sản phẩm
        public IActionResult Create()
        {
            return View();
        }
        // Action này sẽ được gọi khi người dùng nhấn nút "Tạo mới" trên form.
        [HttpPost]
        [ValidateAntiForgeryToken] // Bảo mật chống tấn công CSRF
        public async Task<IActionResult> Create(Product product)
        {
            // Kiểm tra xem dữ liệu người dùng gửi lên có hợp lệ không (dựa trên các quy tắc trong Model)
            if (ModelState.IsValid)
            {
                _context.Add(product); // Thêm sản phẩm mới vào DbContext
                await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
                return RedirectToAction(nameof(Index)); // Chuyển hướng về trang danh sách sản phẩm
            }

            // Nếu dữ liệu không hợp lệ, hiển thị lại form với các lỗi
            return View(product);
        }
        //Xoá sản phẩm
        // Action này tìm sản phẩm và hiển thị trang xác nhận.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        // Action này được gọi khi người dùng nhấn nút "Xóa" trên trang xác nhận.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product); // Xóa sản phẩm khỏi DbContext
                await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
            }

            return RedirectToAction(nameof(Index)); // Chuyển hướng về trang danh sách
        }
        //Sửa sản phẩm
        //Get lấy thông tin sản phẩm và hiện form chỉnh sửa
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        //Post nhận dữ liệu và thay đổi trong database
        // Action này được gọi khi người dùng nhấn nút "Lưu thay đổi" trên form.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product); // Đánh dấu đối tượng là đã bị thay đổi
                    await _context.SaveChangesAsync(); // Lưu thay đổi vào database
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Xử lý trường hợp sản phẩm đã bị xóa bởi người khác trong lúc mình đang sửa
                    if (!_context.Products.Any(e => e.Id == product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index)); // Chuyển hướng về trang danh sách
            }
            return View(product);
        }

    }
}