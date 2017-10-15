namespace MyNewsWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNewsComments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewsComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Comment = c.String(),
                        UserInfoId = c.Int(),
                        GoodNewId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GoodNews", t => t.GoodNewId, cascadeDelete: true)
                .ForeignKey("dbo.UserInfoes", t => t.UserInfoId)
                .Index(t => t.UserInfoId)
                .Index(t => t.GoodNewId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NewsComments", "UserInfoId", "dbo.UserInfoes");
            DropForeignKey("dbo.NewsComments", "GoodNewId", "dbo.GoodNews");
            DropIndex("dbo.NewsComments", new[] { "GoodNewId" });
            DropIndex("dbo.NewsComments", new[] { "UserInfoId" });
            DropTable("dbo.NewsComments");
        }
    }
}
