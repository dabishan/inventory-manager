namespace InventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoveNameFromCustomerToOwnerTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Owners", "Name", c => c.String());
            DropColumn("dbo.Customers", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "Name", c => c.String());
            DropColumn("dbo.Owners", "Name");
        }
    }
}
