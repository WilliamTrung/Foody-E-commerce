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

namespace FoodyWebApplication.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var foodyContext = await _unitOfWork.ProductService.Get(expression: null, "Category", "Supplier");
            return View(foodyContext.ToList());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _unitOfWork.ProductService.GetFirst(expression: c => c.ProductId == id, "Category", "Supplier");
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }
        [Authorize(Roles = "Administrator,Seller")]
        // GET: Product/Create
        public IActionResult Create()
        {
            var categories = _unitOfWork.CategoryService.Get().Result;
            var suppliers = _unitOfWork.SupplierService.Get().Result;
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");
            ViewData["SupplierId"] = new SelectList(suppliers, "SupplierId", "Address");
            return View();
        }
        [Authorize(Roles = "Administrator,Seller")]
        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,ProductImage,IsDeleted")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.ProductService.Add(product);
                return RedirectToAction(nameof(Index));
            }
            var categories = _unitOfWork.CategoryService.Get().Result;
            var suppliers = _unitOfWork.SupplierService.Get().Result;
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");
            ViewData["SupplierId"] = new SelectList(suppliers, "SupplierId", "Address");
            return View(product);
        }
        [Authorize(Roles = "Administrator,Seller")]
        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _unitOfWork.ProductService.GetFirst(c => c.ProductId == id, "Category", "Supplier");
            if (product == null)
            {
                return NotFound();
            }
            var categories = _unitOfWork.CategoryService.Get().Result;
            var suppliers = _unitOfWork.SupplierService.Get().Result;
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");
            ViewData["SupplierId"] = new SelectList(suppliers, "SupplierId", "Address");
            return View(product);
        }
        [Authorize(Roles = "Administrator,Seller")]
        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,ProductImage,IsDeleted")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.ProductService.Update(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            var categories = _unitOfWork.CategoryService.Get().Result;
            var suppliers = _unitOfWork.SupplierService.Get().Result;
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");
            ViewData["SupplierId"] = new SelectList(suppliers, "SupplierId", "Address");
            return View(product);
        }
        [Authorize(Roles = "Administrator,Seller")]
        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _unitOfWork.ProductService.GetFirst(p => p.ProductId == id, "Category", "Supplier");
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [Authorize(Roles = "Administrator,Seller")]
        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _unitOfWork.ProductService.GetFirst(p => p.ProductId == id, "Category", "Supplier");
            if (product != null)
            {
                await _unitOfWork.ProductService.Delete(product);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
