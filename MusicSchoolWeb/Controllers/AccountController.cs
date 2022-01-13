using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicSchoolWeb.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string UserId, string password)
        {
            try
            {
                if (UserId == "admin" && password == "admin@123")
                {
                    Session["User'"] = UserId;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    TempData["Msg"] = "Please Enter valid UserName and Password";
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            return View();
        }
        public ActionResult Logout()
        {
            return RedirectToAction("Index", "Home");
        }

    }
}