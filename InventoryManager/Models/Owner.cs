using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        public ICollection<Contact> Contacts { get; set; }
        public ICollection<Location> Locations { get; set; }

        public class CustomerConfig : EntityTypeConfiguration<Customer>
        {
            public CustomerConfig()
            {
                Property(c => c.Name)
                    .HasMaxLength(60)
                    .IsRequired();

                Property(c => c.Description)
                    .HasMaxLength(400)
                    .IsRequired();

                HasMany(c => c.Locations)
                    .WithRequired(c => c.Customer)
                    .HasForeignKey(c => c.CustomerId);

                HasMany(c => c.Contacts)
                    .WithRequired(c => c.Customer)
                    .HasForeignKey(c => c.CustomerId);
            }
        }
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

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public class LocationConfig : EntityTypeConfiguration<Location>
        {
            public LocationConfig()
            {
                Property(l => l.Street)
                    .HasMaxLength(150)
                    .IsRequired();

                Property(l => l.Unit)
                    .HasMaxLength(150);

                Property(l => l.City)
                    .HasMaxLength(42)
                    .IsRequired();

                Property(l => l.State)
                    .HasMaxLength(3)
                    .IsRequired();

                Property(l => l.Phone)
                    .HasMaxLength(40);

                Property(l => l.Fax)
                    .HasMaxLength(40);
            }
        }
    }

    public class Contact
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public class ContactConfig : EntityTypeConfiguration<Contact>
        {
            public ContactConfig()
            {
                Property(c => c.FirstName)
                    .HasMaxLength(40)
                    .IsRequired();

                Property(c => c.LastName)
                    .HasMaxLength(40)
                    .IsRequired();

                Property(c => c.Email)
                    .HasMaxLength(40);

                Property(c => c.Email)
                    .HasMaxLength(40);
            }
        }
    }
}