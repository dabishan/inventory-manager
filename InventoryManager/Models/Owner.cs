using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventoryManager.Models
{
    public enum OwnerType
    {
        [Display(Name = "GBH Employee")]
        GbhEmployee,

        [Display(Name = "Customer")]
        Customer
    }

    public class Owner
    {
        public int Id { get; set; }
        public OwnerType OwnerType { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class GbhContact
    {
        public int Id { get; set; }
        public User User { get; set; }

        public Owner Owner { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Owner Owner { get; set; }

        public ICollection<Contact> Contact { get; set; }
        public ICollection<Location> Locations { get; set; }
    }


    public class Location
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string Unit { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }

    }

    public class Contact
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}