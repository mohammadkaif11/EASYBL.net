using EASYBL.bussiness.UserService;
using EASYBL.model.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EASYBL.web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        public ActionResult Login()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        }

        [HttpPost]
        public ActionResult Login(string Email, string Password, string ConfirmPassword)
        {
            try
            {
                if (Password != ConfirmPassword)
                {
                    ModelState.AddModelError("Password", "Password is Not Match");
                    return View();
                }
                else
                {
                    var user = userService.IsRegistered(Password, Email);
                    if (user != null)
                    {

                        FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
                        return RedirectToAction("Index", "Main");
                    }
                    else
                    {

                        ModelState.AddModelError("Email and Password", "Crendials is not Found");
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {

                return RedirectToAction("ErrorPage", "Error");
            }

        }

        public ActionResult Register()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        }

        [HttpPost]
        public ActionResult Register(string Name, string Address, string Number, string GstNo, string Email, string Password, string ConfirmPassowrd)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Password == ConfirmPassowrd)
                    {
                        var IsAvailable = userService.IsEmailandName(Email, Name);
                        if (IsAvailable)
                        {
                            ModelState.AddModelError("User", "Email And Shop Are Available");
                        }
                        ShopkeeperUsers user = new ShopkeeperUsers() { Email = Email, Password = Password, Name = Name, Number = Number, GstNo = GstNo, Address = Address, CurrentBillNo = 1 };
                        var users = userService.Register(user);

                        FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);

                        return RedirectToAction("Index", "Main");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Password is Not Match");
                        return View();
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");

            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}