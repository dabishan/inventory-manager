using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.Linq;
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
        private InventoryContext db = new InventoryContext();

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
        [AllowAnonymous]
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

            var username = viewModel.Username;
            if (!viewModel.Username.EndsWith(ConfigurationManager.AppSettings["DomainName"]))
            {
                username = username + "@" + ConfigurationManager.AppSettings["DomainName"];
            }
            var connectionString = ConfigurationManager.AppSettings["LdapConnectionString"]; 

            var groups = new List<string>();
            string displayName;
            try
            {
                using (var directoryEntry = new DirectoryEntry(connectionString, username, viewModel.Password))
                {
                    var search = new DirectorySearcher(directoryEntry, "(userPrincipalName=" + username + ")");
                    var result = search.FindOne();

                    var adUser = new DirectoryEntry(result.Path);
                    displayName = adUser.Properties["displayName"].Value.ToString();

                    var obGroups = adUser.Invoke("Groups");
                    foreach (var ob in (IEnumerable) obGroups)
                    {
                        // Create object for each group.
                        var obGpEntry = new DirectoryEntry(ob);
                        groups.Add(obGpEntry.Name);
                    }
                    
                }
            }
            catch (DirectoryServicesCOMException)
            {
                viewModel.Password = "";
                TempData["AlertMessage"] = "Invalid Username or Password";
                return View(viewModel);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                viewModel.Password = "";
                TempData["AlertMessage"] = "Cannot Connect to Server";
                return View(viewModel);
            }
            catch (Exception e)
            {
                viewModel.Password = "";
                TempData["AlertMessage"] = e.Message;
                return View(viewModel);
            }

            var user = UserManager.FindByName(username);
            if (user == null)
            {
                user = new ApplicationUser() {UserName = username, Email = username};
                UserManager.Create(user);

                
                var employee = new Employee()
                {
                    User = db.Users.Find(user.Id),
                    Owner = new Owner
                    {
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        Name = displayName,
                        OwnerType = OwnerType.GbhEmployee
                    }
                };

                db.Employees.Add(employee);
                db.SaveChanges();
            }
            else
            {
                var oldRoles = UserManager.GetRoles(user.Id).ToArray();
                UserManager.RemoveFromRoles(user.Id, oldRoles);
            }

            UpdateRoles(groups, user.Id);

            
            SignInManager.SignIn(user, false, false);
            return RedirectToAction("Index", "Home");
        }

        public void UpdateRoles(List<string> groupArr, string userId)
        {
            var roles = new List<string>();

            foreach (var str in groupArr)
            {
                switch (str)
                {
                    case "CN=" + ApplicationUser.ADMIN:
                        roles.Add(ApplicationUser.ADMIN);
                        break;
                    case "CN=" + ApplicationUser.MANAGER:
                        roles.Add(ApplicationUser.MANAGER);
                        break;
                    case "CN=" + ApplicationUser.CUSTOMER_MANAGER:
                        roles.Add(ApplicationUser.CUSTOMER_MANAGER);
                        break;
                    case "CN=" + ApplicationUser.AUDITOR:
                        roles.Add(ApplicationUser.AUDITOR);
                        break;
                }
            }
            if (roles.Count == 0) 
                UserManager.AddToRole(userId, ApplicationUser.USER);
            else
                UserManager.AddToRoles(userId, roles.ToArray());
        }

        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
    }
}