namespace WebBiblioteca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModeloPrestamoYConfiguracion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prestamoes", "FechaSolicitud", c => c.DateTime());
            AddColumn("dbo.Prestamoes", "Incidencia", c => c.Boolean(nullable: false));
            AddColumn("dbo.Prestamoes", "DuracionDias", c => c.Int(nullable: false));
            AlterColumn("dbo.Prestamoes", "FechaPrestamo", c => c.DateTime());
            DropColumn("dbo.Prestamoes", "Estado");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Prestamoes", "Estado", c => c.String());
            AlterColumn("dbo.Prestamoes", "FechaPrestamo", c => c.DateTime(nullable: false));
            DropColumn("dbo.Prestamoes", "DuracionDias");
            DropColumn("dbo.Prestamoes", "Incidencia");
            DropColumn("dbo.Prestamoes", "FechaSolicitud");
        }
    }
}
