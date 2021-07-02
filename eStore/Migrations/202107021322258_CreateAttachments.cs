namespace eStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateAttachments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            DropColumn("dbo.Products", "ImageUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ImageUrl", c => c.String());
            DropForeignKey("dbo.Attachments", "ProductId", "dbo.Products");
            DropIndex("dbo.Attachments", new[] { "ProductId" });
            DropTable("dbo.Attachments");
        }
    }
}
