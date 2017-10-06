namespace InventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDocumentTablesAndRelations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UploadedOn = c.DateTime(nullable: false),
                        Name = c.String(),
                        FileType = c.String(),
                        Path = c.String(),
                        InventoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inventories", t => t.InventoryId, cascadeDelete: true)
                .Index(t => t.InventoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "InventoryId", "dbo.Inventories");
            DropIndex("dbo.Documents", new[] { "InventoryId" });
            DropTable("dbo.Documents");
        }
    }
}
