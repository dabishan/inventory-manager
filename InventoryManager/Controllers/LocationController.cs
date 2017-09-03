using System.Linq;
using System.Net;
using System.Web.Mvc;
using InventoryManager.Models;
using InventoryManager.ViewModel;

namespace InventoryManager.Controllers
{
    public class LocationController : Controller
    {
        private InventoryContext db = new InventoryContext();


        public ActionResult Add(int CustomerId = 0)
        {
            if (CustomerId == 0) return new HttpNotFoundResult();

            if (!db.Customers.Any(c => c.Id == CustomerId)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var location = new LocationData();
            location.CustomerId = CustomerId;

            return View(location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(LocationData c)
        {
            if (c.CustomerId == 0) return new HttpNotFoundResult();

            if (!ModelState.IsValid) return View(c);

            var location = new Location();
            db.Locations.Add(location);

            MapLocation(location, c);
            db.SaveChanges();

            TempData["AlertMessage"] = "Location Saved";
            return RedirectToAction("Edit", "Customer", new { Id = c.CustomerId });
        }

        public ActionResult Edit(int? Id)
        {
            if (Id == null) return new HttpNotFoundResult();

            var location = db.Locations.SingleOrDefault(l => l.Id == Id);
            if (location == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var viewModel = new LocationData();
            PopulateLocationData(location, viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LocationData c)
        {
            if (c.CustomerId == 0) return new HttpNotFoundResult();

            if (!ModelState.IsValid) return View(c);

            var location = db.Locations.SingleOrDefault(i => i.Id == c.Id);
            if (location == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            MapLocation(location, c);
            db.SaveChanges();

            TempData["AlertMessage"] = "Location Saved";
            return RedirectToAction("Edit", "Customer", new { Id = c.CustomerId });
        }

        public void MapLocation(Location locationToUpdate, LocationData data)
        {
            locationToUpdate.CustomerId = data.CustomerId;
            locationToUpdate.Street = data.Street;
            locationToUpdate.Unit = data.Unit;
            locationToUpdate.City = data.City;
            locationToUpdate.State = data.City;
            locationToUpdate.Zip = data.Zip;
            locationToUpdate.Phone = data.Phone;
            locationToUpdate.Fax = data.Fax;

        }

        public void PopulateLocationData(Location location, LocationData locationToDisplay)
        {
            locationToDisplay.CustomerId = location.CustomerId;
            locationToDisplay.Street = location.Street;
            locationToDisplay.Unit = location.Unit;
            locationToDisplay.City = location.City;
            locationToDisplay.State = location.City;
            locationToDisplay.Zip = location.Zip;
            locationToDisplay.Phone = location.Phone;
            locationToDisplay.Fax = location.Fax;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}