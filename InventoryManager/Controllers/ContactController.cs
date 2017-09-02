using System.Linq;
using System.Net;
using System.Web.Mvc;
using InventoryManager.Models;

namespace InventoryManager.Controllers
{
    public class ContactController : Controller
    {
        private InventoryContext db = new InventoryContext();

        public ActionResult Add(int CustomerId = 0)
        {
            if (CustomerId == 0) return new HttpNotFoundResult();

            if (!db.Customers.Any(c => c.Id == CustomerId)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var contact = new Contact();
            contact.CustomerId = CustomerId;

            return View(contact);
        }

        public ActionResult Edit(int? Id)
        {
            if (Id == null) return new HttpNotFoundResult();

            var contact = db.Contacts.SingleOrDefault(l => l.Id == Id);
            if (contact == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return View(contact);
        }

        public ActionResult Save(Contact c)
        {
            Contact contact;
            if (ModelState.IsValid == false) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (c.Id == 0)
            {
                contact = new Contact();
                db.Contacts.Add(contact);
            }
            else
            {
                contact = db.Contacts.SingleOrDefault(i => i.Id == c.Id);
                if (contact == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            contact.CustomerId = c.CustomerId;
            contact.FirstName = c.FirstName;
            contact.LastName = c.LastName;
            contact.Phone = c.Phone;
            contact.Email = c.Email;

            db.SaveChanges();
            TempData["AlertMessage"] = "Contact Saved";
            return RedirectToAction("Edit", "Owner", new { Id = c.CustomerId });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}