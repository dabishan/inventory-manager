using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventoryManager.Models;
using PagedList;

namespace InventoryManager.ViewModel
{
    public class HistoryList : ViewModel
    {
        public IPagedList<History> Histories { get; set; }
        public Inventory Inventory { get; set; }
    }
}