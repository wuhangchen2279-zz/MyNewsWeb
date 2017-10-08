namespace MyNewsWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class judyupdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GoodNews", "UserInfo_Id", "dbo.UserInfoes");
            DropIndex("dbo.GoodNews", new[] { "UserInfo_Id" });
            RenameColumn(table: "dbo.GoodNews", name: "UserInfo_Id", newName: "UserInfoId");
            AddColumn("dbo.GoodNews", "NewsDate", c => c.DateTime());
            AlterColumn("dbo.GoodNews", "UserInfoId", c => c.Int(nullable: false));
            CreateIndex("dbo.GoodNews", "UserInfoId");
            AddForeignKey("dbo.GoodNews", "UserInfoId", "dbo.UserInfoes", "Id", cascadeDelete: true);
            DropColumn("dbo.GoodNews", "JournalistId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GoodNews", "JournalistId", c => c.Int(nullable: false));
            DropForeignKey("dbo.GoodNews", "UserInfoId", "dbo.UserInfoes");
            DropIndex("dbo.GoodNews", new[] { "UserInfoId" });
            AlterColumn("dbo.GoodNews", "UserInfoId", c => c.Int());
            DropColumn("dbo.GoodNews", "NewsDate");
            RenameColumn(table: "dbo.GoodNews", name: "UserInfoId", newName: "UserInfo_Id");
            CreateIndex("dbo.GoodNews", "UserInfo_Id");
            AddForeignKey("dbo.GoodNews", "UserInfo_Id", "dbo.UserInfoes", "Id");
        }
    }
}
