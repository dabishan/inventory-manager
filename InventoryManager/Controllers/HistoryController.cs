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
    public class HistoryController : Controller
    {
        private InventoryContext db = new InventoryContext();
        // GET: HardwareType
        public ActionResult Index(int? Id, HistoryList viewModel)
        {
            if(Id == null) return new HttpNotFoundResult();

            var inventory = db.Inventories
                .Include(i => i.Owner)
                .SingleOrDefault(i => i.Id == Id);

            if(inventory == null) return new HttpNotFoundResult();
            viewModel.Inventory = inventory;

            viewModel.Histories = db.Histories
                /*.Include(i => i.AssignedBy)*/
                .Include(i => i.AssignedTo)
                .Where(i => i.InventoryId == Id)
                .OrderByDescending(i => i.AssignedOn)
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