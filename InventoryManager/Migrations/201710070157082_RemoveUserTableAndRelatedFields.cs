namespace InventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUserTableAndRelatedFields : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Histories", "AssignedById", "dbo.Users");
            DropForeignKey("dbo.GbhContacts", "User_Id", "dbo.Users");
            DropIndex("dbo.Histories", new[] { "AssignedById" });
            DropIndex("dbo.GbhContacts", new[] { "User_Id" });
            DropColumn("dbo.Histories", "AssignedById");
            DropColumn("dbo.GbhContacts", "User_Id");
            DropTable("dbo.Users");
        }
        
        public override void Down()
        {
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
            
            AddColumn("dbo.GbhContacts", "User_Id", c => c.Int());
            AddColumn("dbo.Histories", "AssignedById", c => c.Int(nullable: false));
            CreateIndex("dbo.GbhContacts", "User_Id");
            CreateIndex("dbo.Histories", "AssignedById");
            AddForeignKey("dbo.GbhContacts", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Histories", "AssignedById", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
