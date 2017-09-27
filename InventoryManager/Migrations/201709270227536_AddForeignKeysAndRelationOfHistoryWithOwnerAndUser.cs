namespace InventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeysAndRelationOfHistoryWithOwnerAndUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Histories", "AssignedBy_Id", "dbo.Users");
            DropForeignKey("dbo.Histories", "AssignedTo_Id", "dbo.Owners");
            DropIndex("dbo.Histories", new[] { "AssignedBy_Id" });
            DropIndex("dbo.Histories", new[] { "AssignedTo_Id" });
            RenameColumn(table: "dbo.Histories", name: "AssignedBy_Id", newName: "AssignedById");
            RenameColumn(table: "dbo.Histories", name: "AssignedTo_Id", newName: "AssignedToId");
            AlterColumn("dbo.Histories", "AssignedById", c => c.Int(nullable: false));
            AlterColumn("dbo.Histories", "AssignedToId", c => c.Int(nullable: false));
            CreateIndex("dbo.Histories", "AssignedById");
            CreateIndex("dbo.Histories", "AssignedToId");
            AddForeignKey("dbo.Histories", "AssignedById", "dbo.Users", "Id");
            AddForeignKey("dbo.Histories", "AssignedToId", "dbo.Owners", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Histories", "AssignedToId", "dbo.Owners");
            DropForeignKey("dbo.Histories", "AssignedById", "dbo.Users");
            DropIndex("dbo.Histories", new[] { "AssignedToId" });
            DropIndex("dbo.Histories", new[] { "AssignedById" });
            AlterColumn("dbo.Histories", "AssignedToId", c => c.Int());
            AlterColumn("dbo.Histories", "AssignedById", c => c.Int());
            RenameColumn(table: "dbo.Histories", name: "AssignedToId", newName: "AssignedTo_Id");
            RenameColumn(table: "dbo.Histories", name: "AssignedById", newName: "AssignedBy_Id");
            CreateIndex("dbo.Histories", "AssignedTo_Id");
            CreateIndex("dbo.Histories", "AssignedBy_Id");
            AddForeignKey("dbo.Histories", "AssignedTo_Id", "dbo.Owners", "Id");
            AddForeignKey("dbo.Histories", "AssignedBy_Id", "dbo.Users", "Id");
        }
    }
}
