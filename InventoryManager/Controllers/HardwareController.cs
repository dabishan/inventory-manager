﻿using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
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

            viewModel.HardwareTypes = db.HardwareTypes.Where(h => h.Status == true);
            viewModel.Vendors = db.Vendors.Where(v => v.Status == true);
            viewModel.Makers = db.Makers.Where(m => m.Status == true);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(HardwareData h)
        {
            var hardware = new Hardware() { Inventory = new Inventory()};

            if (!ModelState.IsValid)
            {
                PopulateHardware(hardware, h);
                h.HardwareTypes = db.HardwareTypes.Where(i => i.Status);
                h.Vendors = db.Vendors.Where(i => i.Status);
                h.Makers = db.Makers.Where(i => i.Status);

                return View(h);
            }

            MapHardware(hardware, h);
            hardware.Inventory.CreatedOn = DateTime.Now;

            db.Hardwares.Add(hardware);
            db.SaveChanges();

            TempData["AlertMessage"] = "Your Hardware is Saved.";
            return RedirectToAction("Edit", new { Id = hardware.Id });
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

            hardwareToUpdate.Inventory.OwnerId = 1; // TODO change when Owner Class is Done

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

        public ActionResult Edit(int? Id)
        {
            if (Id == null ) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var hardware = db.Hardwares
                .Include(i => i.Inventory)
                .SingleOrDefault(i => i.Id == Id);

            if(hardware == null) return new HttpNotFoundResult();

            var viewModel = new HardwareData();
            
            PopulateHardware(hardware, viewModel);
            viewModel.HardwareTypes = db.HardwareTypes.Where(h => h.Status == true);
            viewModel.Vendors = db.Vendors.Where(v => v.Status == true);
            viewModel.Makers = db.Makers.Where(m => m.Status == true);

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


            if (!ModelState.IsValid)
            {
                PopulateHardware(hardware, h);
                h.HardwareTypes = db.HardwareTypes.Where(i => i.Status);
                h.Vendors = db.Vendors.Where(i => i.Status);
                h.Makers = db.Makers.Where(i => i.Status);

                return View(h);
            }

            MapHardware(hardware, h);
            db.SaveChanges();

            TempData["AlertMessage"] = "Your Hardware is Saved.";
            return RedirectToAction("Edit", new { Id = hardware.Id });
        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Save(HardwareData h)
//        {
//            var r = Request.Form;
//
//            Hardware hardware;
//            if (ModelState.IsValid == false) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            if (h.Hardware.Id == 0 )
//            {
//                hardware = h.Hardware;
//                db.Hardwares.Add(hardware);
//
//                hardware.Inventory.CreatedOn = DateTime.Now.Date;
//            }
//            else
//            {
//                hardware = db.Hardwares
//                    .Include(i => i.Inventory)
//                    .Single(i => i.HardwareId == h.Hardware.HardwareId);
//
//                if (hardware == null) return new HttpNotFoundResult();
//            }
//
//            hardware.Inventory.InventoryType = InventoryType.Hardware;
//            hardware.Inventory.Owner = db.Owners.Find(1);
//            hardware.Inventory.VendorId = h.Hardware.Inventory.VendorId;
//            hardware.Inventory.MakerId = h.Hardware.Inventory.MakerId;
//            hardware.HardwareTypeId = h.Hardware.HardwareTypeId;
//            hardware.Inventory.ModifiedOn = DateTime.Now.Date;
//
//            hardware.Inventory.Status = InventoryStatus.Pending;
//            db.SaveChanges();
//
//            TempData["AlertMessage"] = "Your Hardware is Saved.";
//            return RedirectToAction("Edit", new { Id = hardware.Id });
//        }

        public ActionResult Show(int? Id)
        {
            if (Id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var viewModel = new HardwareData();

            var hardware = db.Hardwares
                .Include(h => h.Inventory)
                .Include(h => h.Inventory.Maker)
                .Include(h => h.Inventory.Vendor)
                .Include(h => h.HardwareType)
                .Single(h => h.Id == Id);

            PopulateHardware(hardware, viewModel);

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