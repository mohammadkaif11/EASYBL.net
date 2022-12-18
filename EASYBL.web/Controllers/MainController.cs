using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EASYBL.web.Controllers
{
    [Authorize]
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            try
            {
                var a = User.Identity.Name;
                if(string.IsNullOrEmpty(a))
                {
                    return RedirectToAction("Login", "Account");
                }
                return View();
            }
            catch(Exception ex) {
               
                return RedirectToAction("Login", "Account");
            }
        }
    }
}