using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventoryManager.Models;

namespace InventoryManager.ViewModel
{
    public class SearchAllView
    {
        public string Query { get; set; }
        public ICollection<Hardware> Hardwares { get; set; }
        public ICollection<Customer> Customers { get; set; }
        public ICollection<Contact> Contacts { get; set; }
    }
}
