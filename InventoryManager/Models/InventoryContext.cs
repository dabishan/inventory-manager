using System.Data.Entity;


namespace InventoryManager.Models
{
    public class InventoryContext : DbContext
    {
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Hardware> Hardwares { get; set; }
        public DbSet<Software> Softwares { get; set; }
        public DbSet<Maker> Makers { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<HardwareType> HardwareTypes { get; set; }
        public DbSet<SoftwareType> SoftwareTypes { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<GbhContact> GbhgContacts { get; set; }

        public InventoryContext() : base("name=InventoryManagerContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
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