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
    public class MakerController : Controller
    {
        private InventoryContext db = new InventoryContext();
        // GET: HardwareType
        public ActionResult Index()
        {
            var makers = db.Makers
                .Select(h => new MakerTable()
                {
                    Maker = h,
                    Count = h.Inventories.Count
                });

            var viewModel = new MakerTableView()
            {
                MakerTables = makers.ToList()
            };
            
            return View(viewModel);
        }

        public ActionResult ListInventories(int? Id)
        {
            if (Id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var maker = db.Makers.Find(Id);

            if(maker == null) return new HttpNotFoundResult();

            var inventories = db.Inventories.Where(h => h.MakerId == maker.Id)
                .Include(i => i.Owner)
                .ToList();

            var viewModel = new MakerInventoryList()
            {
                Inventories = inventories,
                Maker = maker
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