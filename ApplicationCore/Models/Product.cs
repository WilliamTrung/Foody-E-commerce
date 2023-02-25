using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Models
{
    public partial class Product : IsDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        [ForeignKey(nameof(Supplier))]
        public int? SupplierId { get; set; }
        [ForeignKey(nameof(Category))]
        public int? CategoryId { get; set; }
        public int QuantityPerUnit { get; set; }
        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }
        public string? ProductImage { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
