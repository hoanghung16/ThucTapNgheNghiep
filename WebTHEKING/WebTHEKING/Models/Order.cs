using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTHEKING.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }

        // Foreign Key đến bảng Users
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; } // Navigation property

        // Một đơn hàng có nhiều chi tiết đơn hàng
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}