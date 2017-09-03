using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int? Id { get; set; } 

        [Required(ErrorMessage = "First Name Cant't Be Empty")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name Cant't Be Empty")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?",
            ErrorMessage = "Email is not Valid")]
        public string Email { get; set; }

        public string Phone { get; set; }

        public int CustomerId { get; set; }
    }

    public class OwnerList : ViewModel
    {
        public IList<Owner> Owners { get; set; }
    }
}