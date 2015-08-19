namespace EvilDuck.Platform.Core.PlaformMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adding_isExported_on_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tables", "IsExported", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tables", "IsExported");
        }
    }
}
