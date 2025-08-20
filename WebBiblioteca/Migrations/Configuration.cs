namespace WebBiblioteca.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading;
    using System.Timers;
    using WebBiblioteca.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebBiblioteca.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebBiblioteca.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // 1. Crear el rol "Administrador" si no existe
            if (!roleManager.RoleExists("Administrador"))
            {
                roleManager.Create(new IdentityRole("Administrador"));
            }

            // 2. Crear un usuario administrador por defecto (si no existe)
            var adminEmail = "admin@biblioteca.com";
            var adminPassword = "Admin@123456";

            var adminUser = userManager.FindByEmail(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    NombreCompleto = "Administrador Principal",
                    FechaRegistro = DateTime.Now
                };

                var result = userManager.Create(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    userManager.AddToRole(adminUser.Id, "Administrador");
                }
            }
            else
            {
                // Si el usuario existe pero no tiene el rol
                if (!userManager.IsInRole(adminUser.Id, "Administrador"))
                {
                    userManager.AddToRole(adminUser.Id, "Administrador");
                }
            }

            // 3. Cargar libros y categorías si no existen
            //if (!context.Libros.Any())
            //{
                var librosSeed = new[]
                {
            new { Categoria = "Romance", Titulo = "El duque y yo", Autor = "Julia Quinn", Anyo = 2000, ISBN = "9788490000000" },
            new { Categoria = "Romance", Titulo = "Seduciendo a Mr. Bridgerton", Autor = "Julia Quinn", Anyo = 2002, ISBN = "9788490000001" },
            new { Categoria = "Romance", Titulo = "A Sir Phillip, con amor", Autor = "Julia Quinn", Anyo = 2003, ISBN = "9788490000002" },
            new { Categoria = "Fantasía", Titulo = "Harry Potter y el misterio del príncipe", Autor = "J.K. Rowling", Anyo = 2005, ISBN = "9788478880000" },
            new { Categoria = "Fantasía", Titulo = "Harry Potter y la piedra filosofal", Autor = "J.K. Rowling", Anyo = 1997, ISBN = "9788478880001" },
            new { Categoria = "Fantasía", Titulo = "Harry Potter y las reliquias de la Muerte", Autor = "J.K. Rowling", Anyo = 2007, ISBN = "9788478880002" },
            new { Categoria = "Fantasía", Titulo = "Harry Potter y la Orden del Fénix", Autor = "J.K. Rowling", Anyo = 2003, ISBN = "9788478880003" },
            new { Categoria = "Fantasía", Titulo = "Harry Potter y el cáliz de fuego", Autor = "J.K. Rowling", Anyo = 2000, ISBN = "9788478880004" },
            new { Categoria = "Fantasía", Titulo = "Harry Potter y el prisionero de Azkaban", Autor = "J.K. Rowling", Anyo = 1999, ISBN = "9788478880005" },
            new { Categoria = "Fantasía", Titulo = "Harry Potter y la cámara secreta", Autor = "J.K. Rowling", Anyo = 1998, ISBN = "9788478880006" },
            new { Categoria = "Terror", Titulo = "Misery", Autor = "Stephen King", Anyo = 1987, ISBN = "9780451169525" },
            new { Categoria = "Terror", Titulo = "El misterio de Salem's Lot", Autor = "Stephen King", Anyo = 1975, ISBN = "9780307743671" },
            new { Categoria = "Terror", Titulo = "Cementerio de animales", Autor = "Stephen King", Anyo = 1983, ISBN = "9780451172440" },
            new { Categoria = "Terror", Titulo = "En las montañas de la locura", Autor = "H.P. Lovecraft", Anyo = 1936, ISBN = "9788490000003" },
            new { Categoria = "Fantasía", Titulo = "El señor de los anillos: La comunidad del anillo", Autor = "J.R.R. Tolkien", Anyo = 1954, ISBN = "9780618640157" },
            new { Categoria = "Fantasía", Titulo = "El señor de los anillos: Las dos torres", Autor = "J.R.R. Tolkien", Anyo = 1954, ISBN = "9780618640188" },
            new { Categoria = "Fantasía", Titulo = "El señor de los anillos: El retorno del rey", Autor = "J.R.R. Tolkien", Anyo = 1955, ISBN = "9780618640201" },
            new { Categoria = "Fantasía", Titulo = "El resplandor", Autor = "Stephen King", Anyo = 1977, ISBN = "9780307743657" },
            new { Categoria = "Misterio", Titulo = "El guardián invisible", Autor = "Dolores Redondo", Anyo = 2013, ISBN = "9788423341989" },
            new { Categoria = "Misterio", Titulo = "El silencio de la ciudad blanca", Autor = "Eva García Sáenz de Urturi", Anyo = 2016 , ISBN = "9788491291100" },
            new { Categoria = "Thriller", Titulo = "Reina Roja", Autor = "Juan Gómez-Jurado", Anyo = 2018, ISBN = "9788466664417" },
            new { Categoria = "Misterio", Titulo = "La sombra del viento", Autor = "Carlos Ruiz Zafón", Anyo = 2001, ISBN = "9788408172177" },
            new { Categoria = "Fantasía", Titulo = "El nombre del viento", Autor = "Patrick Rothfuss", Anyo = 2007, ISBN = "9788401352835" },
            new { Categoria = "Fantasía", Titulo = "El príncipe de la niebla", Autor = "Carlos Ruiz Zafón", Anyo = 1993, ISBN = "9788408072803" },
            new { Categoria = "Thriller", Titulo = "El código Da Vinci", Autor = "Dan Brown", Anyo = 2003, ISBN = "9788408172177" },
            new { Categoria = "Romance", Titulo = "Orgullo y prejuicio", Autor = "Jane Austen", Anyo = 1813, ISBN = "9788491052610" },
            new { Categoria = "Ficción", Titulo = "Los pilares de la tierra", Autor = "Ken Follett", Anyo = 1989, ISBN = "9780451166890" },
            new { Categoria = "Fantasía", Titulo = "Alicia en el país de las maravillas", Autor = "Lewis Carroll", Anyo = 1865, ISBN = "9788491052611" },
            new { Categoria = "Histórica", Titulo = "Devoradores de cadáveres", Autor = "Michael Crichton", Anyo = 1976, ISBN = "9780060873004" },
            new { Categoria = "Histórica", Titulo = "La catedral", Autor = "César Mallorquí ", Anyo = 2004, ISBN = "9788423678344" },
            new { Categoria = "Histórica", Titulo = "La cruz del dorado", Autor = "José Calvo Poyato", Anyo = 2001, ISBN = "9788408054183" },
            new { Categoria = "Misterio", Titulo = "El retrato de Carlota", Autor = "Ana Alcolea", Anyo = 2003, ISBN = "9788426369652" },
            new { Categoria = "Thriller", Titulo = "La cara norte del corazón", Autor = "Dolores Redondo", Anyo = 2019, ISBN = "9788423355580" },
            new { Categoria = "Misterio", Titulo = "Todo esto te daré", Autor = "Dolores Redondo", Anyo = 2016, ISBN = "9788423350844" },
            new { Categoria = "Thriller", Titulo = "La chica del tre", Autor = "Paula Hawkins", Anyo = 2015, ISBN = "9788408141470" },
            new { Categoria = "Terror", Titulo = "Una cabeza llena de fantasmas", Autor = "Paul Tremblay", Anyo = 2015, ISBN = "9788491043466" },
            new { Categoria = "Terror", Titulo = "La cabaña del fin del mundo", Autor = "Paul Tremblay", Anyo = 2018, ISBN = "9788491043467" },
            new { Categoria = "Ciencia ficción", Titulo = "Dune", Autor = "Frank Herbert ", Anyo = 1965, ISBN = "9780441172719" },
            new { Categoria = "Ciencia ficción", Titulo = "Un mundo feliz", Autor = "Aldous Huxley", Anyo = 1923, ISBN = "9780060850524" },
            new { Categoria = "Ciencia ficción", Titulo = "1984", Autor = "George Orwell ", Anyo = 1949, ISBN = "9780451524935" },
            new { Categoria = "Sátira política", Titulo = "Rebelión en la granja", Autor = "George Orwell", Anyo = 1945 , ISBN = "9780451526342" },
            new { Categoria = "Ciencia ficción", Titulo = "Soy leyenda", Autor = "Richard Matheson", Anyo = 1954, ISBN = "9780575094161" },
            new { Categoria = "Terror", Titulo = "La mujer de negro", Autor = "Susan Hill", Anyo = 1983, ISBN = "9780307745316" },
            new { Categoria = "Histórica", Titulo = "La ladrona de libros", Autor = "Markus Zusak", Anyo = 2005, ISBN = "9780375842207" },
            new { Categoria = "Terror", Titulo = "Guerra Mundial Z", Autor = "Max Brooks", Anyo = 2006, ISBN = "9780307346612" },
            new { Categoria = "Ciencia ficción", Titulo = "El cuento de la criada", Autor = "Margaret Atwood", Anyo = 1985, ISBN = "9788499890948" },
            new { Categoria = "Terror", Titulo = "El aspecto del diablo", Autor = "Tomás Bárbulo", Anyo = 2019, ISBN = "9788490667631" },
            new { Categoria = "Terror", Titulo = "Amigo imaginario", Autor = "Stephen Chbosky", Anyo = 2019, ISBN = "9781538731338" },
            new { Categoria = "Terror", Titulo = "Los chicos del valle", Autor = "Philip Fracassi", Anyo = 2023, ISBN = "9788418037365" },
            new { Categoria = "Ciencia Ficción", Titulo = "La larga marcha", Autor = "Stephen King", Anyo = 1979, ISBN = "9780451197962" },
            new { Categoria = "Thriller", Titulo = "La novia roja", Autor = "Carmen Posadas", Anyo = 2013, ISBN = "9788401342102" },
            new { Categoria = "Ciencia ficción", Titulo = "El astronauta de Bohemia", Autor = "Jaroslav Kalfař", Anyo = 2017 , ISBN = "9788439733522" },
            new { Categoria = "Thriller", Titulo = "El cuarto mono", Autor = "J.D. Barker", Anyo = 2017, ISBN = "9788401019783" },
            new { Categoria = "Terror", Titulo = "La casa infernal", Autor = "Richard Matheson", Anyo = 1971, ISBN = "9788435017749" },
            new { Categoria = "Ficción", Titulo = "Las indignas", Autor = "María Oruña", Anyo = 2021, ISBN = "9788408245697" },
            new { Categoria = "Biografía", Titulo = "Figuras ocultas", Autor = "Margot Lee Shetterly", Anyo = 2016, ISBN = "9780062363602" },
            new { Categoria = "Fantasía", Titulo = "Alas de sangre", Autor = "Rebecca Yarros", Anyo = 2023, ISBN = "9788418037366" },
            new { Categoria = "Terror", Titulo = "Frankenstein", Autor = "Mary Shelley", Anyo = 1818, ISBN = "9788491052610" },
            new { Categoria = "Romance", Titulo = "Luna nueva", Autor = "Stephenie Meyer", Anyo = 2006, ISBN = "9788498000000" },
            new { Categoria = "Romance", Titulo = "Crepúsculo", Autor = "Stephenie Meyer", Anyo = 2005, ISBN = "9788498000001" },
            new { Categoria = "Misterio", Titulo = "El juego del ángel", Autor = "Carlos Ruiz Zafón", Anyo = 2008, ISBN = "9788408081189" },
            new { Categoria = "Histórica", Titulo = "Aquitania", Autor = "Eva García Sáenz de Urturi", Anyo = 2020, ISBN = "9788408239283" },
            new { Categoria = "Histórica", Titulo = "Los cuatro pilares", Autor = "Ken Follett", Anyo = 2020, ISBN = "9788408239290" },
            new { Categoria = "Histórica", Titulo = "Una fortuna peligrosa", Autor = "Ken Follett", Anyo = 1993, ISBN = "9780451173973" },
            new { Categoria = "Histórica", Titulo = "Violeta", Autor = "Isabel Allende", Anyo = 2020, ISBN = "9780063021792" },
            new { Categoria = "Realismo mágico", Titulo = "Cien años de soledad", Autor = "Gabriel García Márquez", Anyo = 1967, ISBN = "9788437604947" },
            new { Categoria = "Ficción", Titulo = "Kafka en la orilla", Autor = "Haruki Murakami", Anyo = 2002, ISBN = "9788498381492" },
            new { Categoria = "Ficción", Titulo = "Tokyo Blues", Autor = "Haruki Murakami", Anyo = 1987, ISBN = "9788498381508" },
            new { Categoria = "Terror", Titulo = "La noche del muñeco viviente", Autor = "R.L. Stine", Anyo = 1993, ISBN = "9780590568921" } 
                };


                foreach (var libro in librosSeed)
                {
                    var categoria = context.Categorias.FirstOrDefault(c => c.Nombre == libro.Categoria);
                    if (categoria == null)
                    {
                        categoria = new Categoria { Nombre = libro.Categoria };
                        context.Categorias.Add(categoria);
                        context.SaveChanges();
                    }

                    context.Libros.Add(new Libro
                    {
                        Titulo = libro.Titulo,
                        Autor = libro.Autor,
                        Anyo = libro.Anyo,
                        ISBN = libro.ISBN,
                        CategoriaId = categoria.Id,
                        Stock = 5
                    });
                }

                context.SaveChanges();
            //}

        }
    }
}
