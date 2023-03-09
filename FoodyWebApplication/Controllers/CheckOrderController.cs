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

namespace FoodyWebApplication.Controllers
{
    public class CheckOrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckOrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: CheckOrder
        public async Task<IActionResult> Index()
        {
            var foodyContext = await _unitOfWork.OrderService.Get();
            return View(foodyContext);
        }

        // GET: CheckOrder/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _unitOfWork.OrderService.GetFirst(c => c.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public async Task<IActionResult> DeletedOrderIndex()
        {
            var foodyContext = await _unitOfWork.OrderService.Get();
            return View(foodyContext);
        }

        public async Task<IActionResult> DeletedOrderDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _unitOfWork.OrderService.GetFirst(c => c.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
