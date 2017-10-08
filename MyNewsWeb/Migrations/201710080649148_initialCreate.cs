namespace MyNewsWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialCreate : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AspNetUsers", new[] { "UserInfo_Id" });
            AlterColumn("dbo.AspNetUsers", "UserInfo_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "UserInfo_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AspNetUsers", new[] { "UserInfo_Id" });
            AlterColumn("dbo.AspNetUsers", "UserInfo_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "UserInfo_Id");
        }
    }
}
