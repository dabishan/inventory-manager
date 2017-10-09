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
    [Authorize(Roles = "InventoryManager, InventoryCustomerManager, InventoryAdmin")]
    public class HardwareTypeController : Controller
    {
        private InventoryContext db = new InventoryContext();
        // GET: HardwareType
        public ActionResult Index(HardwareTypeTableView viewModel)
        {
            viewModel.HardwareTypeTables = db.HardwareTypes
                .Select(h => new HardwareTypeTable()
                {
                    HardwareType = h,
                    Count = h.Hardwares.Count
                })
                .OrderBy(h => h.HardwareType.Name)
                .ToPagedList(viewModel.Page, viewModel.PageSize);

            return View(viewModel);
        }

        public ActionResult ListInventories(int? Id, HardwareTypeHardwareList viewModel)
        {
            if (Id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var hardwareType = db.HardwareTypes.Find(Id);

            if(hardwareType == null) return new HttpNotFoundResult();

            viewModel.Hardwares = db.Hardwares.Where(h => h.HardwareTypeId == hardwareType.Id)
                    .Include(i => i.Inventory.Owner)
                    .OrderBy(h => h.Inventory.Name)
                    .ToPagedList(viewModel.Page, viewModel.PageSize);

            viewModel.HardwareType = hardwareType;

            return View(viewModel);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}