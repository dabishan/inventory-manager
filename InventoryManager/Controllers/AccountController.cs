using System;
using System.Configuration;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using InventoryManager.ViewModel;

namespace InventoryManager.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View(new LoginData());
        }

        [HttpPost]
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
            
            if (isAuthenticated)
            {

                return RedirectToAction("Index", "Home");
            }

            viewModel.Password = "";
            TempData["AlertMessage"] = "Invalid Username or Password";
            return View(viewModel);
        }

        public static bool ValidateCredentials(string domainController, int port, string domain, string username, string password)
        {
            const int LDAP_InvalidCredentials = 0x31;

            LdapDirectoryIdentifier ldapDirectoryIdentifier
                = new LdapDirectoryIdentifier(domainController, port);

            NetworkCredential networkCredential
                = new NetworkCredential(username, password, domain);

            try
            {
                using (LdapConnection ldapConnection = new LdapConnection(ldapDirectoryIdentifier))
                {
                    ldapConnection.SessionOptions.SecureSocketLayer = true;
                    ldapConnection.AuthType = AuthType.Negotiate;
                    ldapConnection.Bind(networkCredential);
                }

                // if the bind succeeds, the credentials are OK
                return true;
            }
            catch (LdapException ldapException)
            {
                // Unfortunately, invalid credentials fall into this block with a specific error code
                if (ldapException.ErrorCode.Equals(LDAP_InvalidCredentials)) return false;
                throw new Exception();
            }

        }
    }
}