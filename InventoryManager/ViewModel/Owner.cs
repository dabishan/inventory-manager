﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using InventoryManager.Models;

namespace InventoryManager.ViewModel
{
    public class CustomerData : ViewModel
    {
        public OwnerType OwnerType { get; set; }

        public int Id { get; set; } 

        [Required(ErrorMessage = "Name Can't be Empty")]
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Location> Locations { get; set; }
        public ICollection<Contact> Contacts { get; set; }

        public int OwnerId { get; set; }
        public Owner Owner { get; set; }
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

    public class LocationData : ViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Street Can't Be Empty")]
        public string Street { get; set; }

        public string Unit { get; set; }

        [Required(ErrorMessage = "City Can't Be Empty")]
        public string City { get; set; }

        [Required(ErrorMessage = "State Can't Be Empty")]
        public string State { get; set; }

        [Display(Name = "Zipcode")]
        [Required(ErrorMessage = "Zipcode Can't Be Empty")]
        public string Zip { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public int CustomerId { get; set; }

    }

    public class OwnerList : ViewModel
    {
        public IList<Owner> Owners { get; set; }
    }
}