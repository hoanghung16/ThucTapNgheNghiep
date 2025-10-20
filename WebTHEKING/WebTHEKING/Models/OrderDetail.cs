using System.ComponentModel.DataAnnotations.Schema;

namespace WebTHEKING.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } // Giá sản phẩm tại thời điểm mua

        // Foreign Key đến bảng Orders
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        // Foreign Key đến bảng Products
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}