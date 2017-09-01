namespace InventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameCreateDateAndModifiedDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Owners", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Owners", "ModifiedOn", c => c.DateTime(nullable: false));
            DropColumn("dbo.Owners", "CreateDate");
            DropColumn("dbo.Owners", "ModifiedDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Owners", "ModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Owners", "CreateDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Owners", "ModifiedOn");
            DropColumn("dbo.Owners", "CreatedOn");
        }
    }
}
