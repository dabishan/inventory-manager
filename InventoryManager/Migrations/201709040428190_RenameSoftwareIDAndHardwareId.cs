namespace InventoryManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameSoftwareIDAndHardwareId : DbMigration
    {
        public override void Up()
        {

            RenameColumn("dbo.Hardwares", "HardwareId", "Id");
            RenameColumn("dbo.Softwares", "SoftwareId", "Id");
        }
        
        public override void Down()
        {

            RenameColumn("dbo.Hardwares", "Id", "HardwareId");
            RenameColumn("dbo.Softwares", "Id", "SoftwareId");
        }
    }
}
