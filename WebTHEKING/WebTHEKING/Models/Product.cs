using WebTHEKING.Enums;
namespace WebTHEKING.Models
{
    public class Product
    {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string ImageUrl { get; set; }
            public decimal Price { get; set; }
            public decimal? OriginalPrice { get; set; } // Giá gốc, có thể null
            public string Category { get; set; } // Dùng cho phụ kiện: "tainghe", "oplung"...

            // Trường phân loại (Discriminator)
            public ProductType Type { get; set; }
    }
}
