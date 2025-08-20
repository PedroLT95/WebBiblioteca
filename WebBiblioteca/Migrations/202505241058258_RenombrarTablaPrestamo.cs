namespace WebBiblioteca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenombrarTablaPrestamo : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Prestamoes", newName: "Prestamo");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Prestamo", newName: "Prestamoes");
        }
    }
}
