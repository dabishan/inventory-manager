using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using InventoryManager.Models;
using InventoryManager.ViewModel;

namespace InventoryManager.Controllers
{
    public class OwnerController : Controller
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

            viewModel.Customer = new Customer()
            {
                Owner = new Owner()
            };

            return View(viewModel);
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
            viewModel.Customer = customer;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(CustomerData c)
        {
            Customer customer;
            if (ModelState.IsValid == false) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            if (c.Customer.Id == 0)
            {
                customer = c.Customer;
                customer.Owner = new Owner()
                {
                    CreatedOn = DateTime.Now.Date,
                    OwnerType = OwnerType.Customer
                };

                db.Customers.Add(customer);
            }
            else
            {
                customer = db.Customers
                    .Include(i => i.Owner)
                    .SingleOrDefault(i => i.Id == c.Customer.Id);
                if(customer == null) return new HttpNotFoundResult();

                customer.Name = c.Customer.Name;
                customer.Description = c.Customer.Description;
                customer.Owner.ModifiedOn = DateTime.Now.Date;

            }

            customer.Owner.ModifiedOn = DateTime.Now.Date;
            
            db.SaveChanges();
            TempData["Alert"] = "New Contact Added.";

            return RedirectToAction("Edit", new {customer.Id});
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}