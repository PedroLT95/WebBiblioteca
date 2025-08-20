namespace WebBiblioteca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbSetConfigSistema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConfiguracionSistemas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DiasPrestamo = c.Int(nullable: false),
                        DiasBloqueoPorVencimiento = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ConfiguracionSistemas");
        }
    }
}
