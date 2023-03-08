﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ApplicationCore;
using ApplicationCore.Models;
using BusinessService.UnitOfWork;
using FoodyWebApplication.Helper;

namespace FoodyWebApplication.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckOutController(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            var user = HttpContext.Session.GetLoginUser();
            if (user != null)
            {
                var foodyContext = await _unitOfWork.OrderService.Get(expression: c => c.AccountId == user.AccountId, includeProperties: "Account");
                return View(foodyContext);
            }
            else
            {
                return RedirectToPage("/Account/Login");
            }

        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = HttpContext.Session.GetLoginUser();
            if (user != null)
            {

                var order = await _unitOfWork.OrderService.GetFirst(c => c.OrderId == id && user.AccountId == c.AccountId, "Account");
            if (order == null)
            {
                return NotFound();
            }

            return View(order);

            }
            else
            {
                return RedirectToPage("/Account/Login");
            }
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            var order = new Order();
            var user = HttpContext.Session.GetLoginUser();
            if(user != null)
            {
                //get cart from session
                //pass data from cart to order
                //set default value to order
                var cart = SessionExtension.GetCart(HttpContext.Session);

                if (cart != null && cart.ProductList.Count > 0)
                {
                    order.OrderDate = DateTime.Now;
                    order.RequiredDate = DateTime.Now.AddDays(7);
                    order.ShippedDate = DateTime.Now.AddDays(14);
                    string total = SessionExtension.Get<string>(HttpContext.Session, "GioHangTotal");
                    order.TotalPrice = Decimal.Parse(total);
                    order.AccountId = user.AccountId;
                    order.Account = user;
                    order.OrderDetails = new List<OrderDetail>();
                    foreach (var item in cart.ProductList)
                    {
                        var orderDetail = new OrderDetail()
                        {
                            ProductId = item.ProductId,
                            Quantity = item.QuantityPerUnit,
                            UnitPrice = item.UnitPrice,

                        };
                        order.OrderDetails.Add(orderDetail);
                    }
                }
                order.IsDeleted = false;
                //end
                return View(order);
            } else
            {
                return RedirectToPage("/Account/Login");
            }

            
        }
        public IActionResult Edit()
        {
            var order = new Order();
            var user = HttpContext.Session.GetLoginUser();
            if (user != null)
            {
                //get cart from session
                //pass data from cart to order
                //set default value to order
                var cart = SessionExtension.GetCart(HttpContext.Session);

                if (cart != null && cart.ProductList.Count > 0)
                {
                    order.OrderDate = DateTime.Now;
                    order.RequiredDate = DateTime.Now.AddDays(7);
                    order.ShippedDate = DateTime.Now.AddDays(14);
                    string total = SessionExtension.Get<string>(HttpContext.Session, "GioHangTotal");
                    order.TotalPrice = Decimal.Parse(total);
                    order.AccountId = user.AccountId;
                    order.Account = user;
                    order.OrderDetails = new List<OrderDetail>();
                    foreach (var item in cart.ProductList)
                    {
                        var orderDetail = new OrderDetail()
                        {
                            ProductId = item.ProductId,
                            Quantity = item.QuantityPerUnit,
                            UnitPrice = item.UnitPrice,

                        };
                        order.OrderDetails.Add(orderDetail);
                    }
                }
                order.IsDeleted = false;
                //end
                return View(order);
            }
            else
            {
                return RedirectToPage("/Account/Login");
            }


        }
        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,AccountId,OrderDate,RequiredDate,ShippedDate,Freight,ShipAddress,TotalPrice,IsDeleted")] Order order)
        {
            order.OrderDetails = new List<OrderDetail>();
            var cart = SessionExtension.GetCart(HttpContext.Session);
            foreach (var item in cart.ProductList)
            {
                var orderDetail = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    Quantity = item.QuantityPerUnit,
                    UnitPrice = item.UnitPrice,

                };
                order.OrderDetails.Add(orderDetail);
            }
            if (order.OrderDetails.Count() > 0 && ModelState.ErrorCount <= 1)
            {
                await _unitOfWork.OrderService.Add(order);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(await _unitOfWork.AccountService.Get(), "AccountId", "Address", order.AccountId);
           
            return View(order);
        }


    }
}
