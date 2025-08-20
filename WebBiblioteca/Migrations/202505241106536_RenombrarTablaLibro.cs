namespace WebBiblioteca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenombrarTablaLibro : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Libroes", newName: "Libro");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Libro", newName: "Libroes");
        }
    }
}
