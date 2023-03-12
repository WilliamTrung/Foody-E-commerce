using ApplicationCore.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodyWebApplication.Models
{
    public class RegisterModel
    {
        [Required]
        public string Username { get; set; } = null!;
        public int RoleId { get; set; }

        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
        [Phone]
        public string Phone { get; set; } = null!;
    }
}
