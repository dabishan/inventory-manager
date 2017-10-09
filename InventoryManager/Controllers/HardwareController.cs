using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using InventoryManager.Models;
using InventoryManager.ViewModel;
using Microsoft.AspNet.Identity;
using PagedList;

namespace InventoryManager.Controllers
{
    public class HardwareController : Controller
    {
        private InventoryContext db = new InventoryContext();
        
        // GET: Inventory
        public ActionResult Index(HardwareList viewModel)
        {
            viewModel.Hardwares = db.Hardwares
                .Include(h => h.Inventory)
                .Include(h => h.Inventory.Maker)
                .OrderBy(i => i.Inventory.Name)
                .ToPagedList(viewModel.Page, viewModel.PageSize);
            
            return View(viewModel);
        }

        public ActionResult Add()
        {
            var viewModel = new HardwareData();

            viewModel.HardwareTypes = db.HardwareTypes.Where(h => h.Status == true);
            viewModel.Vendors = db.Vendors.Where(v => v.Status == true);
            viewModel.Makers = db.Makers.Where(m => m.Status == true);
            viewModel.Owners = db.Owners.ToList();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(HardwareData h)
        {
            var hardware = new Hardware() { Inventory = new Inventory()};

            if(User.IsInRole(ApplicationUser.USER))
            {
                var userId = User.Identity.GetUserId();
                var employee = db.Employees
                    .Include(e => e.Owner)
                    .SingleOrDefault(e => e.User.Id == userId);
                if(employee == null ) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                h.OwnerId = employee.Owner.Id;
                h.Status = InventoryStatus.Pending;
            }

            if (!ModelState.IsValid)
            {
                PopulateHardware(hardware, h);
                h.HardwareTypes = db.HardwareTypes.Where(i => i.Status);
                h.Vendors = db.Vendors.Where(i => i.Status);
                h.Makers = db.Makers.Where(i => i.Status);
                h.Owners = db.Owners.ToList();

                return View(h);
            }

            UpdateHistory(hardware, h);
            MapHardware(hardware, h);
            hardware.Inventory.CreatedOn = DateTime.Now;

            db.Hardwares.Add(hardware);
            db.SaveChanges();

            TempData["AlertMessage"] = "Your Hardware is Saved.";
            return RedirectToAction("Show", new { Id = hardware.Id });
        }

        public void MapHardware(Hardware hardwareToUpdate, HardwareData data)
        {
            hardwareToUpdate.Inventory.Name = data.Name;
            hardwareToUpdate.Inventory.Description = data.Description;
            hardwareToUpdate.Inventory.ModelName = data.ModelName;
            hardwareToUpdate.Inventory.ModelNo = data.ModelNo;
            hardwareToUpdate.Inventory.ModelYear = data.ModelYear;
            hardwareToUpdate.Inventory.PurchasePrice = data.PurchasePrice;
            hardwareToUpdate.Inventory.PurchaseDate = data.PurchaseDate;
            hardwareToUpdate.Inventory.SerialNo = data.SerialNo;
            hardwareToUpdate.Inventory.MakerId = data.MakerId;
            hardwareToUpdate.Inventory.VendorId = data.VendorId;
            hardwareToUpdate.Inventory.OwnerId = data.OwnerId;
            hardwareToUpdate.Inventory.Status = data.Status == 0 ? InventoryStatus.Pending : data.Status;
            hardwareToUpdate.Inventory.ModifiedOn = DateTime.Now;
            
            hardwareToUpdate.HardwareTypeId = data.HardwareTypeId;
            hardwareToUpdate.WarrentyExpiration = data.WarrentyExpiration;
        }

        public void PopulateHardware(Hardware hardware, HardwareData hardwareToDisplay)
        {
            hardwareToDisplay.Name = hardware.Inventory.Name;
            hardwareToDisplay.Description = hardware.Inventory.Description;
            hardwareToDisplay.ModelName = hardware.Inventory.ModelName;
            hardwareToDisplay.ModelNo = hardware.Inventory.ModelNo;
            hardwareToDisplay.ModelYear = hardware.Inventory.ModelYear;
            hardwareToDisplay.PurchasePrice = hardware.Inventory.PurchasePrice;
            hardwareToDisplay.PurchaseDate = hardware.Inventory.PurchaseDate;
            hardwareToDisplay.SerialNo = hardware.Inventory.SerialNo;
            hardwareToDisplay.MakerId = hardware.Inventory.MakerId;
            hardwareToDisplay.VendorId = hardware.Inventory.VendorId;
            hardwareToDisplay.OwnerId = hardware.Inventory.OwnerId;
            hardwareToDisplay.Status = hardware.Inventory.Status;

            hardwareToDisplay.HardwareTypeId = hardware.HardwareTypeId;
            hardwareToDisplay.WarrentyExpiration = hardware.WarrentyExpiration;
        }

        public void UpdateHistory(Hardware hardware, HardwareData data)
        {
            if (data.Status != hardware.Inventory.Status
                || data.OwnerId != hardware.Inventory.OwnerId)
            {
                var history = new History()
                {
                    AssignedOn = DateTime.Now,
                    AssignedToId = data.OwnerId,
                    InventoryId = hardware.Inventory.Id,
                    StatusAssigned = data.Status,
                    AssignedById = User.Identity.GetUserId()
                };

                db.Histories.Add(history);
            }
        }
        public ActionResult Edit(int? Id)
        {
            if (Id == null ) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var hardware = db.Hardwares
                .Include(i => i.Inventory)
                .SingleOrDefault(i => i.Id == Id);

            if(hardware == null) return new HttpNotFoundResult();

            var userId = User.Identity.GetUserId();
            var employee = db.Employees
                .Include(e => e.Owner)
                .SingleOrDefault(i => i.User.Id == userId);
            if (employee == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (User.IsInRole(ApplicationUser.USER))
            {
                if (hardware.Inventory.Status != InventoryStatus.Pending || hardware.Inventory.OwnerId != employee.Owner.Id)
                {
                    return new HttpUnauthorizedResult();
                }
            }
            
            var viewModel = new HardwareData();
            
            PopulateHardware(hardware, viewModel);
            viewModel.HardwareTypes = db.HardwareTypes.Where(h => h.Status == true);
            viewModel.Vendors = db.Vendors.Where(v => v.Status == true);
            viewModel.Makers = db.Makers.Where(m => m.Status == true);
            viewModel.Owners = db.Owners.ToList();

            viewModel.ReferrerUrl = Request.UrlReferrer?.AbsoluteUri;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HardwareData h)
        {
            var hardware = db.Hardwares
                .Include(i => i.Inventory)
                .SingleOrDefault(i => i.Id == h.Id);
            if (hardware == null) return new HttpNotFoundResult();

            var userId = User.Identity.GetUserId();
            var employee = db.Employees
                .Include(e => e.Owner)
                .SingleOrDefault(i => i.User.Id == userId);

            if (employee == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (User.IsInRole(ApplicationUser.USER))
            {
                if (hardware.Inventory.Status != InventoryStatus.Pending || hardware.Inventory.OwnerId != employee.Owner.Id)
                {
                    return new HttpUnauthorizedResult();
                }

                h.Status = InventoryStatus.Pending;
                h.OwnerId = employee.Owner.Id;
            }

            if (!ModelState.IsValid)
            {
                PopulateHardware(hardware, h);
                h.HardwareTypes = db.HardwareTypes.Where(i => i.Status);
                h.Vendors = db.Vendors.Where(i => i.Status);
                h.Makers = db.Makers.Where(i => i.Status);
                h.Owners = db.Owners.ToList();

                return View(h);
            }

            UpdateHistory(hardware, h);
            MapHardware(hardware, h);
            db.SaveChanges();

            TempData["AlertMessage"] = "Your Hardware is Saved.";
            return RedirectToAction("Show", new { Id = hardware.Id });
        }

        public ActionResult Show(int? Id)
        {
            if (Id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var viewModel = new HardwareView();

            var hardware = db.Hardwares
                .Include(h => h.Inventory)
                .Include(h => h.Inventory.Maker)
                .Include(h => h.Inventory.Vendor)
                .Include(h => h.HardwareType)
                .Include(h => h.Inventory.Owner)
                .Include(h => h.Inventory.Documents)
                .SingleOrDefault(h => h.Id == Id);

            var userId = User.Identity.GetUserId();
            var employee = db.Employees
                .Include(e => e.Owner)
                .SingleOrDefault(i => i.User.Id == userId);

            if (User.IsInRole(ApplicationUser.USER) && hardware.Inventory.OwnerId != employee.Owner.Id)
            {
                return new HttpUnauthorizedResult();
            }

            if (hardware == null) return new HttpNotFoundResult();

            viewModel.Hardware = hardware;

            return View(viewModel);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}