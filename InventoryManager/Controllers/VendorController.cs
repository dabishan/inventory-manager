using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryManager.Models;
using InventoryManager.ViewModel;
using PagedList;

namespace InventoryManager.Controllers
{
    public class VendorController : Controller
    {
        private InventoryContext db = new InventoryContext();
        // GET: HardwareType
        public ActionResult Index(VendorTableView viewModel)
        {

            viewModel.VendorTables = db.Vendors
                .Select(h => new VendorTable()
                {
                    Vendor = h,
                    Count = h.Inventories.Count
                })
                .OrderBy(v => v.Vendor.Name)
                .ToPagedList(viewModel.Page, viewModel.PageSize);
            
            return View(viewModel);
        }

        public ActionResult ListInventories(int? Id, VendorInventoryList viewModel)
        {
            if (Id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var vendor = db.Vendors.Find(Id);

            if(vendor == null) return new HttpNotFoundResult();

            viewModel.Vendor = vendor;

            viewModel.Inventories = db.Inventories.Where(h => h.VendorId == vendor.Id)
                .Include(i => i.Owner)
                .OrderBy(v => v.Name)
                .ToPagedList(viewModel.Page, viewModel.PageSize);

            return View(viewModel);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}