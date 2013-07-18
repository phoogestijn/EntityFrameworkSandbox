namespace EntityFrameworkTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Arrangements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        BpNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArrangementVersions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Version = c.Int(nullable: false),
                        Arrangement_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Arrangements", t => t.Arrangement_Id, cascadeDelete: true)
                .Index(t => t.Arrangement_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        FullName = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Privileges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PrivilegeUsers",
                c => new
                    {
                        Privilege_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Privilege_Id, t.User_Id })
                .ForeignKey("dbo.Privileges", t => t.Privilege_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Privilege_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.ArrangementUsers",
                c => new
                    {
                        Arrangement = c.Int(nullable: false),
                        User = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Arrangement, t.User })
                .ForeignKey("dbo.Arrangements", t => t.Arrangement, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User, cascadeDelete: true)
                .Index(t => t.Arrangement)
                .Index(t => t.User);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ArrangementUsers", new[] { "User" });
            DropIndex("dbo.ArrangementUsers", new[] { "Arrangement" });
            DropIndex("dbo.PrivilegeUsers", new[] { "User_Id" });
            DropIndex("dbo.PrivilegeUsers", new[] { "Privilege_Id" });
            DropIndex("dbo.ArrangementVersions", new[] { "Arrangement_Id" });
            DropForeignKey("dbo.ArrangementUsers", "User", "dbo.Users");
            DropForeignKey("dbo.ArrangementUsers", "Arrangement", "dbo.Arrangements");
            DropForeignKey("dbo.PrivilegeUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.PrivilegeUsers", "Privilege_Id", "dbo.Privileges");
            DropForeignKey("dbo.ArrangementVersions", "Arrangement_Id", "dbo.Arrangements");
            DropTable("dbo.ArrangementUsers");
            DropTable("dbo.PrivilegeUsers");
            DropTable("dbo.Privileges");
            DropTable("dbo.Users");
            DropTable("dbo.ArrangementVersions");
            DropTable("dbo.Arrangements");
        }
    }
}
