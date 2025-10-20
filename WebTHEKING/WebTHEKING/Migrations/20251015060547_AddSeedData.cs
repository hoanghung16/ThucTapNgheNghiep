using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebTHEKING.Migrations
{
    public partial class AddSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "ImageUrl", "Name", "OriginalPrice", "Price", "Type" },
                values: new object[,]
                {
                    { 1, "Apple", "Hiệu năng ổn định", "/img/9.jpg", "Iphone 13", null, 12890000m, 1 },
                    { 2, "Apple", "Camera cải tiến", "/img/19.jpg", "Iphone 14", null, 13790000m, 1 },
                    { 3, "Apple", "Thiết kế mới với Dynamic Island", "/img/10.jpg", "Iphone 15", null, 15390000m, 1 },
                    { 4, "Samsung", "Sức mạnh nhiếp ảnh đỉnh cao", "/img/20.jpg", "SamSung S25", null, 12500000m, 1 },
                    { 5, "tainghe", "Chống ồn chủ động", "/img/11.jpg", "Airpods Pro 3", null, 6790000m, 2 },
                    { 6, "tainghe", "Âm thanh Hi-Fi", "/img/12.jpg", "AirPods Max USB C", 13790000m, 12990000m, 2 },
                    { 7, "oplung", "Bảo vệ toàn diện", "/img/21.jpg", "Ốp lưng MagSafe JINYA", null, 550000m, 2 },
                    { 8, "daysac", "Sạc nhanh an toàn", "/img/26.jpg", "Cáp Type C", 220000m, 200000m, 2 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FullName", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "admin@theking.com", "Quản Trị Viên", "AQAAAAEAACcQAAAAE... (Đây là một chuỗi hash mẫu)", "Admin", "admin" },
                    { 2, "customer@email.com", "Khách Hàng A", "AQAAAAEAACcQAAAAE... (Đây là một chuỗi hash mẫu)", "Customer", "customer" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
