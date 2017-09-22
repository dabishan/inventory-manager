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
    public class MakerController : Controller
    {
        private InventoryContext db = new InventoryContext();
        // GET: HardwareType
        public ActionResult Index(MakerTableView viewModel)
        {
            viewModel.MakerTables = db.Makers
                .Select(h => new MakerTable()
                {
                    Maker = h,
                    Count = h.Inventories.Count
                })
                .OrderBy(m => m.Maker.Name)
                .ToPagedList(viewModel.Page, viewModel.PageSize);

            return View(viewModel);
        }

        public ActionResult ListInventories(int? Id, MakerInventoryList viewModel)
        {
            if (Id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var maker = db.Makers.Find(Id);

            if(maker == null) return new HttpNotFoundResult();

            viewModel.Maker = maker;

            viewModel.Inventories = db.Inventories
                .Where(h => h.MakerId == maker.Id)
                .Include(i => i.Owner)
                .OrderBy(m => m.Name)
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