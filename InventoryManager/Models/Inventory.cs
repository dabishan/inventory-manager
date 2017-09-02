using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace InventoryManager.Models
{
    public enum InventoryStatus { Pending, UnAssigned, Active, InActive, Decommissioned }

    public enum InventoryType
    {
        [Display(Name = "Hardware")]
        Hardware,

        [Display(Name="Software")]
        Software
    }


    public class Inventory
    {
        public int Id { get; set; }

        [Display(Name = "Product Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Display(Name = "Model Name")]
        public string ModelName { get; set; }

        [Display(Name = "Model Year")]
        public int? ModelYear { get; set; }

        [Display(Name = "Model No")]
        public string ModelNo { get; set; }

        [Display(Name = "Serial No")]
        public string SerialNo { get; set; }

        [Display(Name = "Inventory Type")]
        public InventoryType InventoryType { get; set; }

        [Display(Name = "Purchase Price")]
        public double PurchasePrice { get; set; }

        [Display(Name = "Purchase Date")]
        public DateTime? PurchaseDate { get; set; }

        public Maker Maker { get; set; }
        public int MakerId { get; set; }

        public Vendor Vendor { get; set; }
        public int VendorId { get; set; }

        [Display(Name = "Current Owner")]
        public Owner Owner { get; set; }
        public int OwnerId { get; set; }

        [Display(Name = "Inventory Status")]
        public InventoryStatus Status { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Modified On")]
        public DateTime ModifiedOn { get; set; }
    }

    public class InventoryConfig : EntityTypeConfiguration<Inventory>
    {
        public InventoryConfig()
        {
            Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(42);

            Property(i => i.Description)
                .IsRequired()
                .HasMaxLength(400);

            Property(i => i.ModelName)
                .HasMaxLength(80);

            Property(i => i.ModelNo)
                .HasMaxLength(80);

            Property(i => i.SerialNo)
                .HasMaxLength(80);

            HasRequired(i => i.Maker)
                .WithMany(i => i.Inventories)
                .HasForeignKey(i => i.MakerId);

            HasRequired(i => i.Vendor)
                .WithMany(i => i.Inventories)
                .HasForeignKey(i => i.VendorId);


        }
    }

    public class Hardware
    {

        [Display(Name = "Hardware Id")]
        public int HardwareId { get; set; }

        [Display(Name = "Hardware Type")]
        public HardwareType HardwareType { get; set; }
        public int HardwareTypeId { get; set; }

        [Display(Name = "Warrenty Expiration")]
        public DateTime? WarrentyExpiration { get; set; }

        public Inventory Inventory { get; set; }

        public class HardwareConfig : EntityTypeConfiguration<Hardware>
        {
            public HardwareConfig()
            {
                HasRequired(i => i.HardwareType)
                    .WithMany(i => i.Hardwares)
                    .HasForeignKey(i => i.HardwareTypeId);
            }
        }
    }

    public class HardwareType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }

        public ICollection<Hardware> Hardwares { get; set; }

        public class HardwareTypeConfig : EntityTypeConfiguration<HardwareType>
        {
            public HardwareTypeConfig()
            {
                Property(h => h.Name).HasMaxLength(80).IsRequired();
            }
        }
    }

    public class Software
    {
        [Display(Name = "Software Id")]
        public int SoftwareId { get; set; }

        [Display(Name = "Software Type")]
        public SoftwareType SoftwareType { get; set; }

        [Display(Name = "Subscription")]
        public bool IsSubscription { get; set; }

        [Display(Name = "Expiration Date")]
        public DateTime? ExpirationDate { get; set; }

        public Inventory Inventory { get; set; }

        public class SoftwareConfig : EntityTypeConfiguration<Hardware>
        {
            public SoftwareConfig() 
            {
                
            }
        }

    }

    public class SoftwareType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }

        public class SoftwareTypeConfig : EntityTypeConfiguration<SoftwareType>
        {
            public SoftwareTypeConfig()
            {
                Property(s => s.Name).HasMaxLength(80).IsRequired(); 
            }
        }
    }

    public class Maker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }

        public ICollection<Inventory> Inventories { get; set; }

        public class MakerConfig : EntityTypeConfiguration<Maker>
        {
            public MakerConfig()
            {
                Property(s => s.Name).HasMaxLength(80).IsRequired();
            }
        }
    }

    public class Vendor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }

        public ICollection<Inventory> Inventories { get; set; }

        public class VendorConfig : EntityTypeConfiguration<Vendor>
        {
            public VendorConfig()
            {
                Property(s => s.Name).HasMaxLength(80).IsRequired();
            }
        }
    }

}