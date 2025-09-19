using System.ComponentModel.DataAnnotations;
using clinic.BLL.ModelVM.AccountVM;
using clinic.BLL.ModelVM.Login;
using clinic.BLL.Service.abstraction;
using clinic.DAL.Enum;
using Microsoft.AspNetCore.Mvc;

namespace clinic.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var (user, errorMessage) = await _accountService.Login(loginVM.Email, loginVM.Password);
                if (user != null)
                {
                    var roles = await _accountService.GetUserRoles(user);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    if (roles.Contains("Admin"))
                        return RedirectToAction("Index", "AdminDashboard");
                    else if (roles.Contains("Doctor"))
                        return RedirectToAction("Index", "DoctorDashboard");
                    else if (roles.Contains("Patient"))
                        return RedirectToAction("Create", "Appointment"); 
                    else
                        return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, errorMessage);
            }
            return View(loginVM);
        }




        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(SignUpVM signUpVM)
        {
            if (ModelState.IsValid)
            {
                var (isSuccess, errors) = await _accountService.Register(signUpVM);
                if (isSuccess)
                {
                    return RedirectToAction("Login","Account");
                }
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            return View(signUpVM);
        }

    }
}
