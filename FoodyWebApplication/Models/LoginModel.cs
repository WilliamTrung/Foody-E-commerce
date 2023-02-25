using System.ComponentModel.DataAnnotations;

namespace FoodyWebApplication.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        public bool RememberLogin { get; set; } = false;
        public string ReturnUrl { get; set; } = null!;
    }
}
