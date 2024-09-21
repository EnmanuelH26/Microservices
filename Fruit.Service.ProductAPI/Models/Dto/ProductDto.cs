using System.ComponentModel.DataAnnotations;

namespace Fruit.Service.ProductAPI.Models.Dto
{
    public class ProductDto
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryName { get; set; }
    }
}
