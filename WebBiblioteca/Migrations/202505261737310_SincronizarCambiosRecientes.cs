namespace WebBiblioteca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SincronizarCambiosRecientes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Prestamo", "UsuarioId", "dbo.AspNetUsers");
            DropIndex("dbo.Prestamo", new[] { "UsuarioId" });
            AlterColumn("dbo.Prestamo", "UsuarioId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Prestamo", "UsuarioId");
            AddForeignKey("dbo.Prestamo", "UsuarioId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prestamo", "UsuarioId", "dbo.AspNetUsers");
            DropIndex("dbo.Prestamo", new[] { "UsuarioId" });
            AlterColumn("dbo.Prestamo", "UsuarioId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Prestamo", "UsuarioId");
            AddForeignKey("dbo.Prestamo", "UsuarioId", "dbo.AspNetUsers", "Id");
        }
    }
}
