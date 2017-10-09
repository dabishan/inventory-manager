namespace InventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameGbhContactToEmployee : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.GbhContacts", newName: "Employees");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Employees", newName: "GbhContacts");
        }
    }
}
