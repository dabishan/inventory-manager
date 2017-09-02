using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using InventoryManager.Models;

namespace InventoryManager.Controllers
{
    public class LocationController : Controller
    {
        private InventoryContext db = new InventoryContext();

        public ActionResult Add(int CustomerId = 0)
        {
            if (CustomerId == 0) return new HttpNotFoundResult();

            if (!db.Customers.Any(c => c.Id == CustomerId)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var location = new Location();
            location.CustomerId = CustomerId;

            return View(location);
        }

        public ActionResult Edit(int? Id)
        {
            if (Id == null) return new HttpNotFoundResult();

            var location = db.Locations.SingleOrDefault(l => l.Id == Id);
            if(location == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return View(location);
        }

        public ActionResult Save(Location l)
        {
            Location location;
            if (ModelState.IsValid == false) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (l.Id == 0)
            {
                location = new Location();
                db.Locations.Add(location);
            }
            else
            {
                location = db.Locations.SingleOrDefault(i => i.Id == l.Id);
                if (location == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            location.CustomerId = l.CustomerId;
            location.Street = l.Street;
            location.Unit = l.Unit;
            location.City = l.City;
            location.State = l.City;
            location.Zip = l.Zip;

            db.SaveChanges();
            TempData["AlertMessage"] = "Location Saved";
            return RedirectToAction("Edit", "Owner", new {Id = l.CustomerId});
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}