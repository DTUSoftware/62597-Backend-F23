using System.ComponentModel.DataAnnotations;

namespace ShopBackend.Models
{
    public class Customer
    {        
        public string FirstName { get; set; } 
        public string LastName { get; set; }       
        public string Password { get; set; } = null!;
        [Key]
        public string Email { get; set; } = null!;
        public int Phone { get; set; }

        public ICollection <Address> Address { get; set; }=null!;        
        public ICollection<Order> Orders { get; set; } = null!;
    }
}
