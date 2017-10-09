﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using InventoryManager.Models;
using PagedList;

namespace InventoryManager.ViewModel
{
    public class HardwareData : ViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "Name Cannot Be Empty")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Description Cannot Be Empty")]
        public string Description { get; set; }

        [Display(Name = "Model Name")]
        public string ModelName { get; set; }

        [Display(Name = "Model Year")]
        [Range(1900,2100, ErrorMessage = "Please enter a valid Year")]
        public int? ModelYear { get; set; }

        [Display(Name = "Model No")]
        public string ModelNo { get; set; }

        [Display(Name = "Serial No")]
        public string SerialNo { get; set; }

        [Display(Name = "Purchase Price")]
        [Required(ErrorMessage = "Price Cannot Be Empty")]
        [DataType(DataType.Currency)]
        public double PurchasePrice { get; set; }

        [Display(Name = "Purchase Date")]
        [DataType(DataType.DateTime)]
        public DateTime? PurchaseDate { get; set; }

        [Required(ErrorMessage = "Maker Cannot Be Empty")]
        public int MakerId { get; set; }

        [Required(ErrorMessage = "Vendor Cannot Be Empty")]
        public int VendorId { get; set; }

        [Display(Name = "Current Owner")]
        [Required(ErrorMessage = "Owner Cannot Be Empty")]
        public int OwnerId { get; set; }

        [Display(Name = "Inventory Status")]
        [Required(ErrorMessage = "Status Cannot Be Empty")]
        public InventoryStatus Status { get; set; }

        [Display(Name = "Hardware Type")]
        [Required(ErrorMessage = "Hardware Type Cannot Be Empty")]
        public int HardwareTypeId { get; set; }

        [Display(Name = "Warrenty Expiration")]
        [DataType(DataType.DateTime, ErrorMessage = "Enter a Valid Date")]
        public DateTime? WarrentyExpiration { get; set; }

        public IEnumerable<HardwareType> HardwareTypes { get; set; }
        public IEnumerable<Maker> Makers { get; set; }
        public IEnumerable<Vendor> Vendors { get; set; }
        public IEnumerable<Owner> Owners { get; set; }
    }

    public class HardwareList : ViewModel
    {
        public IPagedList<Hardware> Hardwares;
    }

    public class HardwareView : ViewModel
    {
        public Hardware Hardware;
    }

    public class HardwareTypeTable
    {
        public HardwareType HardwareType { get; set; }
        public int Count { get; set; }
    }

    public class HardwareTypeTableView : ViewModel
    {
        public IPagedList<HardwareTypeTable> HardwareTypeTables { get; set; }
    }

    public class HardwareTypeHardwareList :ViewModel
    {
        public HardwareType HardwareType;
        public IPagedList<Hardware> Hardwares { get; set; }
    }

    public class DocumentJson : JsonResult
    {
        public HttpPostedFileBase File { get; set; }
        public string Name { get; set; }
    }

    public class DocumentListJson : JsonResult
    {
        public int InventoryId { get; set; }
        public ICollection<Document> Documents { get; set; }
    }
}