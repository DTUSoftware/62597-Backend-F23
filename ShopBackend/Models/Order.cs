using System.ComponentModel.DataAnnotations;
namespace ShopBackend.Models

{
    public class Order
    {
        [Key]
        public required string id { get;}
        [Required(ErrorMessage = "Total cannot be empty")]
        public required int total { get; set; }
        [Required(ErrorMessage = "Must have a list of products"]
        public List<Product> products;


    }
}