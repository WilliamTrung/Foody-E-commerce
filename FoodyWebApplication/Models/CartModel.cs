using ApplicationCore.Models;
using System.ComponentModel.DataAnnotations;
namespace FoodyWebApplication.Models
{
    public class CartModel
    {
        public CartModel()
        {
            ProductList = new List<Product>();
        }
        
        public int AccountId { get; set; }
        public decimal Total { get; set; }
        public List<Product> ProductList { get; set; }
        
    }
}
