using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace WebApplication3.ViewModels
{
    public class MembershipRegister
    {
        [Required, RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Full Name should only contain letters")]
        [DataType(DataType.Text)]
        public string FullName { get; set; } = string.Empty;

        [Required, RegularExpression(@"^[0-9]*$", ErrorMessage = "Mobile number should only contain 16 numbers"), MaxLength(16)]
        [DataType(DataType.CreditCard)]
        public string CreditCard { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Text)]
        public string Gender { get; set; }

        [Required, RegularExpression(@"^[0-9]*$", ErrorMessage = "Mobile number should only contain less than 10 numbers"), MaxLength(9)]
        public string MobileNumber { get; set; }

        [Required, RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Delivery address should only contain letters and numbers")]
        public string DeliveryAddress { get; set; } = string.Empty;

        [Required, RegularExpression(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$", ErrorMessage = "Email address must be in correct format. example@mail.com")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string Photo { get; set; } = "/uploads/user.jpg";
        public string AboutMe { get; set; } = string.Empty;
    }
}
