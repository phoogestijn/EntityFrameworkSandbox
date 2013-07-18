namespace EntityFrameworkTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLicensePlate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Arrangements", "LicensePlate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Arrangements", "LicensePlate");
        }
    }
}
