using Microsoft.EntityFrameworkCore;
using WebTHEKING.Models; // Namespace chứa các model cốt lõi

namespace WebTHEKING.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor này cho phép Dependency Injection hoạt động.
        // Nó nhận các tùy chọn cấu hình (như chuỗi kết nối) từ file Program.cs.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Khai báo các bảng sẽ được tạo trong cơ sở dữ liệu.
        // Mỗi thuộc tính DbSet<T> tương ứng với một bảng.

        public DbSet<User> Users { get; set; }             // Bảng người dùng (Admin, Customer) 🧑‍💻
        public DbSet<Product> Products { get; set; }         // Bảng sản phẩm (gộp cả điện thoại và phụ kiện) 📱
        public DbSet<Order> Orders { get; set; }             // Bảng đơn hàng 🛒
        public DbSet<OrderDetail> OrderDetails { get; set; } // Bảng chi tiết đơn hàng 📄
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình cho các thuộc tính decimal trong Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Price).HasColumnType("decimal(18, 2)");
                entity.Property(p => p.OriginalPrice).HasColumnType("decimal(18, 2)");
            });

            // Cấu hình cho thuộc tính decimal trong Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(o => o.TotalAmount).HasColumnType("decimal(18, 2)");
            });

            // Cấu hình cho thuộc tính decimal trong OrderDetail
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(od => od.Price).HasColumnType("decimal(18, 2)");
            });
            modelBuilder.Entity<User>().HasData(
        new User
        {
            Id = 1,
            Username = "admin",
            // QUAN TRỌNG: Trong thực tế, không bao giờ lưu mật khẩu dạng text. 
            // Đây là mật khẩu đã được mã hóa. Mật khẩu gốc là "123".
            PasswordHash = "AQAAAAEAACcQAAAAE... (Đây là một chuỗi hash mẫu)",
            FullName = "Quản Trị Viên",
            Email = "admin@theking.com",
            Role = "Admin"
        },
        new User
        {
            Id = 2,
            Username = "customer",
            PasswordHash = "AQAAAAEAACcQAAAAE... (Đây là một chuỗi hash mẫu)",
            FullName = "Khách Hàng A",
            Email = "customer@email.com",
            Role = "Customer"
        }
    );
            modelBuilder.Entity<Product>().HasData(
        // Điện thoại
        new Product { Id = 1, Name = "Iphone 13", Description = "Hiệu năng ổn định", ImageUrl = "/img/9.jpg", Price = 12890000m, Category = "Apple", Type = WebTHEKING.Enums.ProductType.Phone },
        new Product { Id = 2, Name = "Iphone 14", Description = "Camera cải tiến", ImageUrl = "/img/19.jpg", Price = 13790000m, Category = "Apple", Type = WebTHEKING.Enums.ProductType.Phone },
        new Product { Id = 3, Name = "Iphone 15", Description = "Thiết kế mới với Dynamic Island", ImageUrl = "/img/10.jpg", Price = 15390000m, Category = "Apple", Type = WebTHEKING.Enums.ProductType.Phone },
        new Product { Id = 4, Name = "SamSung S25", Description = "Sức mạnh nhiếp ảnh đỉnh cao", ImageUrl = "/img/20.jpg", Price = 12500000m, Category = "Samsung", Type = WebTHEKING.Enums.ProductType.Phone },

        // Phụ kiện
        new Product { Id = 5, Name = "Airpods Pro 3", Description = "Chống ồn chủ động", ImageUrl = "/img/11.jpg", Price = 6790000m, Category = "tainghe", Type = WebTHEKING.Enums.ProductType.Accessory },
        new Product { Id = 6, Name = "AirPods Max USB C", Description = "Âm thanh Hi-Fi", ImageUrl = "/img/12.jpg", Price = 12990000m, OriginalPrice = 13790000m, Category = "tainghe", Type = WebTHEKING.Enums.ProductType.Accessory },
        new Product { Id = 7, Name = "Ốp lưng MagSafe JINYA", Description = "Bảo vệ toàn diện", ImageUrl = "/img/21.jpg", Price = 550000m, Category = "oplung", Type = WebTHEKING.Enums.ProductType.Accessory },
        new Product { Id = 8, Name = "Cáp Type C", Description = "Sạc nhanh an toàn", ImageUrl = "/img/26.jpg", Price = 200000m, OriginalPrice = 220000m, Category = "daysac", Type = WebTHEKING.Enums.ProductType.Accessory }
    );
        }

    }
}