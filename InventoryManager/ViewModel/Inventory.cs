using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventoryManager.Models;

namespace InventoryManager.ViewModel
{
    public class HardwareData : ViewModel
    {
        public Hardware Hardware { get; set; }
        public IEnumerable<HardwareType> HardwareTypes { get; set; }
        public IEnumerable<Maker> Makers { get; set; }
        public IEnumerable<Vendor> Vendors { get; set; }

    }

    public class HardwareList : ViewModel
    {
        public IList<Hardware> Hardwares;
    }
}