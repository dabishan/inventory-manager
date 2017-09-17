using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryManager.Models;
using InventoryManager.ViewModel;

namespace InventoryManager.Controllers
{
    public class VendorController : Controller
    {
        private InventoryContext db = new InventoryContext();
        // GET: HardwareType
        public ActionResult Index()
        {
            var vendors = db.Vendors
                .Select(h => new VendorTable()
                {
                    Vendor = h,
                    Count = h.Inventories.Count
                });

            var viewModel = new VendorTableView()
            {
                VendorTables = vendors.ToList()
            };
            
            return View(viewModel);
        }

        public ActionResult ListInventories(int? Id)
        {
            if (Id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var vendor = db.Vendors.Find(Id);

            if(vendor == null) return new HttpNotFoundResult();

            var inventories = db.Inventories.Where(h => h.VendorId == vendor.Id)
                .Include(i => i.Owner)
                .ToList();

            var viewModel = new VendorInventoryList()
            {
                Inventories = inventories,
                Vendor = vendor
            };
            return View(viewModel);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}