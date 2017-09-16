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
            return RedirectToAction("Show", "Customer", new { customer.Id });
        }

        private void MapCustomer(Customer customerToUpdate, CustomerData data)
        {
            customerToUpdate.Owner.Name = data.Name;
            customerToUpdate.Description = data.Description;
            customerToUpdate.Owner.ModifiedOn = DateTime.Now.Date;
        }

        private void PopulateCustomer(Customer customer, CustomerData customerToDisplay)
        {
            customerToDisplay.Name = customer.Owner.Name;
            customerToDisplay.Description = customer.Description;
            customerToDisplay.Id = customer.Id;
        }

        public ActionResult Edit(int? Id)
        {
            if(Id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var customer = db.Customers
                .Include(c => c.Owner)
                .SingleOrDefault(c => c.Id == Id);

            if (customer == null) return new HttpNotFoundResult();

            var viewModel = new CustomerData();
            PopulateCustomer(customer, viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerData c)
        {
            if (c.Id == 0) return new HttpNotFoundResult();

            var customer = db.Customers
                .Include(i => i.Owner)
                .SingleOrDefault(i => i.Id == c.Id);

            if (customer == null) return new HttpNotFoundResult();

            MapCustomer(customer, c);

            db.SaveChanges();

            TempData["AlertMessage"] = "Customer Saved";
            return RedirectToAction("Show", "Customer", new { customer.Id });
        }

        public ActionResult Show(int? Id)
        {
            if (Id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var viewModel = new CustomerData();

            var customer = db.Customers
                .Include(c => c.Owner)
                .Include(c => c.Locations)
                .Include(c => c.Contacts)
                .SingleOrDefault(c => c.Id == Id);

            if (customer == null) return new HttpNotFoundResult();

            PopulateCustomer(customer, viewModel);

            viewModel.Locations = customer.Locations;
            viewModel.Contacts = customer.Contacts;

            return View(viewModel);
        }

        public ActionResult ListInventories(int? Id)
        {
            if (Id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var customer = db.Customers
                .Include(c => c.Owner.Inventories)
                .SingleOrDefault(c => c.Id == Id);

            if (customer == null) return new HttpNotFoundResult();

            
            return View(customer);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}