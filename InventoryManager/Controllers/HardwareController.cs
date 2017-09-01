using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using InventoryManager.Models;
using InventoryManager.ViewModel;

namespace InventoryManager.Controllers
{
    public class HardwareController : Controller
    {
        private InventoryContext db = new InventoryContext();

        // GET: Inventory
        public ActionResult Index()
        {
            var viewModel = new HardwareList();

            viewModel.Hardwares = db.Hardwares
                .Include(h => h.Inventory)
                .Include(h => h.Inventory.Maker).ToList();

            return View(viewModel);
        }

        public ActionResult Add()
        {
            var viewModel = new HardwareData();

            viewModel.Hardware = new Hardware();
            viewModel.Hardware.Inventory = new Inventory();
            viewModel.HardwareTypes = db.HardwareTypes.Where(h => h.Status == true);
            viewModel.Vendors = db.Vendors.Where(v => v.Status == true);
            viewModel.Makers = db.Makers.Where(m => m.Status == true);

            return View(viewModel);
        }

        public ActionResult Edit(int? Id)
        {
            if (Id == null ) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var hardware = db.Hardwares
                .Include(i => i.Inventory)
                .Single(i => i.HardwareId == Id);


            if(hardware == null) return new HttpNotFoundResult();

            var viewModel = new HardwareData();

            viewModel.Hardware = hardware;
            viewModel.HardwareTypes = db.HardwareTypes.Where(h => h.Status == true);
            viewModel.Vendors = db.Vendors.Where(v => v.Status == true);
            viewModel.Makers = db.Makers.Where(m => m.Status == true);

            viewModel.ReferrerUrl = Request.UrlReferrer?.AbsoluteUri;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(HardwareData h)
        {
            var r = Request.Form;

            Hardware hardware;
            if (ModelState.IsValid == false) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (h.Hardware.HardwareId == 0 )
            {
                hardware = h.Hardware;
                db.Hardwares.Add(hardware);

                hardware.Inventory.CreatedOn = DateTime.Now.Date;
            }
            else
            {
                hardware = db.Hardwares
                    .Include(i => i.Inventory)
                    .Single(i => i.HardwareId == h.Hardware.HardwareId);

                if (hardware == null) return new HttpNotFoundResult();
            }

            hardware.Inventory.InventoryType = InventoryType.Hardware;
            hardware.Inventory.Owner = db.Owners.Find(1);
            hardware.Inventory.VendorId = h.Hardware.Inventory.VendorId;
            hardware.Inventory.MakerId = h.Hardware.Inventory.MakerId;
            hardware.HardwareTypeId = h.Hardware.HardwareTypeId;
            hardware.Inventory.ModifiedOn = DateTime.Now.Date;

            hardware.Inventory.Status = InventoryStatus.Pending;
            db.SaveChanges();
            TempData["AlertMessage"] = "I am Here";
            return RedirectToAction("Edit", new { Id = hardware.HardwareId });
        }

        public ActionResult Show(int? Id)
        {
            if (Id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var viewModel = new HardwareData();

            viewModel.Hardware = db.Hardwares
                .Include(h => h.Inventory)
                .Include(h => h.Inventory.Maker)
                .Include(h => h.Inventory.Vendor)
                .Include(h => h.HardwareType)
                .Single(h => h.HardwareId == Id);

            viewModel.ReferrerUrl = Request.UrlReferrer.AbsoluteUri;
            return View(viewModel);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}