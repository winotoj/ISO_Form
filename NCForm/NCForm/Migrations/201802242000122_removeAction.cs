namespace NCForm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeAction : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Issue", "Action");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Issue", "Action", c => c.String(maxLength: 2500));
        }
    }
}
