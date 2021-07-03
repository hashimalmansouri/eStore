namespace eStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "PaymentType", c => c.String());
            AddColumn("dbo.AspNetUsers", "Balance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Balance");
            DropColumn("dbo.Orders", "PaymentType");
        }
    }
}
