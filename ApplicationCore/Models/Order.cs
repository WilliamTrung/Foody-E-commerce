using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Models
{
    public partial class Order : IsDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public string? Freight { get; set; }
        public string ShipAddress { get; set; } = null!;
        [Column(TypeName ="money")]
        public decimal TotalPrice { get; set; }
        public virtual Account? Account { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = null!;

        public static implicit operator List<object>(Order v)
        {
            throw new NotImplementedException();
        }
    }
}
