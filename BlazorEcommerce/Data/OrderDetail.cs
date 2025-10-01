using System.ComponentModel.DataAnnotations;

namespace BlazorEcommerce.Data
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }
        public OrderHeader OrderHeader { get; set; }// Navigation property to OrderHeader
        public int ProductId { get; set; }
        public Product Product { get; set; }// Navigation property to Product
        [Required]
        public int Count { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string ProductName { get; set; }
    }
}
