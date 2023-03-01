using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ApplicationCore;
using ApplicationCore.Models;
using BusinessService.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using FoodyWebApplication.Helper;
using FoodyWebApplication.Models;

namespace FoodyWebApplication.Controllers
{
    [Authorize(Roles = "Administrator,Member,Suplier")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
       
        public decimal Total { get; set; } = 0;
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public CartModel Carts
        {
            get
            {
                var data = HttpContext.Session.Get<CartModel>("GioHang");
                if(data == null)
                {
                   data = new CartModel();
                }
                return data;
            }
        }
        public IActionResult Index()
        {
            CartModel cart = SessionExtension.GetCart(HttpContext.Session);
            foreach (var product in cart.ProductList)
            {
                Total += (product.QuantityPerUnit * product.UnitPrice);
                product.UnitPrice = Math.Round(product.UnitPrice, 0);
            }
            Total = Math.Round(Total, 0);
            cart.Total = Total;
            HttpContext.Session.Set<string>("GioHangTotal", cart.Total.ToString());
            return View("Index");

        }
        public async Task<IActionResult> AddToCartAsync(int id,int quantity) 
        {
            CartModel cart = Carts;
            var product = await _unitOfWork.ProductService.GetFirst(p => p.ProductId == id, "Category", "Supplier");
            if(product != null)
            {
                AddToCart(cart,product, quantity);
            }   
            HttpContext.Session.Set("GioHang", cart);
            return RedirectToAction("Index");
        }  
        public IActionResult UpdateCart(int? remove, int? update, int?  quantity)
        {
            if (remove == null && update != null)
            {
                //update
                var cart = SessionExtension.GetCart(HttpContext.Session);
                //adjust quantity
                var find = cart.ProductList.FirstOrDefault(p => p.ProductId == update);
                if (find != null)
                {
                    find.QuantityPerUnit = (int)quantity;
                }
                HttpContext.Session.Set("GioHang", cart);
            }
            else if (update == null && remove != null)
            {
                var cart = SessionExtension.GetCart(HttpContext.Session);
                var products = cart.ProductList.Where(p => p.ProductId == remove);
                var product = products.First();
                cart.ProductList.Remove(product);
                HttpContext.Session.Set("GioHang", cart);
            }
            else
            {
                //check out
                var cart = SessionExtension.GetCart(HttpContext.Session);
                if (cart != null  && cart.ProductList != null && cart.ProductList.Count() > 0)
                {
                   // var product = await _unitOfWork.ProductService.GetFirst(p => p.ProductId == id, "Category", "Supplier");
                    cart = new CartModel();
                    HttpContext.Session.Set("GioHang", cart);
                    return RedirectToPage("/Customer/Orders/Index");
                }
                //ErrorMessage = "Cart is empty!";
            }
            return Index();
        }
        private async void AddToCart(CartModel cart, Product product, int quantity)
        {
                if (quantity > 0) // chưa có
            {
                var find = cart.ProductList.FirstOrDefault(p => p.ProductId == product.ProductId);
                //check quantity of product vs max quantity in product_check
                bool checkQuantity = product.QuantityPerUnit < quantity;
                //end check
                if (checkQuantity)
                {
                    if (find != null)
                    {
                        find.QuantityPerUnit += quantity;
                    }
                    else
                    {
                        product.QuantityPerUnit = quantity;
                        cart.ProductList.Add(product);
                    }
                }
                
            }




        }

        }
    }

