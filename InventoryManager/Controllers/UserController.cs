using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryManager.ViewModel;

namespace InventoryManager.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View( new LoginData());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(LoginData l)
        {
            if(!ModelState.IsValid) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return View(l);
        }

        public ActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}