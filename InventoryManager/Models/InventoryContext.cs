using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace InventoryManager.Models
{
    public class InventoryContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Hardware> Hardwares { get; set; }
        public DbSet<Software> Softwares { get; set; }
        public DbSet<Maker> Makers { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<HardwareType> HardwareTypes { get; set; }
        public DbSet<SoftwareType> SoftwareTypes { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<History> Histories { get; set; }

        public InventoryContext() : base("name=InventoryManagerContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public static InventoryContext Create()
        {
            return new InventoryContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new InventoryConfig());
            modelBuilder.Configurations.Add(new Maker.MakerConfig());
            modelBuilder.Configurations.Add(new Vendor.VendorConfig());
            modelBuilder.Configurations.Add(new Hardware.HardwareConfig());
            modelBuilder.Configurations.Add(new SoftwareType.SoftwareTypeConfig());

            base.OnModelCreating(modelBuilder);
        }
    }
}