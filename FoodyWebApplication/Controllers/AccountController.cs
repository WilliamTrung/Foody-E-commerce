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
using FoodyWebApplication.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FoodyWebApplication.Helper;

namespace FoodyWebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorize(Roles = "Administrator")]
        // GET: Account
        public async Task<IActionResult> Index()
        {
            var foodyContext = await _unitOfWork.AccountService.Get(includeProperties: "Role");
            return View(foodyContext);
        }
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("AccountId,Username,RoleId,Password,Address,Phone,IsDeleted")] Account account)
        {

            var loginuser = SessionExtension.GetLoginUser(HttpContext.Session);
            string action = string.Empty;
            bool check = true;
            if (loginuser != null)
            {
                if(loginuser.RoleId == 1)
                {
                    //is admin
                    action = "Index";
                } else if(loginuser.AccountId == account.AccountId)
                {
                    action = "Details";
                } else
                {
                    check = false;
                }
            }
            if (check)
            {
                ModelState.Remove("IsDeleted");
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
                    return RedirectToAction(action, new { id = account.AccountId });
                }
                var roles = await _unitOfWork.RoleService.Get();
                ViewData["RoleId"] = new SelectList(roles, "Id", "Name", account.RoleId);
                return RedirectToAction(action, new { id = account.AccountId });
            } else
            {
                return RedirectToAction("Logout", "Home");
            }
            
        }
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl = "Product/Index")
        {
            LoginModel objLoginModel = new LoginModel();
            //objLoginModel.ReturnUrl = ReturnUrl;
            return View(objLoginModel);
        }
        // GET: Account/Register
        public async Task<IActionResult> Register()
        {
            var roles = await _unitOfWork.RoleService.Get();
            ViewData["RoleId"] = new SelectList(roles, "Id", "Name");
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel objLoginModel)
        {
            if (ModelState.IsValid)
            {
                var user = _unitOfWork.AccountService.GetFirst(x => x.Username == objLoginModel.UserName && x.Password == objLoginModel.Password && !x.IsDeleted, includeProperties: "Role").Result;
                if (user != null)
                {
                    //A claim is a statement about a subject by an issuer and    
                    //represent attributes of the subject that are useful in the context of authentication and authorization operations.
                    var claims = new List<Claim>() {
                        new Claim(ClaimTypes.Name, user.Username, ClaimValueTypes.String),
                        new Claim(ClaimTypes.Role, user.Role.Name, ClaimValueTypes.String)
                    };
                    //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
                    var principal = new ClaimsPrincipal(identity);
                    //SignInAsync is a Extension method for Sign in a principal for the specified scheme.    
                    await HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(identity),
                                new AuthenticationProperties
                                {
                                    IsPersistent = objLoginModel.RememberLogin,
                                    ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
                                });
                    Helper.SessionExtension.Set(HttpContext.Session, "login-user", user);
                    //Account? userlog = Helper.SessionExtension.GetLoginUser(HttpContext.Session);
                    return LocalRedirect(objLoginModel.ReturnUrl);
                }
                else
                {
                    ViewBag.Message = "Invalid Credential";
                    return View(user);
                }
            }
            return View(objLoginModel);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel register)
        {
            var regiser_account = new Account()
            {
                Address = register.Address,
                Password = register.Password,
                Phone = register.Phone,
                RoleId = register.RoleId,
                Username = register.Username,
            };
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _unitOfWork.AccountService.Register(regiser_account);
                    if (user != null)
                    {
                        //A claim is a statement about a subject by an issuer and    
                        //represent attributes of the subject that are useful in the context of authentication and authorization operations.
                        var claims = new List<Claim>() {
                        new Claim(ClaimTypes.Name, user.Username, ClaimValueTypes.String),
                        new Claim(ClaimTypes.Role, user.Role.Name, ClaimValueTypes.String)
                    };
                        //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
                        var principal = new ClaimsPrincipal(identity);
                        //SignInAsync is a Extension method for Sign in a principal for the specified scheme.    
                        await HttpContext.SignInAsync(
                                    CookieAuthenticationDefaults.AuthenticationScheme,
                                    new ClaimsPrincipal(identity),
                                    new AuthenticationProperties
                                    {
                                        ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
                                    });
                        Helper.SessionExtension.Set(HttpContext.Session, "login-user", user);
                        //Account? userlog = Helper.SessionExtension.GetLoginUser(HttpContext.Session);
                        return LocalRedirect("~/Product/Index");
                    }
                    else
                    {
                        ViewBag.Message = "Cannot register this user";
                        return View(register);
                    }
                } catch (Exception ex)
                {
                    if(ex.Message == "DUPLICATE USERNAME")
                    {
                        ModelState.AddModelError("Username", "This username has been taken!");
                    }
                }
                
            }
            var roles = await _unitOfWork.RoleService.Get();
            ViewData["RoleId"] = new SelectList(roles, "Id", "Name");
            return View(regiser_account);
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Helper.SessionExtension.Logout(HttpContext.Session);
            return LocalRedirect("/");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
