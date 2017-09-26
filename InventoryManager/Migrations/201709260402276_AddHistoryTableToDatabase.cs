namespace InventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHistoryTableToDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Histories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InventoryId = c.Int(nullable: false),
                        AssignedOn = c.DateTime(nullable: false),
                        StatusAssigned = c.Int(nullable: false),
                        AssignedBy_Id = c.Int(),
                        AssignedTo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AssignedBy_Id)
                .ForeignKey("dbo.Owners", t => t.AssignedTo_Id)
                .ForeignKey("dbo.Inventories", t => t.InventoryId, cascadeDelete: true)
                .Index(t => t.InventoryId)
                .Index(t => t.AssignedBy_Id)
                .Index(t => t.AssignedTo_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Histories", "InventoryId", "dbo.Inventories");
            DropForeignKey("dbo.Histories", "AssignedTo_Id", "dbo.Owners");
            DropForeignKey("dbo.Histories", "AssignedBy_Id", "dbo.Users");
            DropIndex("dbo.Histories", new[] { "AssignedTo_Id" });
            DropIndex("dbo.Histories", new[] { "AssignedBy_Id" });
            DropIndex("dbo.Histories", new[] { "InventoryId" });
            DropTable("dbo.Histories");
        }
    }
}
