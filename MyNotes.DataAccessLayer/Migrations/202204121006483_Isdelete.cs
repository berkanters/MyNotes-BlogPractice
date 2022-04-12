namespace MyNotes.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Isdelete : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblMyNotesUsers", "IsDelete", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblMyNotesUsers", "IsDelete");
        }
    }
}
