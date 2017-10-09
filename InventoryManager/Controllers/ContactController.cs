using System.Linq;
using System.Net;
using System.Web.Mvc;
using InventoryManager.Models;
using InventoryManager.ViewModel;

namespace InventoryManager.Controllers
{
    [Authorize(Roles = "InventoryCustomerManager, InventoryAdmin")]
    public class ContactController : Controller
    {
        private InventoryContext db = new InventoryContext();

        public ActionResult Add(int CustomerId = 0)
        {
            if (CustomerId == 0) return new HttpNotFoundResult();

            if (!db.Customers.Any(c => c.Id == CustomerId)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var contact = new ContactData();
            contact.CustomerId = CustomerId;

            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ContactData c)
        {
            if(c.CustomerId == 0) return new HttpNotFoundResult();

            if(!ModelState.IsValid) return View(c);
            
            var contact = new Contact();
            db.Contacts.Add(contact);

            MapContact(contact, c);
            db.SaveChanges();

            TempData["AlertMessage"] = "Contact Saved";
            return RedirectToAction("Show", "Customer", new { Id = c.CustomerId });
        }

        public ActionResult Edit(int? Id)
        {
            if (Id == null) return new HttpNotFoundResult();

            var contact = db.Contacts.SingleOrDefault(l => l.Id == Id);
            if (contact == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var viewModel = new ContactData();
            PopulateContactData(contact, viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContactData c)
        {
            if (c.CustomerId == 0) return new HttpNotFoundResult();

            if (!ModelState.IsValid) return View(c);

            var contact = db.Contacts.SingleOrDefault(i => i.Id == c.Id);
            if (contact == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            MapContact(contact, c);
            db.SaveChanges();

            TempData["AlertMessage"] = "Contact Saved";
            return RedirectToAction("Show", "Customer", new { Id = c.CustomerId });
        }

        public void MapContact(Contact contactToUpdate, ContactData data)
        {
            contactToUpdate.CustomerId = data.CustomerId;
            contactToUpdate.FirstName = data.FirstName;
            contactToUpdate.LastName = data.LastName;
            contactToUpdate.Phone = data.Phone;
            contactToUpdate.Email = data.Email;

        }

        public void PopulateContactData(Contact contact, ContactData contactToDisplay)
        { 
            contactToDisplay.CustomerId = contact.CustomerId;
            contactToDisplay.FirstName = contact.FirstName;
            contactToDisplay.LastName = contact.LastName;
            contactToDisplay.Phone = contact.Phone;
            contactToDisplay.Email = contact.Email;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}