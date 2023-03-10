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
            var loginuser = SessionExtension.GetLoginUser(HttpContext.Session);
            IEnumerable<Product>? products;
            if(loginuser != null && (loginuser.RoleId == 1 || loginuser.RoleId == 3)) //is admin or seller
            {
                products = await _unitOfWork.ProductService.Get(expression: null, "Category", "Supplier");
            } else
            {
                products = await _unitOfWork.ProductService.Get(expression: c => !c.IsDeleted, "Category", "Supplier");
            }
            
            return View(products.ToList());
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
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice")] Product product, IFormFile file)
        {            
            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.ProductService.Add(product);
                    var tail = "." + file.FileName.Split('.').Last();
                    var file_stored = "wwwroot" + "/" + "images" + "/" + product.ProductName + tail;
                    var file_save = "images" + "/" + product.ProductName + tail;
                    using (var filestream = new FileStream(file_stored, FileMode.Create))
                    {
                        await file.CopyToAsync(filestream);
                    }
                    product.ProductImage = file_save;
                    await _unitOfWork.ProductService.Update(product);
                    return RedirectToAction(nameof(Index));
                } catch (Exception ex)
                {
                    var message = ex.Message;
                    var error = message.Split("-");
                    if (error.Count() == 2)
                    {
                        ModelState.AddModelError(error[0], error[1]);
                    }
                }
                
            }
          
            var categories = _unitOfWork.CategoryService.Get().Result;
            var suppliers = _unitOfWork.SupplierService.Get().Result;
            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");
            ViewData["SupplierId"] = new SelectList(suppliers, "SupplierId", "Address");
            return View(product);
        }
        [Authorize]
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
        [Authorize]
        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string oldPic,[Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,IsDeleted")] Product product, IFormFile? file)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }
            ModelState.Remove("oldPic");
            if (ModelState.IsValid)
            {
                try
                {
                    if(file != null)
                    {
                        var tail = "." + file.FileName.Split('.').Last();
                        var file_stored = "wwwroot" + "/" + "images" + "/" + product.ProductName + tail;
                        var file_save = "images" + "/" + product.ProductName + tail;
                        using (var filestream = new FileStream(file_stored, FileMode.Create))
                        {
                            await file.CopyToAsync(filestream);

                        }
                        product.ProductImage = file_save;
                    } else
                    {
                        product.ProductImage = oldPic;
                    }
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
        public async Task<IActionResult> DeleteConfirmed(int ProductId)
        {
            var product = await _unitOfWork.ProductService.GetFirst(p => p.ProductId == ProductId, "Category", "Supplier");
            if (product != null)
            {
                await _unitOfWork.ProductService.Delete(product);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
