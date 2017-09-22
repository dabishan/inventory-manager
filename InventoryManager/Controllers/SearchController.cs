using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InventoryManager.Models;
using InventoryManager.ViewModel;

namespace InventoryManager.Controllers
{
    public class SearchController : Controller
    {
        private InventoryContext db = new InventoryContext();

        // GET: Search
        public ActionResult Index(string query)
        {
            var viewModel = new SearchAllView()
            {
                Query = query,
                Hardwares = new List<Hardware>(),
                Customers = new List<Customer>(),
                Contacts = new List<Contact>()
            };

            if (string.IsNullOrWhiteSpace(query) || query.Length < 3)
            {
                ViewBag.InvalidQuery = true;
                return View(viewModel);
            }
            else
            {
                ViewBag.InvalidQuery = false;
            }

            query = query.ToLower();

            viewModel.Hardwares = SearchHardware(query).Include(i=> i.Inventory).ToList();
            viewModel.Customers = SearchCustomers(query).Include(c => c.Owner).ToList();
            viewModel.Contacts = SearchContacts(query).Include(c => c.Customer.Owner).ToList();

            return View(viewModel);
        }

        public IQueryable<Hardware> SearchHardware(string query)
        {
            var hardwares = db.Hardwares
                .Where(i => i.Inventory.Name.ToLower().Contains(query)
                            || i.Inventory.ModelName.ToLower().Contains(query));

            return hardwares;
        }

        public IQueryable<Customer> SearchCustomers(string query)
        {
            var customers = db.Customers
                .Where(c => c.Owner.Name.ToLower().Contains(query));

            return customers;
        }

        public IQueryable<Contact> SearchContacts(string query)
        {
            var contacts = db.Contacts
                .Where(c => c.FirstName.ToLower().Contains(query)
                    || c.LastName.ToLower().Contains(query));
                    
            return contacts;
        }
        
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}