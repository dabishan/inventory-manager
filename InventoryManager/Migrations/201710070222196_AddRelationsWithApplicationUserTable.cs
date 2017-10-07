namespace InventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRelationsWithApplicationUserTable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Histories", new[] { "AssignedBy_Id" });
            DropColumn("dbo.Histories", "AssignedById");
            RenameColumn(table: "dbo.Histories", name: "AssignedBy_Id", newName: "AssignedById");
            AlterColumn("dbo.Histories", "AssignedById", c => c.String(maxLength: 128));
            CreateIndex("dbo.Histories", "AssignedById");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Histories", new[] { "AssignedById" });
            AlterColumn("dbo.Histories", "AssignedById", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Histories", name: "AssignedById", newName: "AssignedBy_Id");
            AddColumn("dbo.Histories", "AssignedById", c => c.Int(nullable: false));
            CreateIndex("dbo.Histories", "AssignedBy_Id");
        }
    }
}
