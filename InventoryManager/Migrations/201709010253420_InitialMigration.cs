namespace InventoryManager.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Owner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Owners", t => t.Owner_Id)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Street = c.String(),
                        Unit = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                        Phone = c.String(),
                        Fax = c.String(),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.Owners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OwnerType = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GbhContacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Owner_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Owners", t => t.Owner_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Owner_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        PassCode = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Hardwares",
                c => new
                    {
                        HardwareId = c.Int(nullable: false, identity: true),
                        HardwareTypeId = c.Int(nullable: false),
                        WarrentyExpiration = c.DateTime(),
                        Inventory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.HardwareId)
                .ForeignKey("dbo.HardwareTypes", t => t.HardwareTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Inventories", t => t.Inventory_Id)
                .Index(t => t.HardwareTypeId)
                .Index(t => t.Inventory_Id);
            
            CreateTable(
                "dbo.HardwareTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inventories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 42),
                        Description = c.String(nullable: false, maxLength: 400),
                        ModelName = c.String(maxLength: 80),
                        ModelYear = c.Int(),
                        ModelNo = c.String(maxLength: 80),
                        SerialNo = c.String(maxLength: 80),
                        InventoryType = c.Int(nullable: false),
                        PurchasePrice = c.Double(nullable: false),
                        PurchaseDate = c.DateTime(),
                        MakerId = c.Int(nullable: false),
                        VendorId = c.Int(nullable: false),
                        OwnerId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Makers", t => t.MakerId, cascadeDelete: true)
                .ForeignKey("dbo.Owners", t => t.OwnerId, cascadeDelete: true)
                .ForeignKey("dbo.Vendors", t => t.VendorId, cascadeDelete: true)
                .Index(t => t.MakerId)
                .Index(t => t.VendorId)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.Makers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 80),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vendors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 80),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Softwares",
                c => new
                    {
                        SoftwareId = c.Int(nullable: false, identity: true),
                        IsSubscription = c.Boolean(nullable: false),
                        ExpirationDate = c.DateTime(),
                        Inventory_Id = c.Int(),
                        SoftwareType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.SoftwareId)
                .ForeignKey("dbo.Inventories", t => t.Inventory_Id)
                .ForeignKey("dbo.SoftwareTypes", t => t.SoftwareType_Id)
                .Index(t => t.Inventory_Id)
                .Index(t => t.SoftwareType_Id);
            
            CreateTable(
                "dbo.SoftwareTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 80),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Softwares", "SoftwareType_Id", "dbo.SoftwareTypes");
            DropForeignKey("dbo.Softwares", "Inventory_Id", "dbo.Inventories");
            DropForeignKey("dbo.Hardwares", "Inventory_Id", "dbo.Inventories");
            DropForeignKey("dbo.Inventories", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.Inventories", "OwnerId", "dbo.Owners");
            DropForeignKey("dbo.Inventories", "MakerId", "dbo.Makers");
            DropForeignKey("dbo.Hardwares", "HardwareTypeId", "dbo.HardwareTypes");
            DropForeignKey("dbo.GbhContacts", "User_Id", "dbo.Users");
            DropForeignKey("dbo.GbhContacts", "Owner_Id", "dbo.Owners");
            DropForeignKey("dbo.Customers", "Owner_Id", "dbo.Owners");
            DropForeignKey("dbo.Locations", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Contacts", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.Softwares", new[] { "SoftwareType_Id" });
            DropIndex("dbo.Softwares", new[] { "Inventory_Id" });
            DropIndex("dbo.Inventories", new[] { "OwnerId" });
            DropIndex("dbo.Inventories", new[] { "VendorId" });
            DropIndex("dbo.Inventories", new[] { "MakerId" });
            DropIndex("dbo.Hardwares", new[] { "Inventory_Id" });
            DropIndex("dbo.Hardwares", new[] { "HardwareTypeId" });
            DropIndex("dbo.GbhContacts", new[] { "User_Id" });
            DropIndex("dbo.GbhContacts", new[] { "Owner_Id" });
            DropIndex("dbo.Locations", new[] { "Customer_Id" });
            DropIndex("dbo.Contacts", new[] { "Customer_Id" });
            DropIndex("dbo.Customers", new[] { "Owner_Id" });
            DropTable("dbo.SoftwareTypes");
            DropTable("dbo.Softwares");
            DropTable("dbo.Vendors");
            DropTable("dbo.Makers");
            DropTable("dbo.Inventories");
            DropTable("dbo.HardwareTypes");
            DropTable("dbo.Hardwares");
            DropTable("dbo.Users");
            DropTable("dbo.GbhContacts");
            DropTable("dbo.Owners");
            DropTable("dbo.Locations");
            DropTable("dbo.Contacts");
            DropTable("dbo.Customers");
        }
    }
}
