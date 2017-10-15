namespace MyNewsWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userinforidrequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GoodNews", "UserInfoId", "dbo.UserInfoes");
            DropIndex("dbo.GoodNews", new[] { "UserInfoId" });
            AlterColumn("dbo.GoodNews", "UserInfoId", c => c.Int(nullable: false));
            CreateIndex("dbo.GoodNews", "UserInfoId");
            AddForeignKey("dbo.GoodNews", "UserInfoId", "dbo.UserInfoes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GoodNews", "UserInfoId", "dbo.UserInfoes");
            DropIndex("dbo.GoodNews", new[] { "UserInfoId" });
            AlterColumn("dbo.GoodNews", "UserInfoId", c => c.Int());
            CreateIndex("dbo.GoodNews", "UserInfoId");
            AddForeignKey("dbo.GoodNews", "UserInfoId", "dbo.UserInfoes", "Id");
        }
    }
}
