using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using InventoryManager.Models;
using InventoryManager.ViewModel;

namespace InventoryManager.Controllers
{
    public class CustomerController : Controller
    {
        private InventoryContext db = new InventoryContext();

        // GET: Owner
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            var viewModel = new CustomerData();
            
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CustomerData c)
        {
            if (!ModelState.IsValid) return View(c);

            var customer = new Customer();
            var owner = new Owner();

            customer.Owner = owner;
            customer.Owner.OwnerType = OwnerType.Customer;

            MapCustomer(customer, c);
            customer.Owner.CreatedOn = DateTime.Now.Date;

            db.Customers.Add(customer);
            db.SaveChanges();

            TempData["AlertMessage"] = "Customer Saved";
            return RedirectToAction("Edit", "Customer", new { customer.Id });
        }

        private void MapCustomer(Customer customerToUpdate, CustomerData data)
        {
            customerToUpdate.Name = data.Name;
            customerToUpdate.Description = data.Description;
            customerToUpdate.Owner.ModifiedOn = DateTime.Now.Date;
        }

        private void PopulateCustomer(Customer customer, CustomerData customerToDisplay)
        {
            customerToDisplay.Name = customer.Name;
            customerToDisplay.Description = customer.Description;
            customerToDisplay.Id = customer.Id;
        }

        public ActionResult Edit(int? Id)
        {
            if(Id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var customer = db.Customers
                .Include(c => c.Owner)
                .Include(c => c.Locations)
                .Include(c => c.Contacts)
                .SingleOrDefault(c => c.Id == Id);
            if (customer == null) return new HttpNotFoundResult();

            var viewModel = new CustomerData();
            PopulateCustomer(customer, viewModel);

            viewModel.Locations = customer.Locations;
            viewModel.Contacts = customer.Contacts;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerData c)
        {
            if (c.Id == 0) return new HttpNotFoundResult();

            var customer = db.Customers
                .Include(i => i.Owner)
                .Include(i => i.Locations)
                .Include(i => i.Contacts)
                .SingleOrDefault(i => i.Id == c.Id);
            if (customer == null) return new HttpNotFoundResult();

            if (!ModelState.IsValid)
            {
                c.Locations = customer.Locations;
                c.Contacts = customer.Contacts;
                return View(c);
            }

            MapCustomer(customer, c);

            db.SaveChanges();

            TempData["AlertMessage"] = "Customer Saved";
            return RedirectToAction("Edit", "Customer", new { customer.Id });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}