namespace NCForm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.History",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false, maxLength: 2500),
                        MsgDate = c.DateTime(nullable: false),
                        Creator = c.String(),
                        FileLoc = c.String(),
                        IssueId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Issue", t => t.IssueId, cascadeDelete: true)
                .Index(t => t.IssueId);
            
            CreateTable(
                "dbo.Issue",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        ClosedDate = c.DateTime(),
                        InitiatedBy = c.Int(nullable: false),
                        ChangedBy = c.String(maxLength: 25),
                        EmployeeName = c.String(maxLength: 50),
                        RefIsoProc = c.String(maxLength: 25),
                        Correction = c.Boolean(nullable: false),
                        CarNo = c.String(maxLength: 25),
                        PurchaseOrderNo = c.String(maxLength: 25),
                        Vendor = c.String(maxLength: 25),
                        Customer = c.String(maxLength: 25),
                        OrderEntryNo = c.String(maxLength: 25),
                        Carrier = c.String(maxLength: 25),
                        Description = c.String(maxLength: 2500),
                        Action = c.String(maxLength: 2500),
                        QMNote = c.String(maxLength: 2500),
                        CostErr = c.Boolean(nullable: false),
                        Quality1 = c.Boolean(nullable: false),
                        Quality2 = c.Boolean(nullable: false),
                        Quality3 = c.Boolean(nullable: false),
                        Quality4 = c.Boolean(nullable: false),
                        CustChange = c.Boolean(nullable: false),
                        ShipDateErr = c.Boolean(nullable: false),
                        CustMistake = c.Boolean(nullable: false),
                        Error1 = c.Boolean(nullable: false),
                        Error2 = c.Boolean(nullable: false),
                        Error3 = c.Boolean(nullable: false),
                        Error4 = c.Boolean(nullable: false),
                        InstErr = c.Boolean(nullable: false),
                        PriceErr = c.Boolean(nullable: false),
                        AddressErr = c.Boolean(nullable: false),
                        Duplicate = c.Boolean(nullable: false),
                        SOPOther = c.Boolean(nullable: false),
                        DelNoApp = c.Boolean(nullable: false),
                        DelTime = c.Boolean(nullable: false),
                        DelShortShip = c.Boolean(nullable: false),
                        DelOverShip = c.Boolean(nullable: false),
                        PackNotAsOrdered = c.Boolean(nullable: false),
                        PackNotBilingual = c.Boolean(nullable: false),
                        PackDirty = c.Boolean(nullable: false),
                        MarkNotAsReq = c.Boolean(nullable: false),
                        MarkTooSmall = c.Boolean(nullable: false),
                        NotWhereReq = c.Boolean(nullable: false),
                        DamagePallet = c.Boolean(nullable: false),
                        DamageBoxesCrush = c.Boolean(nullable: false),
                        DamageInTransit = c.Boolean(nullable: false),
                        PalletNotpalletized = c.Boolean(nullable: false),
                        PalletRcvIncorrect = c.Boolean(nullable: false),
                        WhOther = c.Boolean(nullable: false),
                        WMNotAssigned = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.History", "IssueId", "dbo.Issue");
            DropIndex("dbo.History", new[] { "IssueId" });
            DropTable("dbo.Issue");
            DropTable("dbo.History");
        }
    }
}
