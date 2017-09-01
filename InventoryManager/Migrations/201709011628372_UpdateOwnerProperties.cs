namespace InventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOwnerProperties : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Locations", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Contacts", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.Contacts", new[] { "Customer_Id" });
            DropIndex("dbo.Locations", new[] { "Customer_Id" });
            RenameColumn(table: "dbo.Locations", name: "Customer_Id", newName: "CustomerId");
            RenameColumn(table: "dbo.Contacts", name: "Customer_Id", newName: "CustomerId");
            AlterColumn("dbo.Contacts", "CustomerId", c => c.Int(nullable: false));
            AlterColumn("dbo.Locations", "CustomerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Contacts", "CustomerId");
            CreateIndex("dbo.Locations", "CustomerId");
            AddForeignKey("dbo.Locations", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Contacts", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contacts", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Locations", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Locations", new[] { "CustomerId" });
            DropIndex("dbo.Contacts", new[] { "CustomerId" });
            AlterColumn("dbo.Locations", "CustomerId", c => c.Int());
            AlterColumn("dbo.Contacts", "CustomerId", c => c.Int());
            RenameColumn(table: "dbo.Contacts", name: "CustomerId", newName: "Customer_Id");
            RenameColumn(table: "dbo.Locations", name: "CustomerId", newName: "Customer_Id");
            CreateIndex("dbo.Locations", "Customer_Id");
            CreateIndex("dbo.Contacts", "Customer_Id");
            AddForeignKey("dbo.Contacts", "Customer_Id", "dbo.Customers", "Id");
            AddForeignKey("dbo.Locations", "Customer_Id", "dbo.Customers", "Id");
        }
    }
}
