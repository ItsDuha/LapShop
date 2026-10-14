using System.ComponentModel.DataAnnotations;

namespace LapShop.Models
{
    public class VmCheckout
    {
        // Customer Information

        [Required(ErrorMessage = "Please enter First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter Phone Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please enter Address")]
        public string Address { get; set; }

        public string? Notes { get; set; }

        // Cart
        public ShoopingCart Cart { get; set; } = new ShoopingCart();
    }
}