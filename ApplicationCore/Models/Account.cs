using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Models
{
    public partial class Account : IsDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }
        [Required]
        public string Username { get; set; } = null!;
        [ForeignKey(nameof(Role))]
        public int RoleId { get; set; }

        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
        [Phone]
        public string Phone { get; set; } = null!;
        public virtual Role? Role { get; set; } = null!;

        public virtual ICollection<Order>? Orders { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
