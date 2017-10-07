using System;
using System.Configuration;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryManager.Models;
using InventoryManager.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace InventoryManager.Controllers
{

    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        // GET: Account
        public ActionResult Login()
        {
            return View(new LoginData());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginData viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            bool isAuthenticated;
            var settings = ConfigurationManager.AppSettings;
            try
            {
                isAuthenticated = ValidateCredentials(settings["LdapDomainController"], Convert.ToInt16(settings["LdapPort"]), settings["LdapDomain"], viewModel.Username, viewModel.Password);
            }
            catch (Exception e)
            {
                viewModel.Password = "";
                TempData["AlertMessage"] = "Cannot Connect to Active Directory";
                return View(viewModel);
            }
            
            if (!isAuthenticated)
            {
                viewModel.Password = "";
                TempData["AlertMessage"] = "Invalid Username or Password";
                return View(viewModel);
            }
            
            var username = viewModel.Username;
            if (!viewModel.Username.EndsWith(settings["LdapDomain"]))
            {
                username = username + "@" + settings["LdapDomain"];
            }
            var user = UserManager.FindByName(username);
            
            if (user == null)
            {
                user = new ApplicationUser() { UserName = username, Email = username};
                UserManager.Create(user);
            }
            
            SignInManager.SignIn(user, false, false);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        public static bool ValidateCredentials(string domainController, int port, string domain, string username, string password)
        {
            const int LDAP_InvalidCredentials = 0x31;

            var ldapDirectoryIdentifier = new LdapDirectoryIdentifier(domainController, port);
            var networkCredential = new NetworkCredential(username, password, domain);

            try
            {
                using (LdapConnection ldapConnection = new LdapConnection(ldapDirectoryIdentifier))
                {
                    ldapConnection.SessionOptions.SecureSocketLayer = true;
                    ldapConnection.AuthType = AuthType.Negotiate;
                    ldapConnection.Bind(networkCredential);
                }
                return true;
            }
            catch (LdapException ldapException)
            {
                if (ldapException.ErrorCode.Equals(LDAP_InvalidCredentials)) return false;
                throw new Exception();
            }

        }
    }
}