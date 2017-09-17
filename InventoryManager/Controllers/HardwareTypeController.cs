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
    public class HardwareTypeController : Controller
    {
        private InventoryContext db = new InventoryContext();
        // GET: HardwareType
        public ActionResult Index()
        {
            var hardwareTypes = db.HardwareTypes
                .Select(h => new HardwareTypeTable()
                {
                    HardwareType = h,
                    Count = h.Hardwares.Count
                });

            var viewModel = new HardwareTypeTableView()
            {
                HardwareTypeTables = hardwareTypes.ToList()
            };
            
            return View(viewModel);
        }

        public ActionResult ListInventories(int? Id)
        {
            if (Id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var hardwareType = db.HardwareTypes.Find(Id);

            if(hardwareType == null) return new HttpNotFoundResult();

            var hardwares = db.Hardwares.Where(h => h.HardwareTypeId == hardwareType.Id)
                .Include(i => i.Inventory.Owner)
                .ToList();

            var viewModel = new HardwareTypeHardwareList()
            {
                Hardwares = hardwares,
                HardwareType = hardwareType
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