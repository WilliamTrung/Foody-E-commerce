using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Models;
using BusinessService.UnitOfWork;

namespace FoodyWebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Account
        public async Task<IActionResult> Index()
        {
            var foodyContext = await _unitOfWork.AccountService.Get(expression: null, "Role");
            return View(foodyContext);
        }

        // GET: Account/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _unitOfWork.AccountService.GetFirst(c => c.AccountId == id, "Role");
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Account/Create
        public async Task<IActionResult> Create()
        {
            var roles = await _unitOfWork.RoleService.Get();
            ViewData["RoleId"] = new SelectList(roles, "Id", "Name");
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,Username,RoleId,Password,Address,Phone")] Account account)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.AccountService.Add(account);                
                return RedirectToAction(nameof(Index));
            }
            var roles = await _unitOfWork.RoleService.Get();
            ViewData["RoleId"] = new SelectList(roles, "Id", "Name", account.RoleId);
            return View(account);
        }

        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _unitOfWork.AccountService.GetFirst(c => c.AccountId == id, "Role");
            if (account == null)
            {
                return NotFound();
            }
            var roles = await _unitOfWork.RoleService.Get();
            ViewData["RoleId"] = new SelectList(roles, "Id", "Name", account.RoleId);
            return View(account);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,Username,RoleId,Password,Address,Phone")] Account account)
        {
            if (id != account.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.AccountService.Update(account);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();                
                }
                return RedirectToAction(nameof(Index));
            }
            var roles = await _unitOfWork.RoleService.Get();
            ViewData["RoleId"] = new SelectList(roles, "Id", "Name", account.RoleId);
            return View(account);
        }

        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _unitOfWork.AccountService.GetFirst(c => c.AccountId == id, "Role");
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _unitOfWork.AccountService.GetFirst(c => c.AccountId == id, "Role");
            if (account != null)
            {
                await _unitOfWork.AccountService.Delete(account);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
