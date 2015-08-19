namespace EvilDuck.Platform.Core.PlaformMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Columns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Caption = c.String(),
                        Type = c.Int(nullable: false),
                        Length = c.Int(nullable: false),
                        DefaultValue = c.String(),
                        IsRelation = c.Boolean(nullable: false),
                        RelationTable = c.String(),
                        RelationColumn = c.String(),
                        IsKey = c.Boolean(nullable: false),
                        AutoincrementKey = c.Boolean(nullable: false),
                        RowVersion = c.Binary(),
                        LastUpdateBy = c.String(),
                        CreatedBy = c.String(),
                        DeletedBy = c.String(),
                        LastUpdateOn = c.DateTime(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        DeletedOn = c.DateTime(),
                        ReferenceId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Table_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tables", t => t.Table_Id)
                .Index(t => t.Table_Id);
            
            CreateTable(
                "dbo.Queries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Caption = c.String(),
                        QueryText = c.String(),
                        QueryParams = c.String(),
                        Type = c.Int(nullable: false),
                        RowVersion = c.Binary(),
                        LastUpdateBy = c.String(),
                        CreatedBy = c.String(),
                        DeletedBy = c.String(),
                        LastUpdateOn = c.DateTime(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        DeletedOn = c.DateTime(),
                        ReferenceId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Repository_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Repositories", t => t.Repository_Id)
                .Index(t => t.Repository_Id);
            
            CreateTable(
                "dbo.Repositories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Caption = c.String(),
                        RowVersion = c.Binary(),
                        LastUpdateBy = c.String(),
                        CreatedBy = c.String(),
                        DeletedBy = c.String(),
                        LastUpdateOn = c.DateTime(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        DeletedOn = c.DateTime(),
                        ReferenceId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        DeleteQuery_Id = c.Int(),
                        InsertQuery_Id = c.Int(),
                        Table_Id = c.Int(),
                        UpdateQuery_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Queries", t => t.DeleteQuery_Id)
                .ForeignKey("dbo.Queries", t => t.InsertQuery_Id)
                .ForeignKey("dbo.Tables", t => t.Table_Id)
                .ForeignKey("dbo.Queries", t => t.UpdateQuery_Id)
                .Index(t => t.DeleteQuery_Id)
                .Index(t => t.InsertQuery_Id)
                .Index(t => t.Table_Id)
                .Index(t => t.UpdateQuery_Id);
            
            CreateTable(
                "dbo.Tables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Caption = c.String(),
                        RowVersion = c.Binary(),
                        LastUpdateBy = c.String(),
                        CreatedBy = c.String(),
                        DeletedBy = c.String(),
                        LastUpdateOn = c.DateTime(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        DeletedOn = c.DateTime(),
                        ReferenceId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Repositories", "UpdateQuery_Id", "dbo.Queries");
            DropForeignKey("dbo.Repositories", "Table_Id", "dbo.Tables");
            DropForeignKey("dbo.Columns", "Table_Id", "dbo.Tables");
            DropForeignKey("dbo.Queries", "Repository_Id", "dbo.Repositories");
            DropForeignKey("dbo.Repositories", "InsertQuery_Id", "dbo.Queries");
            DropForeignKey("dbo.Repositories", "DeleteQuery_Id", "dbo.Queries");
            DropIndex("dbo.Repositories", new[] { "UpdateQuery_Id" });
            DropIndex("dbo.Repositories", new[] { "Table_Id" });
            DropIndex("dbo.Repositories", new[] { "InsertQuery_Id" });
            DropIndex("dbo.Repositories", new[] { "DeleteQuery_Id" });
            DropIndex("dbo.Queries", new[] { "Repository_Id" });
            DropIndex("dbo.Columns", new[] { "Table_Id" });
            DropTable("dbo.Tables");
            DropTable("dbo.Repositories");
            DropTable("dbo.Queries");
            DropTable("dbo.Columns");
        }
    }
}
