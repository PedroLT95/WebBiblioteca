namespace WebBiblioteca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BloqueoUsuariosHasta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "BloqueadoHasta", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "BloqueadoHasta");
        }
    }
}
