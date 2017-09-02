using System.Collections.Generic;
using InventoryManager.Models;

namespace InventoryManager.ViewModel
{
    public class CustomerData : ViewModel
    {
        public OwnerType OwnerType { get; set; }
        public Customer Customer { get; set; }
    }

    public class ContactData : ViewModel
    {
        public Contact Contact { get; set; }
    }

    public class OwnerList : ViewModel
    {
        public IList<Owner> Owners { get; set; }
    }
}